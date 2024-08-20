﻿#pragma warning disable 0649 // variable declared but not used.
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Threading.Tasks;

using com.spacepuppy;

namespace com.spacepuppyeditor.Settings
{

    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
    public class BulkBuildPostBuildCallbackAttribute : System.Attribute
    {

    }

    [CreateAssetMenu(fileName = "BulkBuildSettings", menuName = "Spacepuppy Build Pipeline/Bulk Build Settings")]
    public class BulkBuildSettings : ScriptableObject
    {

        [System.Flags]
        public enum ScriptOptions
        {
            Nothing = 0,
            Run = 1,
            CancelIfBuildFails = 2,
            BlockUntilComplete = 4
        }

        #region Fields

        [SerializeField]
        [ReorderableArray]
        private List<BuildSettings> _builds;

        [SerializeField, EnumFlags, UnityEngine.Serialization.FormerlySerializedAs("_postBuildScriptRunOptions")]
        private ScriptOptions _buildScriptRunOptions;

        [SerializeField, ReorderableArray]
        private List<string> _preBuildScripts;

        [SerializeField, ReorderableArray]
        private List<string> _postBuildScripts;

        #endregion

        #region Properties

        public List<BuildSettings> Builds
        {
            get { return _builds; }
        }

        public ScriptOptions PostBuildScriptRunOptions
        {
            get { return _buildScriptRunOptions; }
            set { _buildScriptRunOptions = value; }
        }

        public List<string> PostBuildScripts
        {
            get { return _postBuildScripts; }
        }

        #endregion

        #region Methods

        public async Task BuildAsync(BuildSettings.PostBuildOption postBuildOption = BuildSettings.PostBuildOption.Nothing)
        {
            await Task.Delay(10);

            if ((_buildScriptRunOptions & ScriptOptions.Run) != 0)
            {
                this.RunScripts(_preBuildScripts);
            }

            await Task.Delay(10);

            bool failed = false;
            foreach (var settings in _builds)
            {
                if (settings != null)
                {
                    if (!(await settings.BuildAsync(postBuildOption)))
                    {
                        failed = true;
                    }
                    await Task.Delay(1000);
                }
            }

            foreach (var m in TypeCache.GetMethodsWithAttribute<BulkBuildPostBuildCallbackAttribute>())
            {
                try
                {
                    m.Invoke(null, new object[] { this });
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            if ((_buildScriptRunOptions & ScriptOptions.Run) == 0) return;
            if ((_buildScriptRunOptions & ScriptOptions.CancelIfBuildFails) != 0 && failed) return;

            this.RunScripts(_postBuildScripts);
        }

        private void RunScripts(IList<string> scripts)
        {
            foreach (var str in scripts)
            {
                if (string.IsNullOrEmpty(str)) continue;

                try
                {
                    string path = str;
                    if (path.StartsWith("."))
                    {
                        path = System.IO.Path.Combine(Application.dataPath, str);
                        path = System.IO.Path.GetFullPath(path);
                    }

                    if (System.IO.File.Exists(path))
                    {
                        var proc = new System.Diagnostics.Process();
                        proc.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
                        proc.StartInfo.FileName = System.IO.Path.GetFileName(path);
                        proc.StartInfo.CreateNoWindow = false;
                        proc.Start();

                        if ((_buildScriptRunOptions & ScriptOptions.BlockUntilComplete) != 0)
                        {
                            proc.WaitForExit();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        #endregion

    }

    [CustomEditor(typeof(BulkBuildSettings))]
    public class BulkBuildSettingsEditor : SPEditor
    {

        protected override void OnSPInspectorGUI()
        {
            base.OnSPInspectorGUI();

            EditorGUILayout.Space(5f);

            var arr = TypeCache.GetMethodsWithAttribute<BulkBuildPostBuildCallbackAttribute>();
            if (arr.Count > 0)
            {
                if (GUILayout.Button("Run Only BulkBuildPostBuildCallbacks"))
                {
                    foreach (var m in arr)
                    {
                        try
                        {
                            m.Invoke(null, new object[] { this.target });
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogException(ex);
                        }
                    }
                }

                EditorGUILayout.Space(5f);
            }

            if (GUILayout.Button("Build"))
            {
                if (EditorUtility.DisplayDialog("Build?", "Confirm that you want to perform a bulk build.", "Yes", "Cancel"))
                {
                    var settings = this.target as BulkBuildSettings;
                    if (settings == null) return;

                    _ = settings.BuildAsync();
                }
            }
        }

    }

}
