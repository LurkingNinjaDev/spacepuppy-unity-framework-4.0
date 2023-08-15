﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

using com.spacepuppy.Motor;

namespace com.spacepuppyeditor.Motor
{

    [CustomPropertyDrawer(typeof(ConfigurableForce))]
    public class ConfigurableForcePropertyDrawer : PropertyDrawer
    {

        public const string PROP_DIR = "_direction";
        public const string PROP_STRENGTH = "_strength";
        public const string PROP_FORCEMODE = "_forceMode";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, property.isExpanded);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
            else
            {
                var dirProp = property.FindPropertyRelative(PROP_DIR);
                var strProp = property.FindPropertyRelative(PROP_STRENGTH);
                var modeProp = property.FindPropertyRelative(PROP_FORCEMODE);
                //var content = string.Format("[Dir:{0}, Str:{1:0.00}, Mode:{2}]", (ConfigurableForce.ForceDirection)dirProp.enumValueIndex, strProp.floatValue, (ForceMode)modeProp.enumValueIndex);
                var content = string.Format("[Dir:{0}, Str:{1:0.00}, Mode:{2}]", dirProp.GetEnumValue<ConfigurableForce.ForceDirection>(), strProp.floatValue, modeProp.GetEnumValue<ForceMode>());

                var r1 = new Rect(position.xMin, position.yMin, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
                var r2 = new Rect(r1.xMax, r1.yMin, position.width - r1.width, r1.height);
                property.isExpanded = EditorGUI.Foldout(r1, property.isExpanded, label, true);
                EditorGUI.LabelField(r2, content);
            }
        }

    }
}
