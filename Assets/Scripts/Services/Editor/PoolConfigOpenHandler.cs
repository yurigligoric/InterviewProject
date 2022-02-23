using Services;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Tools.Pool
{
    public static class PoolConfigOpenHandler
    {
        [OnOpenAsset(1)]
        public static bool OpenPoolConfigEditor(int instanceID, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj is PoolConfig config)
            {
                PoolConfigEditor.ShowWindow<PoolConfigEditor>(config, false, "Pool Editor");
                return true;// we did handle the open
            }
            else
            {
                return false;
            }
        }
    }
}