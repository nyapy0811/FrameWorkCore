using UnityEngine;

namespace Framework.Core
{
    public enum GameState
    {
        Boot,       // 초기화 중
        MainMenu,   // 메뉴
        Loading,    // 스테이지 이동/메뉴 복귀 등 전환 대기(딜레이)
        Playing,    // 게임 진행
        Paused,     // 일시정지
        Cleared,    // 스테이지 클리어
        Quitting    // 종료 전 저장 등 마무리 작업
    }

    /// <summary>
    /// 게임 전역 상태를 관리하는 최상위 매니저.
    /// MonoSingleton을 상속하므로 GameManager.Instance 로 접근한다.
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        public GameState State { get; private set; } = GameState.Boot;

        /// <summary>상태가 바뀔 때 호출된다. (이전 상태, 새 상태)</summary>
        public event System.Action<GameState, GameState> OnStateChanged;

        protected override void OnAwake()
        {
            // 부팅 시점 초기화 자리. 지금은 바로 메뉴로 전환.
            ChangeState(GameState.MainMenu);
        }

        public void ChangeState(GameState newState)
        {
            if (State == newState) return;

            var previous = State;
            State = newState;
            Debug.Log($"[GameManager] {previous} -> {newState}");
            OnStateChanged?.Invoke(previous, newState);
        }

        public void StartGame() => ChangeState(GameState.Playing);

        public void Pause()
        {
            if (State != GameState.Playing) return;
            Time.timeScale = 0f;
            ChangeState(GameState.Paused);
        }

        public void Resume()
        {
            if (State != GameState.Paused) return;
            Time.timeScale = 1f;
            ChangeState(GameState.Playing);
        }

        /// <summary>스테이지 클리어. 진행 중일 때만 유효.</summary>
        public void StageClear()
        {
            if (State != GameState.Playing) return;
            ChangeState(GameState.Cleared);
        }

        /// <summary>
        /// 로딩 상태로 전환. 스테이지 이동이나 메뉴 복귀 전 딜레이 부여용.
        /// 일시정지로 멈춰 있던 시간을 원복하고 넘어간다.
        /// </summary>
        public void BeginLoading()
        {
            Time.timeScale = 1f;
            ChangeState(GameState.Loading);
        }

        /// <summary>
        /// 종료 절차. Quitting 상태로 바꾼 뒤 앱을 끈다.
        /// 저장 등 마무리 작업은 OnStateChanged(Quitting)를 구독한 쪽에서 처리한다.
        /// (구독자는 ChangeState 안에서 동기적으로 실행되므로 Quit 전에 완료된다.)
        /// </summary>
        public void Quit()
        {
            ChangeState(GameState.Quitting);
            Application.Quit();
        }
    }
}
