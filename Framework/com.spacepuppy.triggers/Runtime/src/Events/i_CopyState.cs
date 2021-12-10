﻿#pragma warning disable 0649 // variable declared but not used.

using UnityEngine;

using com.spacepuppy.Dynamic;
using com.spacepuppy.Utils;

namespace com.spacepuppy.Events
{

    public class i_CopyState : AutoTriggerable
    {

        #region Fields

        [SerializeField]
        [TriggerableTargetObject.Config(typeof(UnityEngine.Object))]
        private TriggerableTargetObject _target;

        [SerializeField]
        [TriggerableTargetObject.Config(typeof(UnityEngine.Object))]
        private TriggerableTargetObject _source;

        [SerializeField]
        [Tooltip("If 'Target' is a VariableStore or other dynamic object, it'll force add the member's of 'Source' that don't already exist on 'Target'.")]
        private bool _forceCopy;

        #endregion

        #region Properties

        public TriggerableTargetObject Target
        {
            get { return _target; }
        }

        public TriggerableTargetObject Source
        {
            get { return _source; }
        }

        /// <summary>
        /// If 'Target' is a VariableStore or other dynamic object, it'll force 
        /// add the member's of 'Source' that don't already exist on 'Target'.
        /// </summary>
        public bool ForceCopy
        {
            get { return _forceCopy; }
            set { _forceCopy = value; }
        }

        #endregion

        #region Triggerable Interface

        public override bool Trigger(object sender, object arg)
        {
            if (!this.CanTrigger) return false;

            var targ = _target.GetTarget<object>(arg);
            var source = _source.GetTarget<object>(arg);

            if (_forceCopy)
                TokenUtil.CopyState(targ, source);
            else
                TokenUtil.SyncState(targ, source);

            return true;
        }

        #endregion

    }

}
