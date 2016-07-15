using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace QuickUnity
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class SymbolText : SymbolWidget
    {
        protected virtual void Awake()
        {
            if (text == null)
            {
                text = GetComponent<Text>();
                if (text == null) return;
            }
            SetSymbolText(symbolText);
        }

        protected override string GetLibraryName() { return QConfig.Symbol.textLibrary; }

        public void SetSymbolText(string symbolText) { SetSymbolText(libraryName, symbolText); }

        public void SetSymbolText(string libName, string symbolText)
        {
            string text = SymbolManager.Translate(libName, symbolText);
            SetText(text);
        }


        public void SetText(string text)
        {
            this.text.text = text == null ? string.Empty : text;
        }

        public Text text;
        public string symbolText;

        protected Coroutine co_load;
    }
}