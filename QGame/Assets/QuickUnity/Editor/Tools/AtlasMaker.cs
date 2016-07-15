using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace QuickUnity
{
    public class AtlasMaker : EditorWindow
    {
        [MenuItem("QuickUnity/UI/Atlas Maker")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow<AtlasMaker>();
        }

        void OnSelectionChange()
        {
            //selectSpriteName = string.Empty;
            Repaint();
        }

        void OnGUI1()
        {
            if (GUILayout.Button("Text"))
            {
                string path = "Assets/Resources/UI/New Atlas.png";
                targetTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

                var updateDict = new Dictionary<string, Texture2D>();
                if (Selection.objects != null && Selection.objects.Length > 0)
                {
                    Object[] objects = Selection.objects;
                    foreach (Object o in objects)
                    {
                        Texture2D tex = o as Texture2D;
                        if (tex == null || tex.name == "Font Texture" || tex == targetTexture) continue;
                        updateDict[tex.name] = tex;
                        Debug.Log("Choose" + tex.name);
                    }
                }

                RepackTargetTexture(updateDict);
            }
            
        }

        void OnGUI()
        {
            // Texture
            var preTargetTexture = targetTexture;
            targetTexture = (Texture2D)EditorGUILayout.ObjectField(targetTexture, typeof(Texture2D), false);
            if (preTargetTexture != targetTexture || spriteDict.Count == 0) UpdateSpriteList();

            // Show current atlas size
            if(targetTexture != null)
            {
                var size = string.Format("{0}x{0}", (int)targetTexture.width, (int)targetTexture.height);
                EditorGUILayout.LabelField("Atlas Size", size);
            }

            // Padding
            padding = EditorGUILayout.IntField("Padding", padding);

            // Search text
            searchText = EditorGUILayout.TextField("", searchText, "SearchTextField");

            // Get update list
            var updateDict = new Dictionary<string, Texture2D>();
            if (Selection.objects != null && Selection.objects.Length > 0)
            {
                Object[] objects = Selection.objects;
                foreach (Object o in objects)
                {
                    Texture2D tex = o as Texture2D;
                    if (tex == null || tex.name == "Font Texture" || tex == targetTexture) continue;
                    updateDict[tex.name] = tex;
                }
            }

            // Show scroll view
            string exportSpriteName = string.Empty; string exportPath = string.Empty;
            string deleteSpriteName = string.Empty;
            if(targetTexture != null)
            {
                using (QuickEditor.BeginScrollView(ref scrollPos))
                {
                    // Show all packed sprite
                    int i = -1;
                    foreach (var it in spriteDict)
                    {
                        // Index ++
                        ++i;

                        // Search filter
                        if (it.Key.IndexOf(searchText, System.StringComparison.CurrentCultureIgnoreCase) == -1) continue;

                        // Set background color
                        var sprite = it.Value;
                        bool update = updateDict.ContainsKey(it.Key);

                        if (selectSpriteName == it.Key) { GUI.backgroundColor = Color.white; }
                        else if (update) GUI.backgroundColor = new Color(0.15f, 0.71f, 0.16f);
                        else { GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f); }

                        using (QuickEditor.BeginHorizontal("AS TextArea", GUILayout.MinHeight(20f)))
                        {
                            GUI.backgroundColor = Color.white;
                            GUILayout.Label(i.ToString(), GUILayout.Width(24f));
                            if (GUILayout.Button(it.Key, "OL TextField", GUILayout.Height(20f)))
                            {
                                selectSpriteName = it.Key;
                                Selection.activeObject = sprite;
                            }

                            if (GUILayout.Button("Export", "minibutton", GUILayout.Width(50f)))
                            {
                                var path = EditorUtility.SaveFilePanelInProject("Save As",
                                    "New.png", "png", "Save atlas as...", Application.dataPath);
                                if(!string.IsNullOrEmpty(path))
                                {
                                    exportSpriteName = it.Key;
                                    exportPath = path;
                                }
                            }

                            if (update)
                            {
                                GUILayout.Label("Update", GUILayout.Width(50f));
                            }
                            else if (GUILayout.Button("", "OL Minus", GUILayout.Width(22f)))
                            {
                                deleteSpriteName = it.Key;
                            }
                        }
                    }

                    // Show all new sprite
                    foreach (var it in updateDict)
                    {
                        ++i;
                        if (spriteDict.ContainsKey(it.Key)) continue;
                        GUI.backgroundColor = new Color(0.39f, 0.39f, 1.0f);
                        using (QuickEditor.BeginHorizontal("AS TextArea", GUILayout.MinHeight(20f)))
                        {
                            GUI.backgroundColor = Color.white;
                            GUILayout.Label(i.ToString(), GUILayout.Width(24f));
                            GUILayout.Label(it.Key, "OL TextField", GUILayout.Height(20f));
                            GUILayout.Label("Add", GUILayout.Width(30f));
                        }
                    }
                }
            }

            // Add, Delete or Export Sprite
            if (!string.IsNullOrEmpty(deleteSpriteName))
            {
                spriteDict.Remove(deleteSpriteName);
                RepackTargetTexture();
            }
            else if (!string.IsNullOrEmpty(exportSpriteName))
            {
                var sprite = spriteDict[exportSpriteName];
                ExportSprite(sprite, exportPath);
            }
            else if (GUILayout.Button("Add") && targetTexture != null)
            {
                RepackTargetTexture(updateDict);
            }
            

            if(GUILayout.Button("Create"))
            {
                string path = EditorUtility.SaveFilePanelInProject("Save As",
                    "New Atlas.png", "png", "Save atlas as...", Application.dataPath);

                if(!string.IsNullOrEmpty(path))
                {
                    var bytes = Texture2D.whiteTexture.EncodeToPNG();
                    System.IO.File.WriteAllBytes(path, bytes);
                    AssetDatabase.Refresh();

                    // Set file properties
                    TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
                    TextureImporterSettings settings = new TextureImporterSettings();
                    ti.ReadTextureSettings(settings);
                    settings.readable = false;
                    settings.mipmapEnabled = false;
                    settings.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                    ti.SetTextureSettings(settings);
                    ti.textureType = TextureImporterType.Sprite;
                    ti.spriteImportMode = SpriteImportMode.Multiple;
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
                }
            }
        }


        protected void UpdateSpriteList()
        {
            spriteDict.Clear();
            if (targetTexture == null) return;
            var path = AssetDatabase.GetAssetPath(targetTexture);

            var assets = AssetDatabase.LoadAllAssetsAtPath(path);
            for (int i=0; i<assets.Length; ++i)
            {
                if (assets[i] == targetTexture) continue;

                var sprite = assets[i] as Sprite;
                if (sprite == null)
                {
                    Debug.LogErrorFormat("{0} is {1}, not Sprite", assets[i].name, assets[i].GetType());
                    continue;
                }

                spriteDict[sprite.name] = sprite;
            }
        }


        protected void RepackTargetTexture(Dictionary<string, Texture2D> updateDict = null)
        {
            if (updateDict == null) updateDict = new Dictionary<string, Texture2D>();

            // Save old atlas settings
            string atlasPath = AssetDatabase.GetAssetPath(targetTexture);

            // Set atlas readable and RGBA32
            // Set readable for GetPixel 
            // Set RGBA32 for Get accurate pixel
            {
                var ti = AssetImporter.GetAtPath(atlasPath) as TextureImporter;
                var settings = new TextureImporterSettings();
                ti.ReadTextureSettings(settings);
                settings.readable = true;
                settings.textureFormat = TextureImporterFormat.RGBA32;
                ti.SetTextureSettings(settings);
                AssetDatabase.ImportAsset(atlasPath, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
            }

            // Merge old and new
            var textureList = new List<Texture2D>();
            var metaList = new List<SpriteMetaData>(); 
            foreach(var it in spriteDict)
            {
                // Need to update
                if (updateDict.ContainsKey(it.Key)) { continue; }

                // Create texture
                var sprite = it.Value;
                var rect = sprite.rect;
                var colors = targetTexture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
                var texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
                texture.alphaIsTransparency = true;
                texture.name = sprite.name;
                texture.SetPixels(colors);
                texture.Apply();
                textureList.Add(texture);

                // Meta
                var meta = new SpriteMetaData();
                meta.alignment = 0;
                meta.border = sprite.border;
                meta.pivot = sprite.pivot;
                meta.rect = sprite.rect;
                meta.name = sprite.name;
                metaList.Add(meta);

            }

            foreach(var it in updateDict)
            {
                var texture = it.Value;

                // Set readable, RGBA32 and npotScale
                var path = AssetDatabase.GetAssetPath(texture);
                TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
                TextureImporterSettings settings = new TextureImporterSettings();

                ti.ReadTextureSettings(settings);
                settings.readable = true;
                settings.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                settings.npotScale = TextureImporterNPOTScale.None;
                settings.alphaIsTransparency = true;
                ti.textureType = TextureImporterType.Advanced; // Can not set sprite, because it should reset readable flag by call ImportAsset
                ti.SetTextureSettings(settings);
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);

                // Texture
                texture.GetPixels32();
                textureList.Add(texture);

                // Meta
                var meta = new SpriteMetaData();
                meta.alignment = settings.spriteAlignment;
                meta.border = settings.spriteBorder;
                meta.pivot = settings.spritePivot;
                meta.name = it.Key;
                metaList.Add(meta);

            }

            // Pack texture to atlas
            Texture2D atlas = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            var rects = atlas.PackTextures(textureList.ToArray(), padding);

            // Save atlas
            byte[] bytes = atlas.EncodeToPNG();
            System.IO.File.WriteAllBytes(atlasPath, bytes);
            bytes = null;
           
            // Save meta and non-readable
            {
                var metas = CalculateSpriteSheet(atlas, metaList.ToArray(), rects);
                var ti = AssetImporter.GetAtPath(atlasPath) as TextureImporter;
                ti.spritesheet = metas;
                ti.isReadable = false;
                ti.textureType = TextureImporterType.Sprite;
                ti.spriteImportMode = SpriteImportMode.Multiple;
                AssetDatabase.ImportAsset(atlasPath, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
            }

            // Update all cache
            
            UpdateSpriteList();
            selectSpriteName = string.Empty;
            //Repaint();
        }

        protected void ExportSprite(Sprite sprite, string outputPath)
        {
            
        }

        protected SpriteMetaData[] CalculateSpriteSheet(Texture2D atlas, SpriteMetaData[] metas, Rect[] rects)
        {
            if(metas.Length != rects.Length)
            {
                Debug.LogError("Invalid arguments");
                return null;
            }
            SpriteMetaData[] smds = new SpriteMetaData[rects.Length];
            for (int i = 0; i < rects.Length; ++i)
            {
                smds[i] = metas[i];
                smds[i].rect = ConvertToPixels(rects[i], atlas.width, atlas.height, false);
            }
            return smds;
        }

        static public Rect ConvertToPixels(Rect rect, int width, int height, bool round)
        {
            Rect final = rect;

            if (round)
            {
                final.x = Mathf.RoundToInt(rect.xMin * width);
                final.y = Mathf.RoundToInt(rect.yMin * width);
                final.width = Mathf.RoundToInt(rect.width * height);
                final.height = Mathf.RoundToInt(rect.height * height);
            }
            else
            {
                final.x = rect.x * width;
                final.y = rect.y * height;
                final.width = rect.width * width;
                final.height = rect.height * height;
            }
            return final;
        }

        protected Texture2D targetTexture;
        protected Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
        protected int padding = 1;
        protected string searchText = string.Empty;

        private string selectSpriteName = string.Empty;
        private Vector2 scrollPos;

    }

}


