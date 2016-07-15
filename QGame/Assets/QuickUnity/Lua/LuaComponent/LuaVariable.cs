using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace QuickUnity
{
    [System.Serializable]
    public partial class LuaVariable : ICloneable
    {
        protected LuaVariable(Symbol symbol)
        {
            this.symbol = symbol;
        }

        public object Clone()
        {
            var other = new LuaVariable(symbol);
            other.name = name;
            other.uObject = uObject;
            other.symbol = symbol;
            other.str_param = str_param;
            other.str_param1 = str_param1;
            other.str_param2 = str_param2;
            other.int_param = int_param;
            other.int_param1 = int_param1;
            return other;
        }

        public string name { get { return _name; } set { _name = value; } }
        [SerializeField]
        protected string _name;

        [SerializeField]
        protected UnityEngine.Object uObject;

        public Symbol symbol { get { return _symbol; } private set { _symbol = value; } }
        [SerializeField]
        protected Symbol _symbol;

        [SerializeField]
        protected string str_param = string.Empty;

        [SerializeField]
        protected string str_param1 = string.Empty;

        [SerializeField]
        protected string str_param2 = string.Empty;

        [SerializeField]
        protected int int_param;

        [SerializeField]
        protected int int_param1;

        public bool isValue { get { return symbol == Symbol.Value; } }
        public bool isArrayTag { get { return symbol == Symbol.ArrayTag; } }
        public enum Symbol { Unknown = 0, ArrayTag = 1, Value = 2,}

    }

    /// <summary>
    ///  Value Type
    /// </summary>
    public partial class LuaVariable
    {
        public System.Object GetValue()
        {
            if (dirty)
            {
                dirty = false;
                _value = null;
            }
            if (_value != null) return _value;
            switch (valueType)
            {
                case ValueType.Common: { _value = Serializer.Deserialize(commonValue, typeName); break; }
                case ValueType.UnityObject: { _value = uObject; break; }
                default: { Debug.LogErrorFormat("Unknown variable type"); break; }
            }
            return _value;
        }

        public bool Equals(LuaVariable other)
        {
            if (!TypeEquals(other)) return false;
            return GetValue() == other.GetValue();
        }

        public bool TypeEquals(LuaVariable other) { return TypeEquals(other.valueType, other.moduleName, other.typeName); }
        protected bool TypeEquals(System.Object value)
        {
            if (value == null)
            {
                return (valueType == ValueType.UnityObject ||
                typeName == typeof(string).Name);
            }
            Type t = value.GetType();
            ValueType lType; string mName; string tName;
            if (!IsSupportType(t, out lType, out mName, out tName))
            {
                return false;
            }
            return TypeEquals(lType, mName, tName);
        }

        protected bool TypeEquals(ValueType lType, string mName, string tName)
        {
            if (valueType != lType) return false;
            if (moduleName != mName) return false;
            if (typeName != tName) return false;
            return true;
        }

        public static bool IsSupportType(Type t)
        {
            ValueType lType; string mName; string tName;
            return IsSupportType(t, out lType, out mName, out tName);
        }

        protected static bool IsSupportType(Type t, out ValueType type, out string moduleName, out string typeName)
        {
            type = ValueType.Unknown;
            moduleName = t.Module.Name;
            typeName = t.FullName;

            if (t.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                type = ValueType.UnityObject;
            }
            else if (Serializer.CanSerialize(t))
            {
                type = ValueType.Common;
            }
            else
            {
                return false;
            }
            return true;
        }

        protected ValueType valueType { get { return (ValueType)int_param; } set { int_param = (int)value; } }
        public string moduleName { get { return str_param1; } private set { str_param1 = value; } }
        public string typeName { get { return str_param2; } private set { str_param2 = value; } }
        protected string commonValue { get { return str_param; } private set { str_param = value; } }

        public bool isCommonValue { get { return symbol == Symbol.Value && valueType == ValueType.Common; } }
        public bool isUnityObject { get { return symbol == Symbol.Value && valueType == ValueType.UnityObject; } }

        [NonSerialized]
        protected System.Object _value;

        [NonSerialized]
        protected bool dirty = true;

        protected enum ValueType { Unknown, Common, UnityObject, }
    }


    /// <summary>
    /// Value type - Editor
    /// </summary>
#if UNITY_EDITOR
    public partial class LuaVariable
    {
        public static LuaVariable CreateValue(string name, Type t)
        {
            var obj = new LuaVariable(Symbol.Value);
            obj.name = name;
            return obj.Reset(t) ? obj : null;
        }

        public bool SetValue(System.Object value)
        {
            if (value == null)
            {
                if (valueType == ValueType.UnityObject)
                {
                    _SetValue(value);
                    return true;
                }

                var t = AssemblyHelper.GetDefaultValue(moduleName, typeName);
                if (t == null)
                {
                    _SetValue(value);
                    return true;
                }
                Debug.LogError("Not enough type info to reset variable");
                return false;
            }

            ValueType lType; string mName; string tName;
            if (!IsSupportType(value.GetType(), out lType, out mName, out tName))
            {
                Debug.LogError("Unsupported type " + value.GetType().FullName);
                return false;
            }
            if (!TypeEquals(lType, mName, tName))
            {
                Reset(name, lType, mName, tName);
            }
            _SetValue(value);
            return true;
        }

        public bool Reset(Type type)
        {
            ValueType lType; string mName; string tName;
            if (!IsSupportType(type, out lType, out mName, out tName))
            {
                Debug.LogError("Unsupported type " + type.FullName);
                return false;
            }

            Reset(name, lType, mName, tName);
            _SetValue(AssemblyHelper.GetDefaultValue(type));
            return true;
        }
        protected void Reset(
            string name = null,
            ValueType type = ValueType.Unknown,
            string moduleName = null,
            string tName = null)
        {
            this.name = string.IsNullOrEmpty(name) ? string.Empty : name;
            this.valueType = type;
            this.moduleName = string.IsNullOrEmpty(moduleName) ? string.Empty : moduleName;
            this.typeName = string.IsNullOrEmpty(tName) ? string.Empty : tName;

            commonValue = string.Empty;
            uObject = null;

            dirty = true;
        }

        protected void _SetValue(System.Object value)
        {
            switch (valueType)
            {
                case ValueType.Common: { commonValue = Serializer.Serialize(value); break; }
                case ValueType.UnityObject: { uObject = value as UnityEngine.Object; break; }
                default: { Debug.LogErrorFormat("Unknown variable type"); break; }
            }
            dirty = true;
        }
    }
#endif


    /// <summary>
    /// Array
    /// </summary>
    public partial class LuaVariable
    {
        public static void CreateArray(string name, out LuaVariable begin, out LuaVariable end)
        {
            begin = new LuaVariable(Symbol.ArrayTag);
            begin.arrayType = ArrayType.Begin;
            begin.name = name;

            end = new LuaVariable(Symbol.ArrayTag);
            end.arrayType = ArrayType.End;
            end.name = name;
        }
        protected ArrayType arrayType { get { return (ArrayType)int_param; } set { int_param = (int)value; } }

        public bool isArrayBegin { get { return symbol == Symbol.ArrayTag && arrayType == ArrayType.Begin; } }
        public bool isArrayEnd { get { return symbol == Symbol.ArrayTag && arrayType == ArrayType.End; } }


        public enum ArrayType { Unknown, Begin, End}
    }


    /// <summary>
    /// Array -- Editor
    /// </summary>

#if UNITY_EDITOR
    public partial class LuaVariable
    {
        [NonSerialized]
        public bool foldout = true;
    }
#endif
}


