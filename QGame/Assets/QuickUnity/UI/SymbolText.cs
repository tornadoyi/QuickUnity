using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace QuickUnity
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Text))]
    public class SymbolText : SymbolWidget
    {
        public Text text;
        public string symbolText;

        protected Coroutine co_load;

        protected virtual void Awake()
        {
            if (text == null)
            {
                text = GetComponent<Text>();
                if (text == null) return;
            }
            SetSymbolText(symbolText);
        }

        
        public void SetSymbolText(string symbolText) { SetSymbolText(libraryName, symbolText); }

        public void SetSymbolText(string libName, string symbolText)
        {
            this.libraryName = libraryName;
            this.symbolText = symbolText;
            string text = SymbolManager.Translate(libraryName, symbolText);
            this.text.text = text == null ? string.Empty : text;
        }


        protected override string GetLibraryName() { return QConfig.Symbol.textLibrary; }

        protected override void OnUpdateSymbol()
        {
            SetSymbolText(libraryName, symbolText);
        }

        
    }
}