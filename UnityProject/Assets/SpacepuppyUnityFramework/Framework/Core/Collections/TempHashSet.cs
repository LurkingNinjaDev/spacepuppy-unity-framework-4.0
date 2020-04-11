﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.spacepuppy.Collections
{
    public class TempHashSet<T> : HashSet<T>, ITempCollection<T>
    {

        private const int MAX_SIZE_INBYTES = 1024;

        #region Fields

        private static ObjectCachePool<TempHashSet<T>> _pool = new ObjectCachePool<TempHashSet<T>>(-1, () => new TempHashSet<T>());

        #endregion

        #region CONSTRUCTOR

        public TempHashSet()
            : base()
        {
        }

        public TempHashSet(IEnumerable<T> e)
            : base(e)
        {
        }

        #endregion

        #region IDisposable Interface

        public void Dispose()
        {
            this.Clear();
            _pool.Release(this);
        }

        #endregion

        #region Static Methods

        public static TempHashSet<T> GetSet()
        {
            return _pool.GetInstance();
        }

        public static TempHashSet<T> GetSet(IEnumerable<T> e)
        {
            TempHashSet<T> result;
            if (_pool.TryGetInstance(out result))
            {
                var le = LightEnumerator.Create<T>(e);
                while(le.MoveNext())
                {
                    result.Add(le.Current);
                }
            }
            else
            {
                result = new TempHashSet<T>(e);
            }
            return result;
        }
        
        #endregion

    }
}
