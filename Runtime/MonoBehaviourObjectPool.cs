using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace YuzuValen.Utils
{
    public class MonoBehaviourObjectPool : MonoBehaviourSingleton<MonoBehaviourObjectPool>
    {
        [SerializeField] private List<PoolConfigObject> pooledPrefabsList = new();
        private HashSet<GameObject> pooledPrefabs = new();
        private Dictionary<GameObject, ObjectPool<GameObject>> pooledObjects = new();

        private void Awake()
        {
            // Registers all objects in PooledPrefabsList to the cache.
            foreach (var configObject in pooledPrefabsList)
            {
                RegisterPrefabInternal(configObject.Prefab, configObject.PrewarmCount);
            }
            UnityEngine.Debug.Log($"Prewarmed {string.Join(", ", pooledPrefabsList.Select(s => s.Prefab.name))}");
        }

        private void RegisterPrefabInternal(GameObject prefab, int prewarmCount)
        {
            GameObject CreateFunc() => Instantiate(prefab, transform);

            void ActionOnGet(GameObject obj)
            {
                obj.SetActive(true);
            }

            void ActionOnRelease(GameObject obj)
            {
                obj.SetActive(false);
            }

            void ActionOnDestroy(GameObject obj)
            {
                Destroy(obj);
            }

            pooledPrefabs.Add(prefab);
            // Create the pool
            pooledObjects[prefab] = new ObjectPool<GameObject>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy,
                defaultCapacity: prewarmCount);

            // Populate the pool
            var prewarmObjs = new List<GameObject>();
            for (var i = 0; i < prewarmCount; i++)
            {
                prewarmObjs.Add(pooledObjects[prefab].Get());
            }
            foreach (var obj in prewarmObjs)
            {
                pooledObjects[prefab].Release(obj);
            }
        }

        /// <summary>
        /// Gets an instance of the given prefab from the pool. The prefab must be registered to the pool.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="position">The position to spawn the object at.</param>
        /// <param name="rotation">The rotation to spawn the object with.</param>
        /// <returns></returns>
        public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var obj = pooledObjects[prefab].Get();
            obj.transform.SetPositionAndRotation(position, rotation);

            return obj;
        }

        /// <summary>
        /// Return an object to the pool (reset objects before returning).
        /// </summary>
        public void Return(GameObject obj, GameObject prefab)
        {
            pooledObjects[prefab].Release(obj);
        }
        public GameObject GetForDuration(GameObject prefab, Vector3 position, Quaternion rotation, float duration)
        {
            var obj = Get(prefab, position, rotation);
            StartCoroutine(ReturnAfterDuration(obj, prefab, duration));
            return obj;
        }

        private IEnumerator ReturnAfterDuration(GameObject obj, GameObject prefab, float duration)
        {
            yield return new WaitForSeconds(duration);
            Return(obj, prefab);
        }
    }
    [Serializable]
    struct PoolConfigObject
    {
        public GameObject Prefab;
        public int PrewarmCount;
    }
}
