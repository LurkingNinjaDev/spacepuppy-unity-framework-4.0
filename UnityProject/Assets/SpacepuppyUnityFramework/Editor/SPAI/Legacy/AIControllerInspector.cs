﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

using com.spacepuppy;
using com.spacepuppy.AI.Legacy;
using com.spacepuppy.StateMachine;
using com.spacepuppy.Utils;

namespace com.spacepuppyeditor.AI
{

    /// <summary>
    /// 
    /// </summary>
    /// <notes>
    /// </notes>
    [CustomEditor(typeof(AIController), true)]
    public class AIControllerInspector : SPEditor
    {

        public const string PROP_STATESOURCE = "_stateSource";
        public const string PROP_DEFAULTSTATE = "_defaultState";

        protected override void OnSPInspectorGUI()
        {
            this.serializedObject.Update();

            var targ = this.target as AIController;
            if (targ == null) return;

            this.DrawPropertyField(EditorHelper.PROP_SCRIPT);

            var sourceProp = this.serializedObject.FindProperty(PROP_STATESOURCE);
            SPEditorGUILayout.PropertyField(sourceProp);
            
            var cache = SPGUI.DisableIfPlaying();
            var stateProp = this.serializedObject.FindProperty(PROP_DEFAULTSTATE);
            
            switch(sourceProp.GetEnumValue<AIStateMachineSourceMode>())
            {
                case AIStateMachineSourceMode.SelfSourced:
                    {
                        var states = ComponentStateGroup<IAIState>.GetComponentsOnTarg(targ.gameObject).Cast<Component>().ToArray();
                        stateProp.objectReferenceValue = SPEditorGUILayout.SelectComponentField(stateProp.displayName, states, stateProp.objectReferenceValue as Component);
                    }
                    break;
                case AIStateMachineSourceMode.ChildSourced:
                    {
                        var states = ParentComponentStateGroup<IAIState>.GetComponentsOnTarg(targ.gameObject, false);
                        var names = (from s in states select EditorHelper.TempContent(GameObjectUtil.GetGameObjectFromSource(s).name + " (" + s.GetType().Name + ")")).ToArray();
                        int i = states.IndexOf(stateProp.objectReferenceValue);
                        i = EditorGUILayout.Popup(EditorHelper.TempContent(stateProp.displayName), i, names);
                        stateProp.objectReferenceValue = (i >= 0) ? states[i] as UnityEngine.Object : null;
                    }
                    break;
                default:
                    {
                        var states = ArrayUtil.Empty<Component>();
                        stateProp.objectReferenceValue = SPEditorGUILayout.SelectComponentField(stateProp.displayName, states, stateProp.objectReferenceValue as Component);
                    }
                    break;
            }

            cache.Reset();


            this.DrawDefaultInspectorExcept(EditorHelper.PROP_SCRIPT, PROP_STATESOURCE, PROP_DEFAULTSTATE);

            this.serializedObject.ApplyModifiedProperties();


            if (Application.isPlaying)
            {
                if (targ.States != null && targ.States.Current != null)
                {
                    var c = targ.States.Current;
                    var msg = string.Format("Currently active state is {0} ({1}).", c.DisplayName, c.GetType().Name);
                    EditorGUILayout.HelpBox(msg, MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox("Currently active state is null.", MessageType.Info);
                }
            }
        }

        public override bool RequiresConstantRepaint()
        {
            return Application.isPlaying;
        }

    }

}
