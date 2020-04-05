﻿#pragma warning disable 0649 // variable declared but not used.
using UnityEngine;
using System.Collections.Generic;

using com.spacepuppy.Geom;
using com.spacepuppy.Utils;

namespace com.spacepuppy.Events
{

    public class t_OnTriggerOccupied : SPComponent, ICompoundTriggerEnterResponder, ICompoundTriggerExitResponder, IOccupiedTrigger
    {

        #region Fields

        [SerializeField]
        private EventActivatorMaskRef _mask = new EventActivatorMaskRef();
        
        [SerializeField]
        private SPEvent _onTriggerOccupied;

        [SerializeField]
        private SPEvent _onTriggerLastExited;
        
        [System.NonSerialized]
        private HashSet<GameObject> _activeObjects = new HashSet<GameObject>();

        #endregion
        
        #region Properties

        public SPEvent OnTriggerOccupied
        {
            get { return _onTriggerOccupied; }
        }

        public SPEvent OnTriggerLastExited
        {
            get { return _onTriggerLastExited; }
        }
        
        public IEventActivatorMask Mask
        {
            get { return _mask.Value; }
            set { _mask.Value = value; }
        }

        public bool IsOccupied
        {
            get { return _activeObjects.Count > 0; }
        }

        #endregion

        #region Methods

        private void AddObject(GameObject obj)
        {
            if (_mask.Value != null && !_mask.Value.Intersects(obj)) return;

            if (_activeObjects.Count == 0)
            {
                _activeObjects.Add(obj);
                _onTriggerOccupied.ActivateTrigger(this, obj);
            }
            else
            {
                _activeObjects.Add(obj);
            }
        }

        private void RemoveObject(GameObject obj)
        {
            if (_activeObjects.Count == 0) return;
            
            _activeObjects.Remove(obj);
            if (_activeObjects.Count == 0)
            {
                _onTriggerLastExited.ActivateTrigger(this, obj);
            }
        }

        #endregion

        #region Messages

        void OnTriggerEnter(Collider other)
        {
            if (this.HasComponent<CompoundTrigger>() || other == null) return;

            this.AddObject(other.gameObject);
        }

        void OnTriggerExit(Collider other)
        {
            if (this.HasComponent<CompoundTrigger>() || other == null) return;

            this.RemoveObject(other.gameObject);
        }

        void ICompoundTriggerEnterResponder.OnCompoundTriggerEnter(Collider other)
        {
            if (other == null) return;
            this.AddObject(other.gameObject);
        }

        void ICompoundTriggerExitResponder.OnCompoundTriggerExit(Collider other)
        {
            if (other == null) return;
            this.RemoveObject(other.gameObject);
        }

        #endregion
        
        #region IObservableTrigger Interface

        BaseSPEvent[] IObservableTrigger.GetEvents()
        {
            return new BaseSPEvent[] { _onTriggerOccupied, _onTriggerLastExited };
        }

        BaseSPEvent IOccupiedTrigger.EnterEvent
        {
            get { return _onTriggerOccupied; }
        }

        BaseSPEvent IOccupiedTrigger.ExitEvent
        {
            get { return _onTriggerLastExited; }
        }

        #endregion

    }

}
