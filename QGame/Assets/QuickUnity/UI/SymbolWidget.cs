using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    [DisallowMultipleComponent]
    public class SymbolWidget : MonoBehaviour
    {
        protected virtual string GetLibraryName() { return string.Empty; }

        public virtual IEnumerator WaitForDone() { yield break; }

        public string libraryName
        {
            get
            {
                var name = GetLibraryName();
                return string.IsNullOrEmpty(name) ? _libraryName : name;
            }
            set { _libraryName = value; }
        }
        private string _libraryName = string.Empty;

    }
}


