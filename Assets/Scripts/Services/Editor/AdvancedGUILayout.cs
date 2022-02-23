using UnityEditor;
using UnityEngine;

namespace Tools.Util.Extensions
{
    public static class AdvancedGUILayout
    {
        
#if UNITY_EDITOR
        public static void RichHelpBox(string text)
        {
            var helpBoxStyle = GUI.skin.GetStyle("HelpBox");
            helpBoxStyle.richText = true;
            
            EditorGUILayout.TextArea(text, helpBoxStyle);
        }

        public static string SearchField(string searchString)
        {
            EditorGUILayout.BeginHorizontal();
            searchString = GUILayout.TextField(searchString, EditorStyles.toolbarSearchField);
            
            if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            {
                // Remove focus if cleared
                searchString = "";
                GUI.FocusControl(null);
            }
            EditorGUILayout.EndHorizontal();

            return searchString;
        }
        
        public static void HorizontalLayoutHead(string text, int spacesAbove = 1)
        {
            for (var i = 0; i < spacesAbove; i++)
            {
                EditorGUILayout.Space();
            }
            
            VerticalLayoutHead(text, spacesAbove);
            
            GUILayout.BeginHorizontal();
        }
        
        public static void HorizontalLayoutEnd()
        {
            GUILayout.EndHorizontal();
            VerticalLayoutEnd();
        }
        
        
        public static void VerticalLayoutHead(string text, int spacesAbove = 1)
        {
            for (var i = 0; i < spacesAbove; i++)
            {
                EditorGUILayout.Space();
            }
            
            GUILayout.BeginVertical(text, "window");
            GUILine();
        }

        public static void VerticalLayoutConnector(string text, int spacesInBetween = 1)
        {
            EditorGUILayout.EndVertical();
            for (var i = 0; i < spacesInBetween; i++)
            {
                EditorGUILayout.Space();
            }

            GUILayout.BeginVertical(text, "window");
            GUILine();
        }

        public static void VerticalLayoutEnd()
        {
            GUILayout.EndVertical();
        }

        public static void SeparationLine(int height = 2)
        {
            EditorGUILayout.Space();
            GUILine(height);
            EditorGUILayout.Space();
        }

        public static void GUILine(int height = 1 )
        {
            var rect = EditorGUILayout.GetControlRect(false, height );
            rect.height = height;
            EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );
        }

        public static void GreenLightLabelField(string displayString)
        {
            var guiContent = EditorGUIUtility.IconContent("greenLight");
            guiContent.text = displayString;
            EditorGUILayout.LabelField(guiContent);
        }
        
        public static void OrangeLightLabelField(string displayString)
        {
            var guiContent = EditorGUIUtility.IconContent("orangeLight");
            guiContent.text = displayString;
            EditorGUILayout.LabelField(guiContent);
        }

        public static void RedLightLabelField(string displayString)
        {
            var guiContent = EditorGUIUtility.IconContent("redLight");
            guiContent.text = displayString;
            EditorGUILayout.LabelField(guiContent);
        }


        public static void BeginVerticalWithColor(Color bgColor)
        {
            var guiStyle = new GUIStyle
            {
                normal =
                {
                    background = MakeTex(600, 1, bgColor)
                }
            };
            EditorGUILayout.BeginVertical(guiStyle);
        }
#endif
        private static Texture2D MakeTex(int width, int height, Color col)
        {
            var pix = new Color[width*height];
 
            for(var i = 0; i < pix.Length; i++)
                pix[i] = col;
 
            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
 
            return result;
        }
        
        
    }
}
