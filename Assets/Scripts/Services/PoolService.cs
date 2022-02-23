using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tools.Pool;
using UnityEngine;

namespace Services
{
    public class PoolService : MonoBehaviour
    {
        [SerializeField] private int preloadAllowedPerPass = 10;
        [SerializeField] private PoolConfig[] poolConfigs = default;

        public bool debugMode = false;
        
        //this is when de-spawning, so pool-service knows which pool the gameObject come from
        private readonly Dictionary<GameObject, string> _gameObjectPoolItemEnumMap =
            new Dictionary<GameObject, string>();
        
        private readonly Dictionary<string, ObjectPool> _objPools = new Dictionary<string, ObjectPool>();
        private int _totalItemsToPreload;
        private Assembly _gameAssembly;

        public PoolPreloadAsyncHandle PreloadAllPools()
        {
            _gameAssembly = Assembly.GetCallingAssembly();
            _totalItemsToPreload = poolConfigs.Sum(poolConfig => poolConfig.TotalPreloadAmount);
            
            //Making sure there's a pool for this item before preload starts
            foreach (var poolConfig in poolConfigs)
            {
                foreach (var poolItemConfig in poolConfig.poolItems)
                {
                    //Making sure there's a pool for this item
                    var targetPool = GetPool(poolItemConfig.poolItemName, false);
                    if (targetPool == null)
                    {
                        targetPool = new ObjectPool(this, poolItemConfig.prefab, transform,
                            poolItemConfig.TargetType(_gameAssembly), poolItemConfig.listenerCallback);

                        _objPools.Add(poolItemConfig.poolItemName, targetPool);
                    }
                }
            }

            var asyncOperationHandle = new PoolPreloadAsyncHandle(PreloadPass);
            StartCoroutine(asyncOperationHandle);
            return asyncOperationHandle;
        }
        
        private float PreloadPass()
        {
            // Calculate result progress, the previous total preloaded, plus the maximum potential of new preloaded this pass. 
            var previousPreloadedAmount = 0;
            foreach (var objectPool in _objPools.Values)
            {
                previousPreloadedAmount += objectPool.PreloadedAmount;
            }

            var preloadedCounter = 0;
            foreach (var poolConfig in poolConfigs)
            {
                foreach (var poolItemConfig in poolConfig.poolItems)
                {
                    
                    var targetPool = GetPool(poolItemConfig.poolItemName);
                    
                    if (debugMode == false)
                    {
                        int poolPreloadedAmount = targetPool.PreloadedAmount;
                        for (var i = poolPreloadedAmount; i < poolItemConfig.toPreloadAmount; i++)
                        {
                            // Check if still allowed to preload in this pass
                            if (preloadedCounter < preloadAllowedPerPass)
                            {
                                targetPool.Preload();
                                preloadedCounter++;
                            }
                            else
                            {
                                // If not, End pass
                                return (float) (previousPreloadedAmount + preloadedCounter) / _totalItemsToPreload;
                            }
                        }
                    }
                }
            }

            return 1.0f;
        }

        private ObjectPool GetPool(string poolItemName, bool allowWarning = true)
        {
            if (_objPools.TryGetValue(poolItemName, out ObjectPool pool))
            {
                return pool;
            }

            if (allowWarning) 
            { 
                Debug.LogError($"Failed to find pool for {poolItemName.ToString()}");
            }
            return null;
        }

        #region Spawn Related

        public T Spawn<T>(string poolItemName, Vector3 position, Quaternion rotation, float scale = 1, Transform parent = null) where T : Component
        {
            T result = GetPool(poolItemName)?.Pop<T>(position, rotation, scale, parent);
            
            if (result != null)
            {
                _gameObjectPoolItemEnumMap.Add(result.gameObject, poolItemName);
            }
            
            return result;   
        }
        
        public GameObject Spawn(string poolItemName, Vector3 position, Quaternion rotation, float scale = 1, Transform parent = null)
        {
            GameObject result = GetPool(poolItemName)?.Pop(position, rotation, scale, parent);
            if (result != null)
            {
                _gameObjectPoolItemEnumMap.Add(result, poolItemName);
            }

            return result;
        }
        
        public void Despawn(Component cp)
        {
            Despawn(cp.gameObject);
        }
        
        public void Despawn(GameObject go)
        {
            if (_gameObjectPoolItemEnumMap.TryGetValue(go, out var poolItemEnum))
            {
                _gameObjectPoolItemEnumMap.Remove(go);
                GetPool(poolItemEnum)?.Push(go);
            }
            else
            {
                Debug.LogWarning("Cannot get gameObject _gameObjectPrefabMap");
            }
        }
        
        public void Despawn(GameObject go, float delay)
        {
            if (delay <= 0)
            {
                Despawn(go);
                return;
            }

            StartCoroutine(DelayDespawn(go, delay));
        }

        private IEnumerator DelayDespawn(GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);
            Despawn(go);
        }
        #endregion
    }
}