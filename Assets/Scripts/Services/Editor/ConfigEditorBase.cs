using Extensions;
using Tools.Util.Extensions;
using UnityEditor;
using UnityEngine;

namespace Services.Editor
{
    public class ConfigEditorBase<T> : EditorWindow where T : ScriptableObject
    {
        protected T TargetConfig;
        
        protected Color BackgroundColor = new Color(0.07f, 0.8f, 0.42f);
        protected Color HighlightColor = new Color(0.54f, 0.8f, 1f);

        public static void ShowWindow<TWindow>(T targetConfig, bool utilityWindow, string title) where TWindow : ConfigEditorBase<T>
        {
            var poolConfigEditor = (TWindow) GetWindow(typeof(TWindow), utilityWindow, title);
        
            poolConfigEditor.ChangeTargetConfig(targetConfig);
        }
        
        protected virtual void SaveButton(string buttonTxt, string notificationTxt)
        {
            if (GUILayout.Button(buttonTxt, GUILayout.Height(50)))
            {
                TargetConfig.Save();
                ShowNotification(new GUIContent(notificationTxt));
            }
        }

        protected virtual void OnGUI()
        {
            DrawConfigEditorHeader();
        }

        protected virtual void DrawConfigEditorHeader()
        {
            AdvancedGUILayout.BeginVerticalWithColor(BackgroundColor);
            var defaultColor = GUI.color;
            GUI.color = HighlightColor;

            SaveButton("Save Config", "Config Saved");

            EditorGUILayout.Space();

            if (InitializeConfig() == false)
            {
                EditorGUILayout.EndVertical();
                return;
            }

            EditorGUILayout.Space();

            GUI.color = defaultColor;
            EditorGUILayout.EndVertical();
        }
        
        
        protected bool InitializeConfig()
        {
            var newTargetConfig = (T) EditorGUILayout.ObjectField("Target Pool Config: ", TargetConfig,
                typeof(T), false);
        
            if (newTargetConfig == null)
            {
                return false;
            }
            else if (newTargetConfig != TargetConfig)
            {
                //On target Pool changed
                ChangeTargetConfig(newTargetConfig);

                return true;
            }

            return true;
        }

        protected virtual void ChangeTargetConfig(T targetConfig)
        {
            TargetConfig = targetConfig;
        }
    }
}