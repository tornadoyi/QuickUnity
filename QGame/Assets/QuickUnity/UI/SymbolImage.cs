using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace QuickUnity
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class SymbolImage : SymbolWidget
    {
        protected virtual void Awake()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
                if (image == null) return;
            }

            SetSymbolImageAsync(symbolImage);
        }

        protected override string GetLibraryName() { return QConfig.Symbol.assetLibrary; }

        public override IEnumerator WaitForDone()
        {
            if (co_load == null) yield break;
            yield return co_load;
        }

        public void SetSymbolImageAsync(string symbol) { SetSymbolImageAsync(libraryName, symbol); }

        public void SetSymbolImageAsync(string libName, string symbol) { SetSymbolImageAsync(libName, symbol, null); }

        public void SetSymbolImageAsync(string libName, string symbol, QEventHandler callback)
        {
            string[] images = SymbolManager.Translates(libName, symbol);
            string name = images.Length > 0 ? images[0] : null;
            string subName = images.Length > 1 ? images[1] : null;
            SetImageAsync(name, subName, callback);
        }

        public void SetImageAsync(string name) { SetImageAsync(name, null, null); }

        public void SetImageAsync(string name, QEventHandler callback) { SetImageAsync(name, null, callback); }

        public void SetImageAsync(string name, string subName) { SetImageAsync(name, subName, null); }

        public void SetImageAsync(string name, string subName, QEventHandler callback)
        {
            if(co_load != null) StopCoroutine(co_load);
            co_load = StartCoroutine(Load(name, subName, () => {
                co_load = null;
                if (callback != null) callback.Invoke();
            }));
        }

        protected IEnumerator Load(string name, string subName, QEventHandler callback)
        {
            do
            {
                if (string.IsNullOrEmpty(name)) break;

                if (image == null)
                {
                    Debug.LogError("No image for override");
                    break;
                }

                AssetManager.LoadSpriteTask task = null;
                if (string.IsNullOrEmpty(subName))
                {
                    task = AssetManager.LoadSpriteAsync(name);
                }
                else
                {
                    task = AssetManager.LoadSpriteAsync(name, subName);
                }
                
                yield return task.WaitForFinish();
                if (task.sprite != null)
                {
                    image.sprite = task.sprite;
                    if (loadFinishEnable) image.enabled = true;
                }

            } while (false);

            if (callback != null) callback.Invoke();
        }

        public Image image;
        public string symbolImage;
        public bool loadFinishEnable = true;
        protected Coroutine co_load;
    }
}


