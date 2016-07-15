using System.Collections;
using SimpleJson;

public static class SimpleJson_Ext
{
    public static bool HasKey(this JsonObject jo, string key)
    {
        if (string.IsNullOrEmpty(key)) return false;
        return jo.ContainsKey(key);
    }

    public static string GetString(this JsonObject jo, string key, string defaultValue = "")
    {
        if (string.IsNullOrEmpty(key)) return defaultValue;
        object value = null;
        if(!jo.TryGetValue(key, out value)) return defaultValue;
        return value is string ? value as string : defaultValue;
    }

    public static int GetInt(this JsonObject jo, string key, int defaultValue = default(int))
    {
        if (string.IsNullOrEmpty(key)) return defaultValue;
        object value = null;
        if (!jo.TryGetValue(key, out value)) return defaultValue;
        return IsNumeric(value) ? System.Convert.ToInt32(value) : defaultValue;
    }

    public static long GetLong(this JsonObject jo, string key, long defaultValue = default(long))
    {
        if (string.IsNullOrEmpty(key)) return defaultValue;
        object value = null;
        if (!jo.TryGetValue(key, out value)) return defaultValue;
        return IsNumeric(value) ? System.Convert.ToInt64(value) : defaultValue;
    }

    public static float GetFloat(this JsonObject jo, string key, float defaultValue = default(float))
    {
        if (string.IsNullOrEmpty(key)) return defaultValue;
        object value = null;
        if (!jo.TryGetValue(key, out value)) return defaultValue;
        return IsNumeric(value) ? System.Convert.ToSingle(value) : defaultValue;
    }

    public static double GetDouble(this JsonObject jo, string key, double defaultValue = default(double))
    {
        if (string.IsNullOrEmpty(key)) return defaultValue;
        object value = null;
        if (!jo.TryGetValue(key, out value)) return defaultValue;
        return IsNumeric(value) ? System.Convert.ToDouble(value) : defaultValue;
    }

    public static bool GetBoolean(this JsonObject jo, string key, bool defaultValue = default(bool))
    {
        if (string.IsNullOrEmpty(key)) return defaultValue;
        object value = null;
        if (!jo.TryGetValue(key, out value)) return defaultValue;
        return value is bool ? (bool)value : defaultValue;
    }

    public static JsonObject GetJson(this JsonObject jo, string key)
    {
        if (string.IsNullOrEmpty(key)) return new JsonObject();
        object value = null;
        if (!jo.TryGetValue(key, out value)) return new JsonObject();
        return value is JsonObject ? value as JsonObject : new JsonObject();
    }

    public static JsonArray GetArray(this JsonObject jo, string key)
    {
        if (string.IsNullOrEmpty(key)) return new JsonArray();
        object value = null;
        if (!jo.TryGetValue(key, out value)) return new JsonArray();
        return value is JsonArray ? value as JsonArray : new JsonArray();
    }



    private static bool IsNumeric(object value)
    {
        if (value is sbyte) return true;
        if (value is byte) return true;
        if (value is short) return true;
        if (value is ushort) return true;
        if (value is int) return true;
        if (value is uint) return true;
        if (value is long) return true;
        if (value is ulong) return true;
        if (value is float) return true;
        if (value is double) return true;
        if (value is decimal) return true;
        return false;
    }
}
