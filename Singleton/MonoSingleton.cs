using UnityEngine;

namespace Framework.Core
{
    /// <summary>
    /// MonoBehaviour 기반 싱글톤 베이스.
    /// 상속해서 쓰면 어디서나 T.Instance 로 접근 가능하다.
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;
        private static bool _isQuitting;

        public static T Instance
        {
            get
            {
                if (_isQuitting) return null;

                if (_instance == null)
                {
                    // 씬에 이미 배치돼 있으면 찾아온다.
                    _instance = FindAnyObjectByType<T>();

                    // 없으면 새 GameObject를 만들어 자동 생성한다.
                    if (_instance == null)
                    {
                        var go = new GameObject(typeof(T).Name);
                        _instance = go.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            // 중복 인스턴스 방지: 이미 있으면 자신을 제거한다.
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = (T)this;
            transform.SetParent(null);          // DontDestroyOnLoad는 루트 오브젝트만 가능
            DontDestroyOnLoad(gameObject);       // 씬이 바뀌어도 유지
            OnAwake();
        }

        /// <summary>Awake 대신 오버라이드할 초기화 훅.</summary>
        protected virtual void OnAwake() { }

        protected virtual void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }
    }
}
