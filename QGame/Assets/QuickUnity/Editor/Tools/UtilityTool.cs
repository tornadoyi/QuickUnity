using UnityEngine;
using System.Collections;
using UnityEditor;

namespace QuickUnity
{
    public class UtilityTool
    {
        [MenuItem("QuickUnity/Utility/Open Persistent Data Path")]
        public static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("QuickUnity/Utility/Open Temporary Cache Path")]
        public static void OpenTemporaryCachePath()
        {
            EditorUtility.RevealInFinder(Application.temporaryCachePath);
        }
    }
     
}

