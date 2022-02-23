using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Services
{
    [CreateAssetMenu(fileName = "PoolConfig", menuName = "Create Pool Config", order = 0)]
    public class PoolConfig : ScriptableObject
    {
        [Serializable]
        public class PoolItemConfig
        {
            // stored in string as type can't be serialized, this is in the format of "UnityEngine.Rigidbody, UnityEngine" or "MyScript"
            [HideInInspector] [SerializeField] public string typeAndNamespace;
            
            //in order the get the type, the type must be in the callingAssembly
            public Type TargetType(Assembly typeContainingAssembly)
            {
                var targetType = Type.GetType(typeAndNamespace);

                if (targetType != null)
                {
                    return targetType;
                }

                targetType = typeContainingAssembly.GetType(typeAndNamespace);
                if (targetType is null)
                {
                    throw new Exception($"Failed to GetType(\"{typeAndNamespace}\") for obj name: {prefab.name}. " +
                                        $"Check if the caller of PoolService.PreloadAllPools is in the same assembly of \"{typeAndNamespace}\"");
                }
                    
                return targetType;
            }
            
            public string poolItemName;
            
            public GameObject prefab;

            public bool listenerCallback;
            public int toPreloadAmount = 1;
        }
    
        public List<PoolItemConfig> poolItems = new List<PoolItemConfig>();

        public int TotalPreloadAmount {
            get
            {
                int totalPreloadCount = 0;
                foreach (var poolItem in poolItems)
                {
                    totalPreloadCount += poolItem.toPreloadAmount;
                }

                return totalPreloadCount;
            }
        }
    }
}
