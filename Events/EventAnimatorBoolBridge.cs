using UnityEngine;

namespace Framework.Core
{
    /// <summary>
    /// TEvent 이벤트가 올 때마다 이벤트 안의 bool 값을 Animator Bool 파라미터에 반영한다.
    /// "지금 이동 중인가" 처럼 현재 상태를 나타내는 이벤트용.
    /// 프로젝트 쪽에서 구체 클래스를 만들고 GetValue로 값을 꺼내는 방법을 정의한다.
    ///   public class MovingBridge : EventAnimatorBoolBridge&lt;PlayerMoveStateChanged&gt;
    ///   {
    ///       protected override bool GetValue(PlayerMoveStateChanged e) =&gt; e.IsMoving;
    ///   }
    /// </summary>
    public abstract class EventAnimatorBoolBridge<TEvent> : MonoBehaviour where TEvent : IEvent
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string parameterName;

        private void OnEnable()
        {
            if (animator == null)
                Debug.LogWarning($"[{GetType().Name}] Animator가 지정되지 않았습니다.", this);
            EventBus.Subscribe<TEvent>(OnEvent);
        }

        private void OnDisable() => EventBus.Unsubscribe<TEvent>(OnEvent);

        private void OnEvent(TEvent e)
        {
            if (animator == null || string.IsNullOrEmpty(parameterName)) return;
            animator.SetBool(parameterName, GetValue(e));
        }

        /// <summary>이벤트에서 bool 값을 꺼내는 방법. 프로젝트 쪽 구체 클래스가 구현.</summary>
        protected abstract bool GetValue(TEvent e);
    }
}
