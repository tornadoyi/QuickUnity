using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SLua;

namespace QuickUnity
{
    [System.Serializable]
    public partial class XType : ICloneable
    {
        protected XType() { }

        protected XType(XValue xValue)
        {
            if (xValue.isUnkown)
            {
                type = Type.Unknown;
            }
            else if (xValue.isArray)
            {
                type = Type.Array;
                xValueList.Add(XValue.ArrayBegin());
                xValueList.Add(XValue.ArrayEnd());
                name = xValue.name;
            }
            else
            {
                type = Type.Value;
                this.xValue = xValue.Clone() as XValue;
            }
        }

        public static XType Value(System.Object o) { return Value(string.Empty, o); }

        public static XType Value(string name, System.Object o)
        {
            if(o == null) { Debug.LogError("Null is ambiguous type for string or unity object type"); return null; }
            return Value(name, o, o.GetType());
        }

        public static XType Value(System.Object o, System.Type t) { return Value(string.Empty, o, t); }

        public static XType Value(string name, System.Object o, System.Type t)
        {
            var xValue = XValue.Value(t);
            if (xValue == null) return null;
            xValue.SetValue(o);
            var xType = new XType();
            xType.type = Type.Value;
            xType.xValue = xValue;
            xType.name = name;
            return xType;
        }

        public static XType Array() { return Array(string.Empty); }

        public static XType Array(string name)
        {
            var xType = new XType();
            xType.type = Type.Array;
            xType.xValueList.Add(XValue.ArrayBegin());
            xType.xValueList.Add(XValue.ArrayEnd());
            xType.name = name;
            return xType;
        }

        public void SetValue(System.Object o, System.Type t)
        {
            if (isArray) { Debug.LogError("Can not set value for array"); return; }
            xValue.SetValue(o, t);
        }

        public static bool IsSupportType(System.Type t) { return XValue.IsSupport(t); }

        public object Clone()
        {
            var other = new XType();
            other.type = type;
            other.xValue = xValue.Clone() as XValue;
            for(int i=0; i<xValueList.Count; ++i)
            {
                other.xValueList.Add(xValueList[i].Clone() as XValue);
            }
            return other;
        }

        // Name
        public string name
        {
            get
            {
                if (isUnknown) return string.Empty;
                if (isValue) return xValue.name;
                return xValueList[0].name;
            }
            set
            {
                if (isUnknown) return;
                if (isValue) xValue.name = value;
                else { xValueList[0].name = value; }
            }
        }

        // Value
        public System.Object value
        {
            get
            {
                if (!isValue)
                {
                    Debug.LogErrorFormat("{0} can not set value", type.ToString());
                    return null;
                }
                return xValue.GetValue(); ;
            }
            set
            {
                if (!isValue)
                {
                    Debug.LogErrorFormat("{0} can not set value", type.ToString());
                    return;
                }
                xValue.SetValue(value);
            }
        }

        // Base properties
        public bool isUnknown { get { return type == Type.Unknown; } }
        public bool isValue { get { return type == Type.Value; } }
        public bool isArray { get { return type == Type.Array; } }


        // Save to meta
        [SerializeField]
        protected XValue xValue;

        [SerializeField]
        protected List<XValue> xValueList = new List<XValue>();

        [SerializeField]
        protected Type type = Type.Unknown;

        public enum Type
        {
            Unknown = 0,
            Value,
            Array,
        }
    }


    /// <summary>
    /// Array 
    /// </summary>
    public partial class XType
    {
        public int Add(XType v)
        {
            if (v == null) { Debug.LogError("Invalid XType"); return -1; }
            if (!isArray)
            {
                Debug.LogErrorFormat("{0} can not Add", type.ToString());
                return -1;
            }

            int index = xValueList.Count - 1;
            Insert(index, v);

            return index;
        }

        public void Insert(int index, XType v)
        {
            if (v == null) { Debug.LogError("Invalid XType"); return; }
            if (!IndexCheck(index)) return;

            // Value or Unknown
            if (!v.isArray)
            {
                xValueList.Insert(index, v.xValue);
            }
            else // Array
            {
                xValueList.InsertRange(index, v.xValueList);
            }
        }

        public void RemoveAt(int index)
        {
            if (!IndexCheck(index)) return;

            var xValue = xValueList[index];
            if (!xValue.isArray)
            {
                xValueList.RemoveAt(index);
            }
            else
            {
                int st = FindArrayBegin(index);
                int ed = FindArrayEnd(index);
                if (st < 0 || ed < 0) { Debug.LogError("Array has broken"); return; }
                xValueList.RemoveRange(st, ed - st + 1);
            }
        }

        public int FindArrayBegin(int from)
        {
            if (!IndexCheck(from)) return -1;

            int restEnd = xValueList[from].isArrayBegin ? 1 : 0;
            for (int i = from; i >= 0; --i)
            {
                var xValue = xValueList[i];
                if (xValue.isArrayBegin)
                {
                    --restEnd;
                    if (restEnd <= 0) return i;
                }
                else if (xValue.isArrayEnd)
                {
                    ++restEnd;
                }
            }
            return -1;
        }

        public int FindArrayEnd(int from)
        {
            if (!IndexCheck(from)) return -1;

            int restBegin = xValueList[from].isArrayEnd ? 1 : 0;
            for (int i = from; i < xValueList.Count; ++i)
            {
                var xValue = xValueList[i];
                if (xValue.isArrayEnd)
                {
                    --restBegin;
                    if (restBegin <= 0) return i;
                }
                else if (xValue.isArrayBegin)
                {
                    ++restBegin;
                }
            }
            return -1;
        }

        public int MoveNext(int index)
        {
            // Check can move
            if (index - 1 < 0 || index >= xValueList.Count) { return index; }
            if (!xValueList[index].isArrayBegin && xValueList[index + 1].isArrayEnd) return index;

            int st1 = index; int ed1 = st1;
            if (xValueList[index].isArray)
            {
                st1 = FindArrayBegin(index);
                ed1 = FindArrayEnd(index);
            }

            if (ed1 + 1 >= xValueList.Count) return index;

            int st2 = ed1 + 1; int ed2 = st2;
            if (xValueList[st2].isArray)
            {
                st2 = FindArrayBegin(ed1 + 1);
                ed2 = FindArrayEnd(ed1 + 1);
            }
            Swap(st1, ed1, st2, ed2);

            return ed2 - st2 + index + 1;
        }

        public int MovePrevious(int index)
        {
            if (index - 1 < 0 || index >= xValueList.Count) { return index; }
            if (!xValueList[index].isArrayEnd && xValueList[index - 1].isArrayBegin) return index;

            int st2 = index; int ed2 = st2;
            if (xValueList[index].isArray)
            {
                st2 = FindArrayBegin(index);
                ed2 = FindArrayEnd(index);
            }

            if (st2 - 1 < 0) return index;

            int st1 = st2 - 1; int ed1 = st1;
            if (xValueList[st1].isArray)
            {
                st1 = FindArrayBegin(index - 1);
                ed1 = FindArrayEnd(index - 1);
            }

            Swap(st1, ed1, st2, ed2);

            return index - (ed1 - st1) - 1;
        }

        void Swap(int st1, int ed1, int st2, int ed2)
        {
            System.Action<int, int> swap = (st, ed) =>
            {
                while (st < ed)
                {
                    var v = xValueList[st];
                    xValueList[st] = xValueList[ed];
                    xValueList[ed] = v;
                    ++st;
                    --ed;
                }
            };

            // left swap
            swap(st1, ed1);

            // right swap
            swap(st2, ed2);

            // all swap
            swap(st1, ed2);
        }

        private bool IndexCheck(int index)
        {
            if (!isArray)
            {
                Debug.LogErrorFormat("{0} can not Add", type.ToString());
                return false;
            }

            if (index < 0 || index >= xValueList.Count)
            {
                Debug.LogError("Invalid index " + index);
                return false;
            }
            return true;
        }

        public int beginIndex
        {
            get
            {
                if (!isArray) { Debug.LogErrorFormat("{0} can not Add", type.ToString()); return -1; }
                return 1;
            }
        }

        public int endIndex
        {
            get
            {
                if (!isArray) { Debug.LogErrorFormat("{0} can not Add", type.ToString()); return -1; }
                return xValueList.Count - 1;
            }
        }
    }


    /// <summary>
    /// Enumerator
    /// </summary>
    public partial class XType : IEnumerable
    {
        public ValueViewer View(int index) { return new ValueViewer(this, index); }
        public Enumerator GetEnumerator(int index) { return new Enumerator(this, index); }
        public Enumerator GetEnumerator() { return new Enumerator(this, 0); }
        IEnumerator IEnumerable.GetEnumerator() { return new Enumerator(this, 0); }
        
        public struct Enumerator : IEnumerator, IDisposable
        {
            public Enumerator(XType xType, int index)
            {
                this.xType = xType;
                this.startIndex = index;
                this.currentIndex = this.startIndex;
            }

            public ValueViewer Current { get { return new ValueViewer(xType, currentIndex); } }

            object IEnumerator.Current { get { return new ValueViewer(xType, currentIndex); } }

            public void Dispose() { }
            public bool MoveNext()
            {
                // Skip sub array
                if (currentIndex != startIndex &&
                    list[currentIndex].isArrayBegin)
                {
                    currentIndex = xType.FindArrayEnd(currentIndex);
                }

                if (currentIndex + 1 >= list.Count) return false;
                ++currentIndex;

                var xValue = list[currentIndex];
                if (xValue.isArrayEnd) return false;
                return true;
            }

            public void Reset()
            {
                currentIndex = startIndex;
            }

            private List<XValue> list { get { return xType.xValueList; } }

            private XType xType;
            private int startIndex;
            private int currentIndex;
        }


        public struct ValueViewer
        {
            public ValueViewer(XType xType, int currentIndex)
            {
                this.xType = xType;
                if(xType.isArray)
                {
                    this.index = currentIndex;
                    this.xValue = xType.xValueList[currentIndex];
                }
                else
                {
                    this.xValue = xType.xValue;
                    this.index = -1;
                }
            }

            public XType CreateXtype() { return new XType(xValue); }

            public bool Update(XType newXtype)
            {
                if (!xValue.SetValue(newXtype.xValue)) return false;
                xValue.name = newXtype.name;
                return true;
            }

            public IEnumerator GetEnumerator() { return new Enumerator(xType, index); }

            public bool isUnkown { get { return xValue.isUnkown; } }
            public bool isArray { get { return xValue.isArray; } }
            public bool isArrayBegin { get { return xValue.isArrayBegin; } }
            public bool isArrayEnd { get { return xValue.isArrayEnd; } }
            public bool isValue { get { return xValue.isValue; } }
            public bool isString { get { return xValue.isString; } }
            public bool isUnityObject { get { return xValue.isUnityObject; } }

            public string name { get { return xValue.name; } set { xValue.name = value; } }
            public System.Object value { get { return xValue.GetValue(); } set { xValue.SetValue(value); } }
            public int index { get; private set; }

            private XValue xValue;
            private XType xType;

#if UNITY_EDITOR
            [DoNotToLua]
            public string typeName { get { return xValue.GetTypeName(); } }

            [DoNotToLua]
            public bool foldout { get { return xValue.foldout; } set { xValue.foldout = value; } }
#endif
        }
    }


    /// <summary>
    /// XValue
    /// </summary>
    public partial class XType
    {
        [System.Serializable]
        public partial class XValue : ICloneable
        {
            // Important !!!! for serialize
            protected XValue() { }

            protected XValue(ValueType t)
            {
                valueType = t;
                dirty = true;
            }

            public static XValue ArrayBegin() { return new XValue(ValueType.ArrayBegin); }
            public static XValue ArrayEnd() { return new XValue(ValueType.ArrayEnd); }
            public static XValue Value(System.Type t)
            {
                if (!IsSupport(t)) return null;
                var xValue = new XValue(ValueType.Unknown);
                if (!xValue.RestType(t)) return null;
                return xValue;
            }

            private bool RestType(System.Type t)
            {
                if (isArray) { return false; }
                if (!IsSupport(t)) return false;
                if (t == typeof(string)) valueType = ValueType.String;
                else if (t.IsSubclassOf(typeof(UnityEngine.Object))) valueType = ValueType.UnityObject;
                else { valueType = ValueType.Value; }

                typeName = t.AssemblyQualifiedName;
                objectValue = null;
                stringValue = string.Empty;
                _value = null;

                return true;
            }

            public static bool IsSupport(System.Type t)
            {
                return (t.IsSubclassOf(typeof(UnityEngine.Object)) ||
                    Serializer.CanSerialize(t));
            }

            public bool SetValue(XValue other)
            {
                if(other == null) { Debug.LogError("Invalid XValue"); return false; }
                if(isArray || other.isArray)
                {
                    Debug.LogError("Can not set value for array");
                    return false;
                }
                valueType = other.valueType;
                typeName = other.typeName;
                objectValue = null;
                stringValue = string.Empty;
                _value = null;

                return SetValue(other.GetValue());
            }

            public bool SetValue(System.Object o, System.Type type)
            {
                if (type != null && !RestType(type)) return false;
                return SetValue(o);
            }

            public bool SetValue(System.Object o)
            {
                if (valueType < ValueType.Value)
                {
                    Debug.LogError("Can not set value for non-value type");
                    return false;
                }

                if (o == null)
                {
                    if (valueType == ValueType.String) stringValue = string.Empty;
                    else if (valueType == ValueType.UnityObject) objectValue = null;
                    else { Debug.LogError("Can not set null to value type"); return false; }
                }
                else if (o is UnityEngine.Object)
                {
                    if (valueType == ValueType.UnityObject)
                    {
                        objectValue = o as UnityEngine.Object;
                        if(objectValue != null) typeName = o.GetType().AssemblyQualifiedName;
                    }
                    else { Debug.LogErrorFormat("UnityObject can not set to {0}", valueType.ToString()); return false; }
                }
                else if (Serializer.CanSerialize(o))
                {
                    stringValue = Serializer.Serialize(o);
                    typeName = o.GetType().AssemblyQualifiedName;
                }
                else
                {
                    Debug.LogErrorFormat("{0} can not set to {1}", o.GetType().Name, valueType.ToString());
                    return false;
                }

                dirty = true;
                return true;
            }


            public System.Object GetValue()
            {
                if (valueType < ValueType.Value)
                {
                    Debug.LogError("non-value type has no value");
                    return null;
                }

                if (valueType == ValueType.UnityObject)
                {
                    _value = objectValue;
                }
                else
                {
                    _value = Serializer.Deserialize(stringValue, typeName);
                }

                dirty = false;
                return _value;
            }



            public object Clone()
            {
                var other = new XValue(valueType);
                other._name = _name;
                other.objectValue = objectValue;
                other.stringValue = stringValue;
                other.valueType = valueType;
                other.typeName = typeName;
                return other;
            }

            // Runtime properties
            [NonSerialized]
            protected System.Object _value;

            [NonSerialized]
            protected bool dirty = true;


            // Base properties
            public string name { get { return _name; } set { _name = value; } }

            public bool isUnkown { get { return valueType == ValueType.Unknown; } }
            public bool isArray { get { return (isArrayBegin || isArrayEnd); } }
            public bool isArrayBegin { get { return valueType == ValueType.ArrayBegin; } }
            public bool isArrayEnd { get { return valueType == ValueType.ArrayEnd; } }
            public bool isValue { get { return valueType == ValueType.Value; } }
            public bool isString { get { return valueType == ValueType.String; } }
            public bool isUnityObject { get { return valueType == ValueType.UnityObject; } }


            // Need to serialize
            [SerializeField]
            protected string _name;

            [SerializeField]
            protected UnityEngine.Object objectValue;

            [SerializeField]
            protected string stringValue = string.Empty;

            [SerializeField]
            protected string typeName = string.Empty;

            [SerializeField]
            protected ValueType valueType = ValueType.Unknown;

            protected enum ValueType
            {
                Unknown = 0,
                ArrayBegin = 9000,
                ArrayEnd = 9999,
                Value = 1,
                String = 2,
                UnityObject = 3,
            }

            public string GetTypeName() { return typeName; }


#if UNITY_EDITOR
            [NonSerialized]
            public bool foldout = true;
#endif
        }
    }


}


