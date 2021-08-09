﻿using UnityEngine;

using com.spacepuppy.Events;
using com.spacepuppy.Geom;

namespace com.spacepuppy.SPInput.Events
{

    public sealed class t_OnMouseClick : TriggerComponent
    {

        #region Fields

        [SerializeField()]
        [Tooltip("A duration of time that the click must be held down to register as a click.")]
        private Interval _buttonLapse = Interval.MinMax(float.NegativeInfinity, float.PositiveInfinity);

        [System.NonSerialized()]
        private float _downT = float.NaN;

        #endregion

        #region Methods

        void OnMouseDown()
        {
            _downT = Time.unscaledTime;
        }

        void OnMouseUpAsButton()
        {
            if (float.IsNaN(_downT)) return;

            if (_buttonLapse.Intersects(Time.unscaledTime - _downT))
            {
                this.ActivateTrigger();
            }
            _downT = float.NaN;
        }

        #endregion

    }

}
