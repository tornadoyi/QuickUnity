using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SLua;
using UnityEditor;

namespace QuickUnity
{
    public class QLuaExporter : ICustomExportPost
    {
        public static void OnAddCustomClass(LuaCodeGen.ExportGenericDelegate add)
        {
            // System
            add(typeof(Enum), null);

            // Event
            add(typeof(EngineEvent), null);
            add(typeof(EventManager), null);
            add(typeof(EventTrigger), null);
            add(typeof(UGUIEventTrigger), null);


            // Quick Manager
            add(typeof(QuickManager), null);

            // Lua
            add(typeof(LuaEngine), null);
            add(typeof(QList), null);
            add(typeof(QDictionary), null);
			add(typeof(QBytes), null);
            
            // Base
            add(typeof(SymbolManager), null);
            add(typeof(QuickBehaviour), null);


            // File
            add(typeof(FileManager), null);
            add(typeof(UserDefault), null);

            // Action
            add(typeof(Action), null);
            add(typeof(Action.ActionBase), null);
            add(typeof(Action.FiniteAction), null);
            add(typeof(Action.IntervalAction), null);
            add(typeof(Action.CallFunc), null);
            add(typeof(Action.Sequence), null);
            add(typeof(Action.Spawn), null);
            add(typeof(Action.Delay), null);
            add(typeof(Action.Repeat), null);
            add(typeof(Action.MoveBy), null);
            add(typeof(Action.MoveTo), null);
            add(typeof(Action.RotateBy), null);
            add(typeof(Action.RotateTo), null);
            add(typeof(Action.RotateAround), null);
            add(typeof(Action.ScaleBy), null);
            add(typeof(Action.ScaleTo), null);
            add(typeof(Action.Blink), null);
            add(typeof(Action.ColorBy), null);
            add(typeof(Action.ColorTo), null);
            add(typeof(Action.FadeTo), null);
            add(typeof(Action.FadeIn), null);
            add(typeof(Action.FadeOut), null);
            add(typeof(Action.Shake), null);

            // Task
            add(typeof(Task), null);
            add(typeof(CoroutineTask), null);
            add(typeof(AsyncTask), null);
            add(typeof(WWWTask), null);
            add(typeof(WWWReadBytesTask), null);
            add(typeof(WWWReadTextTask), null);
            add(typeof(WWWRequest), null);
            add(typeof(HttpDownloadTask), null);

            // UI
            add(typeof(SymbolWidget), null);
            add(typeof(SymbolImage), null);
            add(typeof(SymbolText), null);

            // Lua
            add(typeof(QLuaUtility), null);
            add(typeof(LuaComponentType), null);
            add(typeof(LuaComponent), null);
            add(typeof(LuaComponent_Update), null);
            add(typeof(LuaComponent_LateUpdate), null);
            add(typeof(LuaComponent_FixedUpdate), null);
            add(typeof(LuaComponent_Update_Late), null);
            add(typeof(LuaComponent_Update_Fixed), null);
            add(typeof(LuaComponent_Late_Fixed), null);
            add(typeof(LuaComponent_Update_Late_Fixed), null);
            add(typeof(SafeLuaCoroutine), null);
            add(typeof(CustomTask), null);

            // AssetManager
            add(typeof(AssetManager), null);
            add(typeof(AssetManager.DownloadAssetBundleTask), null);
            add(typeof(AssetManager.LoadAssetBundleTask), null);
            add(typeof(AssetManager.LoadAssetTask), null);
            add(typeof(AssetUnloadLevel), null);


            add(typeof(AssetManager.LoadSpecifyAssetTask), null);
            add(typeof(AssetManager.LoadFontTask), null);
            add(typeof(AssetManager.LoadSpriteTask), null);
            add(typeof(AssetManager.LoadTextureTask), null);
            add(typeof(AssetManager.LoadTextTask), null);
            add(typeof(AssetManager.LoadBinaryTask), null);

            // HttpManager
            add(typeof(HttpManager), null);

            // Network
            add(typeof(SocketBase), null);
            add(typeof(SocketClient), null);
            add(typeof(SocketManager), null);
            
        }

    }
}
