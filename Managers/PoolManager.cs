using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    /// <summary>
    /// 여러 프리팹의 풀을 한 곳에서 관리.
    /// 사용:
    ///   var go = PoolManager.Instance.Spawn(bulletPrefab, pos, rot);
    ///   PoolManager.Instance.Despawn(go);
    /// </summary>
    public class PoolManager : MonoSingleton<PoolManager>
    {
        // 프리팹 -> 그 프리팹 전용 풀
        private readonly Dictionary<GameObject, ObjectPool> _pools = new();
        // 꺼내 쓴 오브젝트 -> 어느 풀 소속인지 (반납할 때 찾기 위함)
        private readonly Dictionary<GameObject, ObjectPool> _owner = new();

        /// <summary>미리 풀을 만들어 두고 싶을 때(선택).</summary>
        public void Prewarm(GameObject prefab, int count)
        {
            GetOrCreatePool(prefab, count);
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var pool = GetOrCreatePool(prefab, 0);
            var go = pool.Get(position, rotation);
            _owner[go] = pool;
            return go;
        }

        public void Despawn(GameObject go)
        {
            if (_owner.TryGetValue(go, out var pool))
            {
                pool.Release(go);
                _owner.Remove(go);
            }
            else
            {
                // 풀 소속이 아니면 그냥 파괴(안전장치).
                Debug.LogWarning($"[PoolManager] 풀 소속이 아닌 '{go.name}' 반납 -> Destroy");
                Destroy(go);
            }
        }

        private ObjectPool GetOrCreatePool(GameObject prefab, int prewarm)
        {
            if (!_pools.TryGetValue(prefab, out var pool))
            {
                // 풀별로 정리용 부모 오브젝트를 만들어 Hierarchy를 깔끔하게.
                var parent = new GameObject($"Pool_{prefab.name}").transform;
                parent.SetParent(transform);
                pool = new ObjectPool(prefab, prewarm, parent);
                _pools[prefab] = pool;
            }
            return pool;
        }
    }
}
