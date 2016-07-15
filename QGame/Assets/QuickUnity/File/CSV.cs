using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System;

namespace QuickUnity
{
    public class CSV
    {
        public void AddLine(string[] elements)
        {
            if (elements == null || elements.Length == 0)
            {
                Debug.LogError("Invalid elements");
                return;
            }

            lines.Add(elements);
        }

        public void AddLine(List<string> elements)
        {
            if (elements == null || elements.Count == 0)
            {
                Debug.LogError("Invalid elements");
                return;
            }

            string[] data = new string[elements.Count];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = elements[i];
            }

            lines.Add(data);
        }

        public void AddLine(string element)
        {
            string[] elements = {element};
            AddLine(elements);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < lines.Count; ++i)
            {
                string[] elements = lines[i];
                for (int j = 0; j < elements.Length; ++j)
                {
                    builder.Append(elements[j]);
                    if (j != elements.Length - 1) builder.Append(split);
                }
                if (i != lines.Count - 1) builder.Append("\r\n");
            }
            return builder.ToString();
        }

        public void FromString(string data)
        {
            lines.Clear();
            string[] s = { "\r\n" };
            string[] lineData = data.Split(s, StringSplitOptions.None);
            for (int i = 0; i < lineData.Length; ++i)
            {
                string line = lineData[i];
                string[] elements = line.Split(split);
                AddLine(elements);
            }
        }

        public char split = ',';
        protected List<string[]> lines = new List<string[]>();
        
    }

}


