using Tools.Pool;
using UnityEditor;
using UnityEngine;

namespace Services.Editor
{
    [CustomEditor(typeof(PoolConfig))]
    public class PoolConfigInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Pool Configurator"))
            {
                PoolConfigEditor.ShowWindow<PoolConfigEditor>( (PoolConfig) target, false, "Pool Config Editor");
            }
        }
    }
}