using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace QuickUnity
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class SymbolImage : SymbolWidget
    {
        public Image image;
        public string symbolImage;
        public bool loadFinishEnable = true;
        protected CustomTask loadTask { get; set; }

        protected virtual void Awake()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
                if (image == null) return;
            }

            SetSymbolImage(symbolImage);
        }


        public Task SetSymbolImage(string symbol) { return SetSymbolImage(libraryName, symbol); }

        public Task SetSymbolImage(string libName, string symbol)
        {
            this.libraryName = libName;
            this.symbolImage = symbol;

            string[] images = SymbolManager.Translates(libName, symbol);
            string name = images.Length > 0 ? images[0] : null;
            string subName = images.Length > 1 ? images[1] : null;
            return SetImage(name, subName);
        }

        public Task SetImage(string name) { return SetImage(name, null); }

        public Task SetImage(string name, string subName)
        {
            if (loadTask != null) return loadTask;
            loadTask = new CustomTask();
            StartCoroutine(Load(name, subName));
            return loadTask;
        }


        protected override string GetLibraryName() { return QConfig.Symbol.assetLibrary; }

        protected override void OnUpdateSymbol()
        {
            SetSymbolImage(symbolImage);
        }


        protected IEnumerator Load(string name, string subName)
        {
            string error = string.Empty;
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
                else
                {
                    error = task.error;
                }

            } while (false);

            var t = loadTask;
            loadTask = null;
            if (string.IsNullOrEmpty(error))
            {
                t.SetFail(error);
            }
            else
            {
                t.SetSuccess();
            }
        }

        
    }
}


