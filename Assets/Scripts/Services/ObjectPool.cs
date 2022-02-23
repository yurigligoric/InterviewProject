using System;
using System.Collections.Generic;
using Tools.Pool;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services
{
    public class ObjectPool
    {
        private class ObjectPoolItemCache
        {
            public readonly Component ComponentCache;
            public readonly HashSet<IPoolInit> OnInitListeners;
            public readonly HashSet<IPoolOnSpawn> OnSpawnListeners;
            public readonly HashSet<IPoolOnDespawn> OnDespawnListeners;
            
            public ObjectPoolItemCache(GameObject poolObject, Type type, bool listenerCallback)
            {
                ComponentCache = poolObject.GetComponent(type);

                if (listenerCallback)
                {
                    //Init cache
                    OnInitListeners = new HashSet<IPoolInit>();
                    foreach (var initComponent in poolObject.GetComponents<IPoolInit>())
                    {
                        OnInitListeners.Add(initComponent);   
                    }
                    
                    foreach (var initComponent in poolObject.GetComponentsInChildren<IPoolInit>())
                    {
                        OnInitListeners.Add(initComponent);   
                    }
                    
                    foreach (var listener in OnInitListeners)
                    {
                        listener.Init();
                    }
                    
                    //Spawn cache
                    OnSpawnListeners = new HashSet<IPoolOnSpawn>();
                    foreach (var onSpawnComponent in poolObject.GetComponents<IPoolOnSpawn>())
                    {
                        OnSpawnListeners.Add(onSpawnComponent);   
                    }
                    
                    foreach (var onSpawnComponent in poolObject.GetComponentsInChildren<IPoolOnSpawn>())
                    {
                        OnSpawnListeners.Add(onSpawnComponent);   
                    }

                    //Despawn cache
                    OnDespawnListeners = new HashSet<IPoolOnDespawn>();
                    foreach (var onDespawnComponent in poolObject.GetComponents<IPoolOnDespawn>())
                    {
                        OnDespawnListeners.Add(onDespawnComponent);   
                    }
                    
                    foreach (var onDespawnComponent in poolObject.GetComponentsInChildren<IPoolOnDespawn>())
                    {
                        OnDespawnListeners.Add(onDespawnComponent);   
                    }
                }
            }
        }
        
        private readonly GameObject _prefab;
        private readonly Transform _poolParent;
        private readonly bool _listenerCallback;
        private readonly Type _type;
        private readonly PoolService _poolService;
        
        private readonly Queue<GameObject> _storedPoolObjects = new Queue<GameObject>();
        private readonly Dictionary<GameObject, ObjectPoolItemCache> _objectPoolItems = new Dictionary<GameObject, ObjectPoolItemCache>();
        public int PreloadedAmount => _storedPoolObjects.Count;

        public ObjectPool(PoolService poolService, GameObject prefab, Transform poolParent, Type type, bool listenerCallback)
        {
            _poolService = poolService;
            _prefab = prefab;
            _poolParent = poolParent;
            _listenerCallback = listenerCallback;
            _type = type;
        }
        
        public void Preload()
        {
            var go = Object.Instantiate(_prefab, _poolParent, true);
            go.SetActive(false);
            var newObjPoolItem = new ObjectPoolItemCache(go, _type, _listenerCallback);
            _storedPoolObjects.Enqueue(go);
            _objectPoolItems.Add(go, newObjPoolItem);
        }

        #region Pop methods

        public T Pop<T>(Vector3 position, Quaternion rotation, float scale, Transform parent) where T : Component
        {
            if (_storedPoolObjects.Count <= 0)
            {
                Preload();
            }
        
            GameObject poolObj = _storedPoolObjects.Dequeue();
            var transform = poolObj.transform;

            transform.SetParent(parent);
            transform.localScale = Vector3.one * scale;
            transform.localPosition = position;
            transform.localRotation = rotation;
        
            poolObj.SetActive(true);


            if (typeof(T) != _type)
            {
                Debug.LogWarning($"Tried to get a different type <{typeof(T)}> than the stored one <{_type}> when popping {_prefab.name}");
                return null;
            }

            if (_objectPoolItems.TryGetValue(poolObj, out ObjectPoolItemCache objPoolItem) == false || objPoolItem == null)
            {
                Debug.LogWarning($"Failed to find ObjectPoolItem for {poolObj.name}");
                return null;
            }

            T result = (T) objPoolItem.ComponentCache;
            if (_listenerCallback)
            {
                foreach (var poolListener in objPoolItem.OnSpawnListeners)
                {
                    poolListener.OnSpawn(_poolService);
                }
            }

            return result;
        }

        public GameObject Pop(Vector3 position, Quaternion rotation, float scale, Transform parent = null)
        {
            if (_storedPoolObjects.Count <= 0)
            {
                Preload();
            }
        
            GameObject poolObj = _storedPoolObjects.Dequeue();
            var transform = poolObj.transform;

            if (parent != null)
            {
                transform.SetParent(parent);                
            }
            
            transform.localScale = Vector3.one * scale;
            transform.localPosition = position;
            transform.localRotation = rotation;
            poolObj.SetActive(true);
            
            if (_objectPoolItems.TryGetValue(poolObj, out ObjectPoolItemCache objPoolItem) == false || objPoolItem == null)
            {
                Debug.LogWarning($"Failed to find ObjectPoolItem for {poolObj.name}");
                return null;
            }
            
            if (_listenerCallback)
            {
                foreach (var listener in objPoolItem.OnSpawnListeners)
                {
                    listener.OnSpawn(_poolService);
                }
            }
        
            return poolObj;
        }
        #endregion

        #region Push methods

        public void Push(GameObject go)
        {
            if (_listenerCallback)
            {
                if (_objectPoolItems.TryGetValue(go, out ObjectPoolItemCache objPoolItem) == false || objPoolItem == null)
                {
                    Debug.LogWarning($"Failed to find ObjectPoolItem for {go.name}");
                    return;
                }
                
                foreach (var listener in objPoolItem.OnDespawnListeners)
                {
                    listener.OnDespawn();
                }
            }

            if (_poolService.debugMode)
            {
                GameObject.Destroy(go);
                return;
            }
            
            go.transform.localScale = Vector3.one;
            go.SetActive(false);
            go.transform.SetParent(_poolParent);
            _storedPoolObjects.Enqueue(go);
        }


        private void Push(Component cp)
        {
            Push(cp.gameObject);
        }

        #endregion
    }
}