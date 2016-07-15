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
            string ns = "QuickUnity";
            // System
            add(typeof(Enum), null);

            // Event
            add(typeof(EngineEvent), ns);
            add(typeof(EventManager), ns);
            add(typeof(EventTrigger), ns);
            add(typeof(UGUIEventTrigger), ns);


            // Quick Manager
            add(typeof(QuickManager), ns);

            // Lua
            add(typeof(LuaEngine), ns);
            add(typeof(QList), ns);
            add(typeof(QDictionary), ns);
			add(typeof(QBytes), ns);
            
            // Base
            add(typeof(SymbolManager), ns);
            add(typeof(QuickBehaviour), ns);


            // File
            add(typeof(FileManager), ns);
            add(typeof(UserDefault), ns);

            // Action
            add(typeof(Action), ns);
            add(typeof(Action.ActionBase), ns);
            add(typeof(Action.FiniteAction), ns);
            add(typeof(Action.IntervalAction), ns);
            add(typeof(Action.CallFunc), ns);
            add(typeof(Action.Sequence), ns);
            add(typeof(Action.Spawn), ns);
            add(typeof(Action.Delay), ns);
            add(typeof(Action.Repeat), ns);
            add(typeof(Action.MoveBy), ns);
            add(typeof(Action.MoveTo), ns);
            add(typeof(Action.RotateBy), ns);
            add(typeof(Action.RotateTo), ns);
            add(typeof(Action.RotateAround), ns);
            add(typeof(Action.ScaleBy), ns);
            add(typeof(Action.ScaleTo), ns);
            add(typeof(Action.Blink), ns);
            add(typeof(Action.ColorBy), ns);
            add(typeof(Action.ColorTo), ns);
            add(typeof(Action.FadeTo), ns);
            add(typeof(Action.FadeIn), ns);
            add(typeof(Action.FadeOut), ns);
            add(typeof(Action.Shake), ns);

            // Task
            add(typeof(Task), ns);
            add(typeof(CoroutineTask), ns);
            add(typeof(AsyncTask), ns);
            add(typeof(WWWTask), ns);
            add(typeof(WWWReadBytesTask), ns);
            add(typeof(WWWReadTextTask), ns);
            add(typeof(WWWRequest), ns);
            add(typeof(HttpDownloadTask), ns);

            // UI
            add(typeof(SymbolWidget), ns);
            add(typeof(SymbolImage), ns);
            add(typeof(SymbolText), ns);

            // Lua
            add(typeof(QLuaUtility), ns);
            add(typeof(LuaComponentType), ns);
            add(typeof(LuaComponent), ns);
            add(typeof(LuaComponent_Update), ns);
            add(typeof(LuaComponent_LateUpdate), ns);
            add(typeof(LuaComponent_FixedUpdate), ns);
            add(typeof(LuaComponent_Update_Late), ns);
            add(typeof(LuaComponent_Update_Fixed), ns);
            add(typeof(LuaComponent_Late_Fixed), ns);
            add(typeof(LuaComponent_Update_Late_Fixed), ns);
            add(typeof(SafeLuaCoroutine), ns);
            add(typeof(CustomTask), ns);

            // AssetManager
            add(typeof(AssetManager), ns);
            add(typeof(AssetManager.DownloadAssetBundleTask), ns);
            add(typeof(AssetManager.LoadAssetBundleTask), ns);
            add(typeof(AssetManager.LoadAssetTask), ns);
            add(typeof(AssetUnloadLevel), ns);


            add(typeof(AssetManager.LoadSpecifyAssetTask), ns);
            add(typeof(AssetManager.LoadFontTask), ns);
            add(typeof(AssetManager.LoadSpriteTask), ns);
            add(typeof(AssetManager.LoadTextureTask), ns);
            add(typeof(AssetManager.LoadTextTask), ns);
            add(typeof(AssetManager.LoadBinaryTask), ns);

            // HttpManager
            add(typeof(HttpManager), ns);

            // Network
            add(typeof(SocketBase), ns);
            add(typeof(SocketClient), ns);
            add(typeof(SocketManager), ns);
            
        }

    }
}
