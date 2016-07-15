using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;
using QuickUnity;

namespace QuickUnity
{
    public class XTypeInspector
    {
        public XTypeInspector(XType xType)
        {
            this.xType = xType;
        }

        public void Draw()
        {
            // Value
            if(!xType.isArray)
            {
                DrawValue(0);
                return;
                    
            }

            // Array
            DrawArray(0);

            EditorGUILayout.Space();
            using (QuickEditor.BeginHorizontal())
            {
                if (GUILayout.Button("Add Variable", GUILayout.Height(23f)))
                {
                    XTypeEditWindow.ShowCreateWindow((editXtype) =>
                    {
                        if (forbidNamelessXtype && string.IsNullOrEmpty(editXtype.name))
                        {
                            Debug.LogError("Need to set name variable");
                            return false;
                        }
                        if (repeatedNameCheckInArray && ExistRepeatedName(editXtype.name, 0))
                        {
                            Debug.LogError("Repeated variable name " + editXtype.name);
                            return false;
                        }
                        xType.Insert(xType.endIndex, editXtype);
                        return true;
                    });
                }

                string buttonText = editMode ? "Save" : "Edit";
                if (GUILayout.Button(buttonText, GUILayout.Height(23f), GUILayout.Width(50f)))
                {
                    editMode = !editMode;
                }
            }


            // Deal with command
            if(command != null)
            {
                if(command.type == CmdType.Remove)
                {
                    xType.RemoveAt(command.index);
                }
                else if(command.type == CmdType.MoveNext)
                {
                    xType.MoveNext(command.index);
                }
                else if(command.type == CmdType.MovePrevious)
                {
                    xType.MovePrevious(command.index);
                }
                command = null;
            }
        }


        void DrawValue(int index)
        {
            var viewer = xType.View(index);
            if(viewer.isUnityObject)
            {
                var type = Type.GetType(viewer.typeName);
                viewer.value = EditorGUILayout.ObjectField(viewer.name, viewer.value as UnityEngine.Object, type, true);
            }
            else
            {
                viewer.value = QuickEditor.DrawVariable(viewer.name, viewer.value);
            }
        }

        void DrawArray(int index)
        {
            var viewer = xType.View(index);

            // Foldout
            if (!drawWithoutOutermostArray || index != 0)
            {
                using (QuickEditor.BeginHorizontal())
                {
                    viewer.foldout = EditorGUILayout.Foldout(viewer.foldout, viewer.name);
                    if(editMode) DrawEditButtons(index);
                }
            }
            if (!viewer.foldout) return;
            
            // Loop
            var e = xType.GetEnumerator(index);
            while (e.MoveNext())
            {
                if (e.Current.isArray)
                {
                    DrawArray(e.Current.index); 
                }
                else
                {
                    using (QuickEditor.BeginHorizontal())
                    {
                        // Draw value
                        DrawValue(e.Current.index);
                        if(editMode) DrawEditButtons(e.Current.index);
                    }   
                }
            }

            // Button for add element
            if (!editMode || index == 0) return;
            EditorGUILayout.Space();
            using (QuickEditor.BeginHorizontal())
            {
                // For layout
                if (GUILayout.Button("", "Label")) { }

                // Add variable
                if (GUILayout.Button("Add Variable", "minibutton"))
                {
                    XTypeEditWindow.ShowCreateWindow((editXtype) =>
                    {
                        if (forbidNamelessXtype && string.IsNullOrEmpty(editXtype.name))
                        {
                            Debug.LogError("Need to set name variable");
                            return false;
                        }
                        if (repeatedNameCheckInArray && ExistRepeatedName(editXtype.name, 0))
                        {
                            Debug.LogError("Repeated variable name " + editXtype.name);
                            return false;
                        }
                        xType.Insert(e.Current.index, editXtype);
                        return true;
                    });
                }
                // For layout
                if (GUILayout.Button("", "Label")) { }
            }
        }

        void DrawEditButtons(int index)
        {
            // Edit
            var viewer = xType.View(index);
            var option = GUILayout.Width(20f);
            if (GUILayout.Button("E", "miniButton", option))
            {
                var newXtype = viewer.CreateXtype();
                XTypeEditWindow.ShowEditWindow(newXtype, (editXtype) =>
                {
                    return xType.View(index).Update(editXtype);
                });
            }
            if (GUILayout.Button("\u25B2", "miniButton", option))
            {
                command = new Command(CmdType.MovePrevious, index);
            }
            if (GUILayout.Button("\u25BC", "miniButton", option))
            {
                command = new Command(CmdType.MoveNext, index);
            }
            if (GUILayout.Button("X", "miniButton", option))
            {
                command = new Command(CmdType.Remove, index);
            }
        }

        public bool ExistRepeatedName(string name, int index)
        {
            var beginIndex = xType.FindArrayBegin(index);
            var e = xType.GetEnumerator(beginIndex);
            while (e.MoveNext())
            {
                if (e.Current.name == name) return true;
            }
            return false;
        }

        private XType xType;
        public bool drawWithoutOutermostArray = false;
        public bool repeatedNameCheckInArray = false;
        public bool forbidNamelessXtype = false;

        public bool editMode = false;
        private Command command;

        class Command
        {
            public Command(CmdType type, int index, XType editXtype = null)
            {
                this.type = type;
                this.index = index;
                this.editXtype = editXtype;
            }
            public CmdType type;
            public int index;
            public XType editXtype;
        }

        enum CmdType { Remove, MoveNext, MovePrevious,}

    }




    /// <summary>
    /// Edit window
    /// </summary>
    public class XTypeEditWindow : EditorWindow
    {
        public static XTypeEditWindow ShowEditWindow(XType xType, System.Func<XType, bool> callback = null)
        {
            //Show existing window instance. If one doesn't exist, make one.
            var window = EditorWindow.GetWindow<XTypeEditWindow>();
            window.xType = xType;
            window.callback = callback;
            window.mode = Mode.Edit;
            return window;
        }

        public static XTypeEditWindow ShowCreateWindow(System.Func<XType, bool> callback = null)
        {
            //Show existing window instance. If one doesn't exist, make one.
            var window = EditorWindow.GetWindow<XTypeEditWindow>();
            window.xType = XType.Value(0);
            window.callback = callback;
            window.mode = Mode.Add;
            return window;
        }

        void OnEnable()
        {
            show = true;
        }

        void OnDestroy()
        {
            show = false;
        }

        void OnLostFocus()
        {
            needClose = true;
        }

        void Update()
        {
            if (needClose) Close();
        }

        void OnGUI()
        {
            currentEvent = Event.current;

            // Type select
            
            if (mode == Mode.Edit)
            {
                EditorGUILayout.LabelField("Type", xType.isArray ? "Array" : "Value");
            }
            else
            {
                var isArray = EditorGUILayout.Toggle("Is Array", xType.isArray);
                if (xType.isArray != isArray)
                {
                    xType = isArray ? XType.Array() : XType.Value(0);
                }
            }
            
            // Name 
            xType.name = EditorGUILayout.TextField("Name", xType.name);

            if(!xType.isArray)
            {
                // Current type
                var xValue = xType.View(0);
                var type = Type.GetType(xValue.typeName);
                if (type != null)
                {
                    if (xValue.isUnityObject)
                    {
                        xValue.value = EditorGUILayout.ObjectField(type.FullName, xValue.value as UnityEngine.Object, type, true);
                    }
                    else
                    {
                        xValue.value = QuickEditor.DrawVariable(type.FullName, xValue.value);
                    }
                }

                // Select type
                DrawTypeSelect();
            }
            

            // Sync Button
            string buttonText = mode == Mode.Add ? "Add" : "Sync";
            if (GUILayout.Button(buttonText))
            {
                if (callback(xType))
                {
                    callback = null;
                    Close();
                }
            }
        }

        void DrawTypeSelect()
        {
            System.Action select_finish = () =>
            {
                if (selectType == null)
                {
                    Debug.LogError("Please select type");
                    return;
                }

                var newXtype = XType.Value(xType.name, AssemblyHelper.GetDefaultValue(selectType), selectType);
                if(newXtype == null)
                {
                    Debug.LogError("Can not support " + selectType.Name);
                    return;
                }
                xType = newXtype;
            };

            // Separator line
            EditorGUILayout.Space();

            // Keyboard process
            if (currentEvent.type == EventType.keyDown)
            {
                if (currentEvent.keyCode == KeyCode.UpArrow)
                {
                    SetSelectIndex(selectIndex - 1);
                    currentEvent.Use();
                }
                else if (currentEvent.keyCode == KeyCode.DownArrow)
                {
                    int index = selectIndex + 1;
                    index = index >= preShowCount - 1 ? preShowCount - 1 : index;
                    SetSelectIndex(index);
                    currentEvent.Use();
                }
                else if (currentEvent.keyCode == KeyCode.Return)
                {
                    currentEvent.Use();
                    select_finish();
                    return;
                }
            }

            // Search Text
            using (QuickEditor.BeginHorizontal())
            {
                var text = searchText;
                searchText = EditorGUILayout.TextField("", searchText, "SearchTextField");
                if (text != searchText) SetSelectIndex(0);
                if (GUILayout.Button("", "SearchCancelButton"))
                {
                    searchText = string.Empty;
                    GUIUtility.keyboardControl = 0;
                }
            }
            EditorGUILayout.Space();

            // Component scroll list
            int curCount = 0;
            int maxCount = int.MaxValue;
            using (QuickEditor.BeginScrollView(ref scrollPosition))
            {
                var selectStyle = new GUIStyle("ServerUpdateChangesetOn");
                var normalStyle = new GUIStyle("Label");
                var option = GUILayout.Width(200f);

                foreach (var item in comDict)
                {
                    foreach (var t in item.Value)
                    {
                        if (!string.IsNullOrEmpty(searchText))
                        {
                            if (t.Name.IndexOf(searchText, System.StringComparison.CurrentCultureIgnoreCase) == -1)
                            {
                                continue;
                            }
                        }
                        var style = curCount == selectIndex ? selectStyle : normalStyle;

                        using (QuickEditor.BeginHorizontal(style))
                        {
                            if (GUILayout.Button(t.Name, "Label", option) ||
                                GUILayout.Button(t.Module.Name, "Label"))
                            {
                                selectIndex = curCount;
                                selectType = t;
                                select_finish();
                                return;
                            }
                        }

                        if (selectIndex == curCount)
                        {
                            selectType = t;
                        }

                        ++curCount;
                        if (curCount > maxCount) break;
                    }
                }
                preShowCount = curCount;
            }
        }

        void SetSelectIndex(int index)
        {
            if (index < 0) index = 0;
            selectIndex = index;
            Repaint();
        }


        XType xType = XType.Value(0);
        Mode mode;
        System.Func<XType, bool> callback;


        string searchText = string.Empty;
        int selectIndex = 0;
        Type selectType;
        Event currentEvent;
        Vector2 scrollPosition = Vector2.zero;
        int preShowCount;
        bool needClose = false;

        public static Dictionary<string, List<Type>> comDict
        {
            get
            {
                if (_comDict != null) return _comDict;
                _comDict = new Dictionary<string, List<Type>>();

                System.Func<Type, bool> filter = (t) =>
                {
                    return XType.IsSupportType(t) && QuickEditor.CanTypeDraw(t);
                };

                {// mscorlib
                    string assemblyName = "mscorlib";
                    var list = AssemblyHelper.LoadAssemblyTypes(assemblyName, filter);
                    _comDict.Add(assemblyName, list);
                }

                {// UnityEngine
                    string assemblyName = "UnityEngine";
                    var list = AssemblyHelper.LoadAssemblyTypes(assemblyName, filter);
                    _comDict.Add(assemblyName, list);
                }

                {// UnityEngine.UI
                    string assemblyName = "UnityEngine.UI";
                    var list = AssemblyHelper.LoadAssemblyTypes(assemblyName, filter);
                    _comDict.Add(assemblyName, list);
                }

                // QuickUnity
                {
                    var list = new List<Type>(){
                        typeof(UGUIEventTrigger),
                        typeof(CollisionEventTrigger)
                    };
                    _comDict.Add("QuickUnity", list);
                }
                return _comDict;
            }
        }
        public static Dictionary<string, List<Type>> _comDict;

        enum Mode { Add, Edit }

        public static bool show { get; private set; }
    }
}

