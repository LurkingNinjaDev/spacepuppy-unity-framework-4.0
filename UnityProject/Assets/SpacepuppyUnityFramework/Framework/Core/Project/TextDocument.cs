﻿#pragma warning disable 0649 // variable declared but not used.
#pragma warning disable 0169 // variable declared but not used.

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace com.spacepuppy.Project
{

    [CreateAssetMenu(fileName = "TextDocument", menuName = "Spacepuppy/TextDocument")]
    public class TextDocument : ScriptableObject, ITextSource
    {

        #region Fields

        [SerializeField()]
        [ReorderableArray()]
        [TextArea(3, 10)]
        private string[] _text;

        #endregion

        #region ITextSource Interface

        public string this[int index]
        {
            get
            {
                return _text[index];
            }
        }

        public int Count
        {
            get
            {
                return _text.Length;
            }
        }

        public string text
        {
            get
            {
                return (_text.Length > 0) ? _text[0] : null;
            }
        }

        #endregion

        #region IEnumerable Interface

        public IEnumerator<string> GetEnumerator()
        {
            return (_text as IEnumerable<string>).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

    }

    [CreateAssetMenu(fileName = "WeightedTextDocument", menuName = "Spacepuppy/WeightedTextDocument")]
    public class WeightedTextDocument : ScriptableObject, ITextSource
    {

        #region Fields

        [SerializeField()]
        [ReorderableArray()]
        [TextArea(3, 10)]
        private string[] _text;
        [SerializeField]
        private float[] _weights;

        #endregion

        #region Properties
        
        #endregion

        #region ITextSource Interface

        public string this[int index]
        {
            get
            {
                return _text[index];
            }
        }

        public int Count
        {
            get
            {
                return _text.Length;
            }
        }

        public string text
        {
            get
            {
                return (_text.Length > 0) ? _text[0] : null;
            }
        }

        #endregion

        #region IEnumerable Interface

        public IEnumerator<string> GetEnumerator()
        {
            return (_text as IEnumerable<string>).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

    }

}
