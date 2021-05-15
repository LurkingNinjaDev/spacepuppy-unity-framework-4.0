﻿#pragma warning disable 0649 // variable declared but not used.

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using com.spacepuppy.Events;
using com.spacepuppy.Utils;

namespace com.spacepuppy.AI
{

    public abstract class SimpleAIStateComponent : SPComponent, IAIState
    {

        #region Fields
        
        [SerializeField()]
        private SPEvent _onEnterState;
        [SerializeField]
        private SPEvent _onExitState;

        [System.NonSerialized()]
        private bool _isActive;

        #endregion

        #region CONSTRUCTOR

        #endregion

        #region Properties

        public SPEvent OnEnterState { get { return _onEnterState; } }

        public SPEvent OnExitState { get { return _onExitState; } }

        #endregion

        #region Methods

        protected abstract void OnStateEntered(IAIStateMachine machine, IAIState lastState);

        protected abstract void OnStateExited(IAIStateMachine machine, IAIState nextState);

        protected abstract void Tick(IAIController ai);

        #endregion

        #region IAIState Interface

        string IAINode.DisplayName
        {
            get
            {
                if (Application.isPlaying)
                    return string.Format("[State {0}] ({1})", this.name, (this.IsActive) ? "active" : "inactive");
                else
                    return string.Format("[State {0}]", this.name);
            }
        }
        
        public bool IsActive { get { return _isActive; } }

        public IAIStateMachine StateMachine
        {
            get;
            private set;
        }

        void IAIState.Init(IAIStateMachine machine)
        {
            this.StateMachine = machine;
            this.OnInit(machine);
        }

        protected virtual void OnInit(IAIStateMachine machine)
        {

        }

        void IAIState.OnStateEntered(IAIStateMachine machine, IAIState lastState)
        {
            _isActive = true;
            this.OnStateEntered(machine, lastState);
            _onEnterState.ActivateTrigger(this, null);
        }

        void IAIState.OnStateExited(IAIStateMachine machine, IAIState nextState)
        {
            this.OnStateExited(machine, nextState);
            _isActive = false;
            _onExitState.ActivateTrigger(this, null);
        }




        ActionResult IAINode.Tick(IAIController ai)
        {
            if(_isActive)
            {
                this.Tick(ai);
                return ActionResult.Waiting;
            }
            else
            {
                return ActionResult.None;
            }
        }

        void IAINode.Reset()
        {

        }
        
        #endregion

    }
}
