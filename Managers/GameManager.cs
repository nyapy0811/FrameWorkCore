using UnityEngine;

namespace Framework.Core
{
    public enum GameState
    {
        Boot,       // 초기화 중
        MainMenu,   // 메뉴
        Playing,    // 게임 진행
        Paused      // 일시정지
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
    }
}
