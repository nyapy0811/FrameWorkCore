using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    /// <summary>
    /// 프리팹 하나를 위한 풀. 비활성 오브젝트를 스택에 쌓아두고 재사용한다.
    /// 보통 PoolManager가 내부에서 생성하므로 직접 만들 일은 드물다.
    /// </summary>
    public class ObjectPool
    {
        private readonly GameObject _prefab;
        private readonly Transform _parent;          // 풀 오브젝트를 담아둘 부모
        private readonly Stack<GameObject> _idle = new(); // 쉬고 있는 오브젝트들

        public ObjectPool(GameObject prefab, int prewarm, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
            for (int i = 0; i < prewarm; i++)
                _idle.Push(CreateNew());
        }

        private GameObject CreateNew()
        {
            var go = Object.Instantiate(_prefab, _parent);
            go.SetActive(false);
            return go;
        }

        /// <summary>풀에서 하나 꺼낸다. 비어 있으면 새로 만든다.</summary>
        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            var go = _idle.Count > 0 ? _idle.Pop() : CreateNew();
            go.transform.SetPositionAndRotation(position, rotation);
            go.SetActive(true);

            if (go.TryGetComponent<IPoolable>(out var p)) p.OnSpawn();
            return go;
        }

        /// <summary>다 쓴 오브젝트를 풀에 되돌린다.</summary>
        public void Release(GameObject go)
        {
            if (go.TryGetComponent<IPoolable>(out var p)) p.OnDespawn();
            go.SetActive(false);
            _idle.Push(go);
        }
    }
}
