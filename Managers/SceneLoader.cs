using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.Core
{
    /// <summary>
    /// 비동기 씬 로딩 매니저 (UniTask 기반).
    /// 화면 끊김 없이 씬을 바꾸고, 진행률을 이벤트로 알린다.
    /// 사용:
    ///   SceneLoader.Instance.Load("GameScene");           // 발사 후 잊기
    ///   await SceneLoader.Instance.LoadAsync("GameScene"); // 완료까지 대기
    /// </summary>
    public class SceneLoader : MonoSingleton<SceneLoader>
    {
        public bool IsLoading { get; private set; }

        /// <summary>완료를 기다리지 않는 호출. 내부적으로 LoadAsync를 실행한다.</summary>
        public void Load(string sceneName) => LoadAsync(sceneName).Forget();

        /// <summary>씬 로딩을 await 할 수 있는 버전.</summary>
        public async UniTask LoadAsync(string sceneName)
        {
            if (IsLoading)
            {
                Debug.LogWarning($"[SceneLoader] 이미 로딩 중이라 '{sceneName}' 요청 무시.");
                return;
            }

            IsLoading = true;
            EventBus.Publish(new SceneLoadStarted { SceneName = sceneName });

            var op = SceneManager.LoadSceneAsync(sceneName);
            // 90%까지 로드되면 자동 전환을 막고, 우리가 직접 마무리한다.
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                // 진행률은 0~0.9 구간이라 0~1로 환산해서 알린다.
                EventBus.Publish(new SceneLoadProgress { Progress = op.progress / 0.9f });
                await UniTask.Yield();
            }

            EventBus.Publish(new SceneLoadProgress { Progress = 1f });

            // 여기서 로딩 화면을 잠깐 보여주는 등 연출 가능.
            op.allowSceneActivation = true;
            await op; // 실제 씬 전환 완료까지 대기

            IsLoading = false;
            EventBus.Publish(new SceneLoadCompleted { SceneName = sceneName });
        }
    }
}
