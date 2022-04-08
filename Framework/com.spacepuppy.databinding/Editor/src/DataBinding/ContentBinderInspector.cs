using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

using com.spacepuppy;
using com.spacepuppy.Dynamic;
using com.spacepuppy.Utils;
using com.spacepuppyeditor;
using com.spacepuppy.DataBinding;
using com.spacepuppy.Collections;

namespace com.spacepuppyeditor.DataBinding
{

    //[CustomEditor(typeof(ContentBinder), true)]
    //public class ContentBinderInspector : SPEditor
    //{

    //    protected override void OnEnable()
    //    {
    //        base.OnEnable();

    //        var targ = this.serializedObject.targetObject as ContentBinder;
    //        if (targ && targ.GetComponent<IDataBindingContext>() == null)
    //        {
    //            var c = targ.gameObject.AddComponent<DataBindingContext>();
    //            using (var lst = TempCollection.GetList<Component>())
    //            {
    //                while(true)
    //                {
    //                    lst.Clear();
    //                    targ.gameObject.GetComponents<Component>(lst);

    //                    int icontext = lst.IndexOf(c);
    //                    if (icontext <= 0) break;

    //                    int ibinder = lst.IndexOf(o => o is ContentBinder);
    //                    if (ibinder < 0) break;

    //                    if (icontext < ibinder) break;

    //                    if (!UnityEditorInternal.ComponentUtility.MoveComponentUp(c)) break;
    //                }
    //            }
    //            this.serializedObject.Update();
    //        }
    //    }

    //    //protected override void OnSPInspectorGUI()
    //    //{
    //    //    ContentBinder target;
    //    //    DataBindingContext context;
    //    //    ISourceBindingProtocol protocol;
    //    //    if (this.serializedObject.isEditingMultipleObjects ||
    //    //        (target = this.serializedObject.targetObject as ContentBinder) == null ||
    //    //        (context = target.GetComponent<DataBindingContext>()) == null ||
    //    //        (protocol = context.BindingProtocol) == null)
    //    //    {
    //    //        this.DrawDefaultInspector();
    //    //        return;
    //    //    }

    //    //    var keys = protocol.GetDefinedKeys();
    //    //    if (keys == null || !keys.Any())
    //    //    {
    //    //        this.DrawDefaultInspector();
    //    //        return;
    //    //    }

    //    //    this.serializedObject.UpdateIfRequiredOrScript();

    //    //    this.DrawPropertyField(EditorHelper.PROP_SCRIPT);

    //    //    var keyprop = this.serializedObject.FindProperty(ContentBinder.PROP_KEY);
    //    //    keyprop.stringValue = SPEditorGUILayout.OptionPopupWithCustom(EditorHelper.TempContent(keyprop.displayName, keyprop.tooltip), keyprop.stringValue, keys.ToArray());

    //    //    this.DrawDefaultInspectorExcept(EditorHelper.PROP_SCRIPT, ContentBinder.PROP_KEY);

    //    //    this.serializedObject.ApplyModifiedProperties();
    //    //}

    //}

    [CustomPropertyDrawer(typeof(ContentBinderKeyAttribute))]
    public class ContentBinderKeyPropertyDrawer : PropertyDrawer
    {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, "ContentBinderKey attributed field is not of type 'string'.");
                return;
            }

            var sob = property.serializedObject;
            Component target;
            DataBindingContext context;
            ISourceBindingProtocol protocol;
            if (sob.isEditingMultipleObjects ||
                (target = sob.targetObject as Component) == null ||
                (context = target.GetComponent<DataBindingContext>()) == null ||
                (protocol = context.BindingProtocol) == null)
            {
                property.stringValue = EditorGUI.TextField(position, label, property.stringValue);
                return;
            }

            var keys = protocol.GetDefinedKeys(context);
            if (keys == null || !keys.Any())
            {
                property.stringValue = EditorGUI.TextField(position, label, property.stringValue);
                return;
            }

            property.stringValue = SPEditorGUI.OptionPopupWithCustom(position, label, property.stringValue, keys.ToArray());
        }

    }

}
