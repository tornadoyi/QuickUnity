using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEditor.Animations;

namespace QuickUnity
{
    public class AnimatorCreator : EditorWindow
    {

        [MenuItem("QuickUnity/Tools/Animator Creator")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(AnimatorCreator));
        }

        void OnGUI()
        {
            animatorController = EditorGUILayout.ObjectField("Animator Controller", animatorController, typeof(AnimatorController), false) as AnimatorController;
            if (animatorController == null) return;

            AnimationClip[] clips = animatorController.animationClips;
            for(int i=0; i<clips.Length; ++i)
            {
                EditorGUILayout.TextField("Animation clip", clips[i].name);
            }
            
            using(QuickEditor.BeginHorizontal())
            {
                newAnimationName = EditorGUILayout.TextField("Name", newAnimationName);
                if(GUILayout.Button("Add Animation Clip"))
                {
                    if (!string.IsNullOrEmpty(newAnimationName))
                    {
                        var clip = AnimatorController.AllocateAnimatorClip(newAnimationName);
                        AssetDatabase.AddObjectToAsset(clip, animatorController);
                        animatorController.AddMotion(clip);
                        AssetDatabase.Refresh();
                        AssetDatabase.SaveAssets();
                        newAnimationName = "";
                    }
                }
            }
        }

        protected AnimatorController animatorController;
        protected string newAnimationName = "";
    }
}

