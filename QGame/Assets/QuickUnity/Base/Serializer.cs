using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

namespace QuickUnity
{
    public class Serializer
    {
        public static bool CanSerialize(System.Object o) { return helper.ContainsKey(o.GetType().AssemblyQualifiedName); }
        public static bool CanSerialize(Type t) { return helper.ContainsKey(t.AssemblyQualifiedName); }
        public static string Serialize(System.Object o)
        {
            if (o == null) return string.Empty;
            var name = o.GetType().AssemblyQualifiedName;
            Function func = null;
            if(!helper.TryGetValue(name, out func))
            {
                Debug.LogError("Can not Serialize " + name);
                return string.Empty;
            }
            string ret = string.Empty;
            try
            {
                ret = func.set(o);
            }
            catch(System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
            return ret;
        }

        public static System.Object Deserialize(string o, string key)
        {
            Function func = null;
            if (!helper.TryGetValue(key, out func))
            {
                Debug.LogError("Can not Deserialize " + key);
                return string.Empty;
            }
            System.Object ret = null;
            try
            {
                ret = func.get(o);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
            return ret;
        }

        protected static Dictionary<string, Function> helper = new Dictionary<string, Function>()
        {
#region mscorlib
            {typeof(sbyte).AssemblyQualifiedName, new Function(common_set, sbyte_get)},
            {typeof(byte).AssemblyQualifiedName, new Function(common_set, byte_get)},
            {typeof(short).AssemblyQualifiedName, new Function(common_set, short_get)},
            {typeof(ushort).AssemblyQualifiedName, new Function(common_set, ushort_get)},
            {typeof(int).AssemblyQualifiedName, new Function(common_set, int_get)},
            {typeof(uint).AssemblyQualifiedName, new Function(common_set, uint_get)},
            {typeof(long).AssemblyQualifiedName, new Function(common_set, long_get)},
            {typeof(ulong).AssemblyQualifiedName, new Function(common_set, ulong_get)},
            {typeof(float).AssemblyQualifiedName, new Function(common_set, float_get)},
            {typeof(double).AssemblyQualifiedName, new Function(common_set, double_get)},
            {typeof(decimal).AssemblyQualifiedName, new Function(common_set, decimal_get)},
            {typeof(bool).AssemblyQualifiedName, new Function(common_set, bool_get)},
            {typeof(string).AssemblyQualifiedName, new Function(common_set, string_get)},
#endregion

#region Unity Struct
            {typeof(Vector2).AssemblyQualifiedName, new Function(Vector2_set, Vector2_get)},
            {typeof(Vector3).AssemblyQualifiedName, new Function(Vector3_set, Vector3_get)},
            {typeof(Vector4).AssemblyQualifiedName, new Function(Vector4_set, Vector4_get)},
            {typeof(Rect).AssemblyQualifiedName, new Function(Rect_set, Rect_get)},
            {typeof(Quaternion).AssemblyQualifiedName, new Function(Quaternion_set, Quaternion_get)},
            {typeof(Color).AssemblyQualifiedName, new Function(Color_set, Color_get)},
            {typeof(Bounds).AssemblyQualifiedName, new Function(Bounds_set, Bounds_get)},
#endregion
        };


        protected static string common_set(System.Object o) { return o.ToString(); }
        protected static System.Object sbyte_get(string o) { return Convert.ToSByte(o); }
        protected static System.Object byte_get(string o) { return Convert.ToByte(o); }
        protected static System.Object short_get(string o) { return Convert.ToInt16(o); }
        protected static System.Object ushort_get(string o) { return Convert.ToUInt16(o); }
        protected static System.Object int_get(string o) { return Convert.ToInt32(o); }
        protected static System.Object uint_get(string o) { return Convert.ToUInt32(o); }
        protected static System.Object long_get(string o) { return Convert.ToInt64(o); }
        protected static System.Object ulong_get(string o) { return Convert.ToUInt64(o); }
        protected static System.Object float_get(string o) { return Convert.ToSingle(o); }
        protected static System.Object double_get(string o) { return Convert.ToDouble(o); }
        protected static System.Object decimal_get(string o) { return Convert.ToDecimal(o); }
        protected static System.Object bool_get(string o) { return Convert.ToBoolean(o); }
        protected static System.Object string_get(string o) { return o; }


        protected static string Vector2_set(System.Object o)
        {
            var d = (Vector2)o;
            return Encode(d.x, d.y);
        }

        protected static System.Object Vector2_get(string o)
        {
            var d = Decode(o);
            return new Vector2(
                Convert.ToSingle(d[0]),
                Convert.ToSingle(d[1])
                );
        }

        protected static string Vector3_set(System.Object o)
        {
            var d = (Vector3)o;
            return Encode(d.x, d.y, d.z);
        }

        protected static System.Object Vector3_get(string o)
        {
            var d = Decode(o);
            return new Vector3(
                Convert.ToSingle(d[0]),
                Convert.ToSingle(d[1]),
                Convert.ToSingle(d[2])
                );
        }

        protected static string Vector4_set(System.Object o)
        {
            var d = (Vector4)o;
            return Encode(d.x, d.y, d.z, d.w);
        }

        protected static System.Object Vector4_get(string o)
        {
            var d = Decode(o);
            return new Vector4(
                Convert.ToSingle(d[0]),
                Convert.ToSingle(d[1]),
                Convert.ToSingle(d[2]),
                Convert.ToSingle(d[3])
                );
        }

        protected static string Rect_set(System.Object o)
        {
            var d = (Rect)o;
            return Encode(
                d.position.x,
                d.position.y,
                d.size.x,
                d.size.y);
        }

        protected static System.Object Rect_get(string o)
        {
            var d = Decode(o);
            return new Rect(
                new Vector2(
                    Convert.ToSingle(d[0]),
                    Convert.ToSingle(d[1])),
                new Vector2(
                    Convert.ToSingle(d[2]),
                    Convert.ToSingle(d[3]))
                );
        }

        protected static string Quaternion_set(System.Object o)
        {
            var d = (Quaternion)o;
            return Encode(d.x, d.y, d.z, d.w);
        }

        protected static System.Object Quaternion_get(string o)
        {
            var d = Decode(o);
            return new Quaternion(
                Convert.ToSingle(d[0]),
                Convert.ToSingle(d[1]),
                Convert.ToSingle(d[2]),
                Convert.ToSingle(d[3])
                );
        }

        protected static string Color_set(System.Object o)
        {
            var d = (Color)o;
            return Encode(d.r, d.g, d.b, d.a);
        }

        protected static System.Object Color_get(string o)
        {
            var d = Decode(o);
            return new Color(
                Convert.ToSingle(d[0]),
                Convert.ToSingle(d[1]),
                Convert.ToSingle(d[2]),
                Convert.ToSingle(d[3])
                );
        }

        protected static string Bounds_set(System.Object o)
        {
            var d = (Bounds)o;
            return Encode(
                d.center.x,
                d.center.y,
                d.center.z,
                d.size.x,
                d.size.y,
                d.size.z);
        }

        protected static System.Object Bounds_get(string o)
        {
            var d = Decode(o);
            return new Bounds(
                new Vector3(
                    Convert.ToSingle(d[0]),
                    Convert.ToSingle(d[1]),
                    Convert.ToSingle(d[2])),
                new Vector3(
                    Convert.ToSingle(d[3]),
                    Convert.ToSingle(d[4]),
                    Convert.ToSingle(d[5]))
                );
        }

        protected static string Encode(params System.Object[] elements)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < elements.Length; ++i)
            {
                builder.Append(elements[i]);
                builder.Append(separator);
            }
            return builder.ToString();
        }

        protected static string[] Decode(string o)
        {
            return o.Split(separator);
        }


        protected class Function
        {
            public Function(SerializeDelegate set, DeserializeDelegate get)
            {
                this.set = set;
                this.get = get;
            }
            public SerializeDelegate set;
            public DeserializeDelegate get;
        }

        protected delegate string SerializeDelegate(System.Object o);
        protected delegate System.Object DeserializeDelegate(string o);

        protected const char separator = '\t';
    }

}

