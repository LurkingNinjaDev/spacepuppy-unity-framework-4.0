﻿#pragma warning disable 0649 // variable declared but not used.

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using com.spacepuppy.Collections;
using com.spacepuppy.Utils;

namespace com.spacepuppy.Sensors.Visual
{

    public abstract class VisualSensor : Sensor
    {

        #region Fields

        [SerializeField()]
        private Color _sensorColor = Color.blue;
        [SerializeField()]
        private bool _canDetectSelf;
        [SerializeField]
        [Tooltip("A mask for things we can sense, leave blank to sense all possible aspects.")]
        private EventActivatorMaskRef _mask;
        
        [SerializeField()]
        [Tooltip("Leave as 'Nothing' to not calculate line of sight.")]
        private LayerMask _lineOfSightMask;

        #endregion

        #region Properties

        public Color SensorColor
        {
            get { return _sensorColor; }
            set { _sensorColor = value; }
        }

        public bool CanDetectSelf
        {
            get { return _canDetectSelf; }
            set { _canDetectSelf = value; }
        }

        public IEventActivatorMask Mask
        {
            get { return _mask.Value; }
            set { _mask.Value = value; }
        }
        
        public LayerMask LineOfSightMask
        {
            get { return _lineOfSightMask; }
            set { _lineOfSightMask = value; }
        }

        #endregion

        #region Methods

        private System.Func<IAspect, bool> _cachedPred;
        protected System.Func<IAspect, bool> GetPredicate(System.Func<IAspect, bool> p)
        {
            if(p == null)
            {
                if (_cachedPred == null) _cachedPred = this.Visible;
                return _cachedPred;
            }
            else
            {
                return (a) => this.Visible(a) && p(a);
            }
        }

        public abstract BoundingSphere GetBoundingSphere();

        public override bool ConcernedWith(UnityEngine.Object obj)
        {
            if(obj is VisualAspect)
            {
                return this.ConcernedWith(obj as VisualAspect);
            }
            else
            {
                var go = GameObjectUtil.GetGameObjectFromSource(obj);
                if (go == null) return false;
                using (var set = com.spacepuppy.Collections.TempCollection.GetSet<VisualAspect>())
                {
                    go.FindComponents<VisualAspect>(set);
                    var e = set.GetEnumerator();
                    while (e.MoveNext())
                    {
                        if (this.ConcernedWith(e.Current))
                            return true;
                    }
                    return false;
                }
            }
        }
        private bool ConcernedWith(VisualAspect vaspect)
        {
            if (vaspect == null) return false;
            if (!vaspect.isActiveAndEnabled) return false;
            if (this.Mask != null && !this.Mask.Intersects(vaspect.gameObject)) return false;
            if (!_canDetectSelf && vaspect.entityRoot == this.entityRoot) return false;

            return true;
        }

        public override bool SenseAny(System.Func<IAspect, bool> p = null)
        {
            //return VisualAspect.Pool.Any(this.GetPredicate(p));

            var sphere = this.GetBoundingSphere();
            using (var lst = TempCollection.GetList<IAspect>())
            {
                if (VisualAspect.GetNearby(lst, sphere.position, sphere.radius) > 0)
                {
                    p = this.GetPredicate(p);
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (p(lst[i])) return true;
                    }
                }
            }

            return false;
        }

        public override IAspect Sense(System.Func<IAspect, bool> p = null)
        {
            //p = this.GetPredicate(p);
            //return VisualAspect.Pool.Find(p);

            var sphere = this.GetBoundingSphere();
            using (var lst = TempCollection.GetList<IAspect>())
            {
                if (VisualAspect.GetNearby(lst, sphere.position, sphere.radius) > 0)
                {
                    p = this.GetPredicate(p);
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (p(lst[i])) return lst[i];
                    }
                }
            }

            return null;
        }

        public override IEnumerable<IAspect> SenseAll(System.Func<IAspect, bool> p = null)
        {
            //p = this.GetPredicate(p);
            //foreach (var a in VisualAspect.Pool)
            //{
            //    if (p(a)) yield return a;
            //}

            var sphere = this.GetBoundingSphere();
            using (var lst = TempCollection.GetList<IAspect>())
            {
                if (VisualAspect.GetNearby(lst, sphere.position, sphere.radius) > 0)
                {
                    p = this.GetPredicate(p);
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (p(lst[i])) yield return lst[i];
                    }
                }
            }
        }

        public override int SenseAll(ICollection<IAspect> results, System.Func<IAspect, bool> p = null)
        {
            //p = this.GetPredicate(p);
            //return VisualAspect.Pool.FindAll(results, p);

            int cnt = 0;
            var sphere = this.GetBoundingSphere();
            using (var lst = TempCollection.GetList<IAspect>())
            {
                if (VisualAspect.GetNearby(lst, sphere.position, sphere.radius) > 0)
                {
                    p = this.GetPredicate(p);
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (p(lst[i]))
                        {
                            results.Add(lst[i]);
                            cnt++;
                        }
                    }
                }
            }
            return cnt;
        }
        
        public override int SenseAll<T>(ICollection<T> results, System.Func<T, bool> p = null)
        {
            //System.Func<T, bool> p2;
            //if (p == null)
            //    p2 = (a) => this.Visible(a);
            //else
            //    p2 = (a) => this.Visible(a) && p(a);

            //return VisualAspect.Pool.FindAll<T>(results, p2);

            int cnt = 0;
            var sphere = this.GetBoundingSphere();
            using (var lst = TempCollection.GetList<IAspect>())
            {
                if (VisualAspect.GetNearby(lst, sphere.position, sphere.radius) > 0)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (lst[i] is T && this.Visible(lst[i]))
                        {
                            if (p != null && !p(lst[i] as T)) continue;
                            results.Add(lst[i] as T);
                            cnt++;
                        }
                    }
                }
            }
            return cnt;
        }

        public override bool Visible(IAspect aspect)
        {
            var vaspect = aspect as VisualAspect;

            if (vaspect == null) return false;
            if (!vaspect.isActiveAndEnabled) return false;
            if (this.Mask != null && !this.Mask.Intersects(vaspect.gameObject)) return false;
            if (!_canDetectSelf && vaspect.entityRoot == this.entityRoot) return false;
            return vaspect.OmniPresent || this.TestVisibility(vaspect);
        }

        protected abstract bool TestVisibility(VisualAspect aspect);

        #endregion
        
    }

}
