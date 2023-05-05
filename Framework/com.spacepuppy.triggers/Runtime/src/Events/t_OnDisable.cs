﻿using UnityEngine;
using com.spacepuppy.Utils;

namespace com.spacepuppy.Events
{

    [Infobox("Due to the ordering of disable events in Unity, if this attempts to trigger a target that is itself disabled it won't be triggered. This includes if it is part of an entity and the entire entity is disabled.")]
    public class t_OnDisable : SPComponent, IObservableTrigger
    {

        #region Fields

        [SerializeField()]
        private SPEvent _trigger = new SPEvent();

        #endregion

        #region Properties

        public SPEvent Trigger => _trigger;

        #endregion

        #region Messages

        protected override void OnDisable()
        {
            base.OnDisable();

            _trigger.ActivateTrigger(this, null);
        }

        #endregion

        #region IObservableTrigger Interface

        BaseSPEvent[] IObservableTrigger.GetEvents()
        {
            return new BaseSPEvent[] { _trigger };
        }

        #endregion

    }

}
