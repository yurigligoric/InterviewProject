using System;
using System.Linq;
using Extensions;
using Services;
using Services.Editor;
using Tools.Util.Extensions;
using UnityEditor;
using UnityEngine;

namespace Tools.Pool
{
    public class PoolConfigEditor : ConfigEditorBase<PoolConfig>
    {
        private Vector2 _scrollPos;
        private PoolConfig.PoolItemConfig _newPoolItemConfig;
        

        private PoolConfig[] _allPoolConfigs;

        private void OnEnable()
        {
            _allPoolConfigs = AssetDatabaseExtension.GetAssetsAtPath<PoolConfig>("Configs/PoolConfigs/");
        }

        protected override void OnGUI()
        {
            EditorGUILayout.Space();
            
            base.OnGUI();

            if (TargetConfig is null)
            {
                return;
            }

            // Scroll GUI
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            for (var index = 0; index < TargetConfig.poolItems.Count; index++)
            {
                var configItem = TargetConfig.poolItems[index];

                AdvancedGUILayout.HorizontalLayoutHead(configItem.poolItemName, 0);
                EditorGUILayout.BeginVertical();
                DrawPoolConfigItem(configItem);
                EditorGUILayout.EndVertical();
            
                //right part
                EditorGUILayout.BeginVertical(GUILayout.MaxWidth(20));

                var guiDelete = EditorGUIUtility.IconContent("P4_DeletedLocal@2x");
                if (GUILayout.Button(guiDelete))
                {
                    TargetConfig.poolItems.Remove(configItem);
                }
                
                EditorGUILayout.EndVertical();
                
                AdvancedGUILayout.HorizontalLayoutEnd();
            }

            EditorGUILayout.EndScrollView();
    
    
            // Bottom GUI
            GUILayout.FlexibleSpace();
        
            AdvancedGUILayout.BeginVerticalWithColor(BackgroundColor);
            var defaultColor = GUI.color;
            GUI.color = HighlightColor;
            
            DrawPoolConfigItem(_newPoolItemConfig);

            if (GUILayout.Button("+"))
            {
                TargetConfig.poolItems.Add(_newPoolItemConfig);
                _newPoolItemConfig = new PoolConfig.PoolItemConfig();
            }

            GUI.color = defaultColor;
            EditorGUILayout.EndVertical();
        }

        protected override void ChangeTargetConfig(PoolConfig targetConfig)
        {
            base.ChangeTargetConfig(targetConfig);
            
            _newPoolItemConfig = new PoolConfig.PoolItemConfig();
        
            //initialize default TargetType in case of new PoolItem
            foreach (var configItem in TargetConfig.poolItems)
            { 
                var componentList = configItem.prefab.GetComponents<Component>(); 
                var existTypeComponent = componentList.First(c => c.GetTypeWithNamespace() == configItem.typeAndNamespace);

                if (existTypeComponent == false)
                {
                    configItem.typeAndNamespace = componentList[0].GetTypeWithNamespace();
                }
            }
        }

        public static void DrawPoolConfigItem(PoolConfig.PoolItemConfig itemConfig, bool readonlyPrefab = false)
        {
            EditorGUI.BeginDisabledGroup(readonlyPrefab);
            itemConfig.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab ", itemConfig.prefab, typeof(GameObject), false);
            EditorGUI.EndDisabledGroup();
            
            if (itemConfig.prefab)
            {
                
                itemConfig.toPreloadAmount = EditorGUILayout.IntField("PreLoad Amount: ", itemConfig.toPreloadAmount); 
                itemConfig.listenerCallback = EditorGUILayout.Toggle("Listener Callback", itemConfig.listenerCallback);
            
                var allComponentsOnRoot = itemConfig.prefab.GetComponents<Component>();
                var typeOptions = allComponentsOnRoot.Select(c => c.GetTypeWithNamespace()).ToArray();
            
                EditorGUILayout.LabelField("Target Type: " + itemConfig.typeAndNamespace);
                var typeIndex = Array.FindIndex(typeOptions,  t => t == itemConfig.typeAndNamespace);
                if (typeIndex < 0)
                {
                    typeIndex = 0;
                }

                typeIndex = EditorGUILayout.Popup(typeIndex, typeOptions);
                itemConfig.typeAndNamespace = typeOptions[typeIndex];
            }
            
        
            EditorGUILayout.Space();
        }

        protected override void SaveButton(string buttonTxt, string notificationTxt)
        {
            if (GUILayout.Button(buttonTxt, GUILayout.Height(50)) == false)
            {
                return;
            }

            var allPoolConfigs = ScriptableExt.GetAllInstances<PoolConfig>();
            
            foreach (var poolConfig in allPoolConfigs)
            {
                foreach (var poolItem in poolConfig.poolItems)
                {
                    poolItem.poolItemName = poolItem.prefab.name;
                }
                poolConfig.Save();
            }
            
            //Update all prefabs with Odin validator
            ShowNotification(new GUIContent(notificationTxt));
        }
    }
}
