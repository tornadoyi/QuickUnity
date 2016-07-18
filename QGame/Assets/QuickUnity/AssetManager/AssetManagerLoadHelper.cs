using UnityEngine;
using System.Collections;

namespace QuickUnity
{

    public partial class AssetManager : BaseManager<AssetManager>
    {
        public static AudioClip LoadAudioClip(string name)
        {
            var asset = LoadAsset(name);
            return asset != null ? asset as AudioClip : null;
        }

        public static LoadAudioClipTask LoadAudioClipAsync(string name)
        {
            var task = new LoadAudioClipTask(name);
            task.Start();
            return task;
        }

        public static Font LoadFont(string name)
        {
            var asset = LoadAsset(name);
            return asset != null ? asset as Font : null;
        }

        public static LoadFontTask LoadFontAsync(string name)
        {
            var task = new LoadFontTask(name);
            task.Start();
            return task;
        }

        public static Sprite LoadSprite(string name)
        {
            var subName = System.IO.Path.GetFileNameWithoutExtension(name);
            return LoadSprite(name, subName);
        }

        public static Sprite LoadSprite(string name, string subName)
        {
            var subAssets = LoadSubAssets(name);
            if (subAssets == null) return null;
            for(int i=0; i< subAssets.Length; ++i)
            {
                Object asset = subAssets[i];
                if (!(asset is Sprite) || asset.name != subName) continue;
                return asset as Sprite;
            }
            return null;
        }

        public static LoadSpriteTask LoadSpriteAsync(string name)
        {
            return LoadSpriteAsync(name, null);
        }

        public static LoadSpriteTask LoadSpriteAsync(string name, string subName)
        {
            var task = new LoadSpriteTask(name, subName);
            task.Start();
            return task;
        }

        public static Texture2D LoadTexture(string name)
        {
            var asset = LoadAsset(name);
            return asset != null ? asset as Texture2D : null;
        }

        public static LoadTextureTask LoadTextureAsync(string name)
        {
            var task = new LoadTextureTask(name);
            task.Start();
            return task;
        }

        public static string LoadText(string name)
        {
            var asset = LoadAsset(name);
            if (asset == null) return null;
            TextAsset textAsset = asset as TextAsset;
            if (textAsset == null) return null;
            return textAsset.text;
        }

        public static LoadTextTask LoadTextAsync(string name)
        {
            var task = new LoadTextTask(name);
            task.Start();
            return task;
        }

        public static byte[] LoadBinary(string name)
        {
            var asset = LoadAsset(name);
            if (asset == null) return null;
            TextAsset textAsset = asset as TextAsset;
            if (textAsset == null) return null;
            return textAsset.bytes;
        }

        public static LoadBinaryTask LoadBinaryAsync(string name)
        {
            var task = new LoadBinaryTask(name);
            task.Start();
            return task;
        }

        public static GameObject LoadGameObject(string name)
        {
            var asset = LoadAsset(name);
            if (asset == null) return null;
            return asset as GameObject;
        }

        public static LoadGameObjectTask LoadGameObjectAsync(string name)
        {
            var task = new LoadGameObjectTask(name);
            task.Start();
            return task;
        }



        #region Load asset task =======================================================================================

        public class LoadAudioClipTask : LoadSpecifyAssetTask
        {
            public AudioClip clip;

            public LoadAudioClipTask(string name) : base(name) { }

            protected override void OnProcessAsset(UnityEngine.Object asset)
            {
                clip = asset as AudioClip;
                if(clip == null)
                {
                    SetFail(string.Format("Asset type error, type is {0}, expect AudioClip", asset.GetType().Name));
                    return;
                }
            }
        }

        public class LoadFontTask : LoadSpecifyAssetTask
        {
            public Font font;

            public LoadFontTask(string name) : base(name) { }

            protected override void OnProcessAsset(UnityEngine.Object asset)
            {
                font = asset as Font;
                if(font == null)
                {
                    SetFail(string.Format("Asset type error, type is {0}, expect Font", asset.GetType().Name));
                    return;
                }
            }
        }

        public class LoadSpriteTask : LoadSpecifyAssetTask
        {
            public Sprite sprite;
            protected string subName;

            public LoadSpriteTask(string name, string subName) : base(name) { this.subName = subName; }

            protected override void OnProcessSubAssets(UnityEngine.Object[] subAssets)
            {
                string name = string.IsNullOrEmpty(subName) ? assetName : subName;
                name = System.IO.Path.GetFileNameWithoutExtension(name);
                for (int i=0; i<subAssets.Length; ++i)
                {
                    Object asset = subAssets[i];
                    if (!(asset is Sprite) || asset.name != name) continue;
                    sprite = asset as Sprite;
                    break;
                }
            }
        }

        public class LoadTextureTask : LoadSpecifyAssetTask
        {
            public Texture2D texture;

            public LoadTextureTask(string name) : base(name) { }

            protected override void OnProcessAsset(UnityEngine.Object asset)
            {
                texture = asset as Texture2D;
                if (texture == null)
                {
                    SetFail(string.Format("Asset type error, type is {0}, expect Texture2D", asset.GetType().Name));
                    return;
                }
            }
        }

        public class LoadTextTask : LoadSpecifyAssetTask
        {
            public string text;

            public LoadTextTask(string name) : base(name) { }

            protected override void OnProcessAsset(UnityEngine.Object asset)
            {
                TextAsset textAsset = asset as TextAsset;
                if (textAsset == null)
                {
                    SetFail(string.Format("Asset type error, type is {0}, expect TextAsset", asset.GetType().Name));
                    return;
                }
                text = textAsset.text;
            }
        }

        public class LoadBinaryTask : LoadSpecifyAssetTask
        {
            public byte[] bytes;

            public LoadBinaryTask(string name) : base(name) { }

            protected override void OnProcessAsset(UnityEngine.Object asset)
            {
                TextAsset textAsset = asset as TextAsset;
                if (textAsset == null)
                {
                    SetFail(string.Format("Asset type error, type is {0}, expect TextAsset", asset.GetType().Name));
                    return;
                }
                bytes = textAsset.bytes;
            }
            
        }

        public class LoadGameObjectTask : LoadSpecifyAssetTask
        {
            public GameObject gameObject { get; private set; }

            public LoadGameObjectTask(string name) : base(name) { }
            protected override void OnProcessAsset(UnityEngine.Object asset)
            {
                gameObject = asset as GameObject;
                if (gameObject == null)
                {
                    SetFail(string.Format("Asset type error, type is {0}, expect GameObject", asset.GetType().Name));
                    return;
                }
            }
        }

        public class LoadSpecifyAssetTask : CoroutineTask
        {
            protected string assetName;

            public LoadSpecifyAssetTask(string name)
            {
                assetName = name;
            }

            protected override IEnumerator OnProcess()
            {
                LoadAssetTask task = new LoadAssetTask(assetName);
                task.Start();
                yield return task.WaitForFinish();
                if(task.fail)
                {
                    SetFail(task.error);
                    yield break;
                }
                if (task.asset != null) OnProcessAsset(task.asset);
                if (task.subAssets != null) OnProcessSubAssets(task.subAssets);
                
            }

            protected virtual void OnProcessAsset(UnityEngine.Object asset) { }
            protected virtual void OnProcessSubAssets(UnityEngine.Object[] subAssets) { }

        }

        #endregion
    }
}