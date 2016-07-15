using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;


namespace QuickUnity
{
    public class BMFontEditor : EditorWindow
    {
        [MenuItem("QuickUnity/UI/BMFont Maker")]
        static public void OpenBMFontMaker()
        {
            EditorWindow.GetWindow<BMFontEditor>(false, "BMFont Maker", true).Show();
        }

        [MenuItem("Assets/BMFont Maker/Process Fnt")]
        static public void ProcessFnt()
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (Path.GetExtension(assetPath) != ".fnt")
            {
                Debug.LogError("Need to select fnt file");
                return;
            }
            string matPath = Path.ChangeExtension(assetPath, ".mat");
            string pngPath = Path.ChangeExtension(assetPath, ".png");
            string fontPath = Path.ChangeExtension(assetPath, ".fontsettings");


            TextAsset fntData = Selection.activeObject as TextAsset;
            Font font = AssetDatabase.LoadMainAssetAtPath(fontPath) as Font;
            Material mat = AssetDatabase.LoadMainAssetAtPath(matPath) as Material;
            Texture2D texture = AssetDatabase.LoadMainAssetAtPath(pngPath) as Texture2D;

            if (font == null) { Debug.LogError("Can not find " + fontPath); return; }
            if (mat == null) { Debug.LogError("Can not find " + matPath); return; }
            if (texture == null) { Debug.LogError("Can not find " + pngPath); return; }

            Process(fntData, font, mat, texture);
        }

        static protected void Process(TextAsset fntData, Font font, Material mat, Texture2D texture)
        {
            BMFont bmFont = new BMFont();

            BMFontReader.Load(bmFont, fntData.name, fntData.bytes); // 借用NGUI封装的读取类
            CharacterInfo[] characterInfo = new CharacterInfo[bmFont.glyphs.Count];
            for (int i = 0; i < bmFont.glyphs.Count; i++)
            {
                BMGlyph bmInfo = bmFont.glyphs[i];
                CharacterInfo info = new CharacterInfo();
                info.index = bmInfo.index;

                Rect uv = new Rect(
                    (float)bmInfo.x / (float)bmFont.texWidth,
                    (float)(bmFont.texHeight - bmInfo.height) / (float)bmFont.texHeight,
                    (float)bmInfo.width / (float)bmFont.texWidth,
                    (float)bmInfo.height / (float)bmFont.texHeight);
                info.uvBottomLeft = new Vector2(uv.xMin, uv.yMin);
                info.uvBottomRight = new Vector2(uv.xMax, uv.yMin);
                info.uvTopLeft = new Vector2(uv.xMin, uv.yMax);
                info.uvTopRight = new Vector2(uv.xMax, uv.yMax);


                info.minX = 0;
                info.minY = -bmInfo.height;//-bmInfo.height / 2;
                info.maxX = bmInfo.width;
                info.maxY = 0;// bmInfo.height - bmInfo.height / 2;
                info.advance = bmInfo.advance;

                info.glyphWidth = bmInfo.width;
                info.glyphHeight = bmInfo.height;

                characterInfo[i] = info;
            }
            font.characterInfo = characterInfo;
            if (mat)
            {
                mat.mainTexture = texture;
            }
            font.material = mat;

            if (mat)
            {
                mat.shader = Shader.Find("UI/Default");//这一行很关键，如果用standard的shader，放到Android手机上，第一次加载会很慢
            }


            Debug.Log("Font size " + font.fontSize);
            Debug.Log("Font lineHeight " + font.lineHeight);
            Debug.Log("Font ascent " + font.ascent);

            EditorUtility.SetDirty(font);
            AssetDatabase.SaveAssets();
            Debug.Log("create font <" + font.name + "> success");
        }

        [SerializeField]
        private Font targetFont;
        [SerializeField]
        private TextAsset fntData;
        [SerializeField]
        private Material fontMaterial;
        [SerializeField]
        private Texture2D fontTexture;



        public BMFontEditor()
        {
        }

        void OnGUI()
        {
            targetFont = EditorGUILayout.ObjectField("Target Font", targetFont, typeof(Font), false) as Font;
            fntData = EditorGUILayout.ObjectField("Fnt Data", fntData, typeof(TextAsset), false) as TextAsset;
            fontMaterial = EditorGUILayout.ObjectField("Font Material", fontMaterial, typeof(Material), false) as Material;
            fontTexture = EditorGUILayout.ObjectField("Font Texture", fontTexture, typeof(Texture2D), false) as Texture2D;

            if (GUILayout.Button("Create BMFont"))
            {
                Process(fntData, targetFont, fontMaterial, fontTexture);
                Close();
            }
        }
    }
}
