using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;


namespace QuickUnity
{
    public class Yaml
    {
        public static Regex YAML_IGNORE = new System.Text.RegularExpressions.Regex("(#.*$| |\r)");
        public Dictionary<string, string> values { get; private set; }

        public Yaml(string content)
        {
            values = new Dictionary<string, string>();
            content.TrimEnd('\0');
            var lines = content.Split("\n"[0]);
            foreach (var line in lines)
            {
                var data = YAML_IGNORE.Replace(line, "");
                var item = data.Split(":"[0]);
                if (item[0] != string.Empty && item.Length > 1)
                {
                    values[item[0]] = item[1];
                    if (item.Length > 2)
                    {
                        var itemLen = item.Length;
                        for (var i = 2; i < itemLen; i++)
                        {
                            this.values[item[0]] += ":" + item[i];
                        }
                    }
                }
            }
        }

        public string Get(string key, string defaultValue = "")
        {
            if (!this.values.ContainsKey(key))
            {
                return defaultValue;
            }
            return this.values[key];
        }

        public string GetString(string key, string defaultValue = "")
        {
            if (!this.values.ContainsKey(key))
            {
                return defaultValue;
            }
            return this.values[key];
        }

        public int GetInt(string key, int defaultValue = default(int))
        {
            var stringValue = Get(key);
            if (string.IsNullOrEmpty(stringValue))
            {
                return defaultValue;
            }
            return int.Parse(stringValue);
        }

        public float GetFloat(string key, float defaultValue = default(float))
        {
            var stringValue = Get(key);
            if (string.IsNullOrEmpty(stringValue))
            {
                return defaultValue;
            }
            return float.Parse(stringValue);
        }

        public bool GetBool(string key, bool defaultValue = default(bool))
        {
            var stringValue = Get(key);
            if (string.IsNullOrEmpty(stringValue))
            {
                return defaultValue;
            }
            return bool.Parse(stringValue);
        }
    }
}

