﻿using UnityEngine;
using UnityEditor;

using com.spacepuppy;
using com.spacepuppy.Dynamic;
using com.spacepuppy.Utils;

namespace com.spacepuppyeditor.Core
{

    [CustomPropertyDrawer(typeof(DisplayIfAttribute))]
    public class DisplayIfModifier : PropertyModifier
    {

        protected internal override void OnBeforeGUI(SerializedProperty property, ref bool cancelDraw)
        {
            var attrib = this.attribute as DisplayIfAttribute;
            var targ = EditorHelper.GetTargetObjectWithProperty(property);

            if (property.serializedObject.isEditingMultipleObjects)
            {
                cancelDraw = true;
            }
            else if (attrib != null && targ != null)
            {
                cancelDraw = !ConvertUtil.ToBool(DynamicUtil.GetValue(targ, attrib.MemberName));
                if (attrib.DisplayIfNot) cancelDraw = !cancelDraw;
            }
        }

    }

}
