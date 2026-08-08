using UnityEngine;

namespace Framework.Core
{
    /// <summary>
    /// TEvent 이벤트가 발행되면 Animator의 Trigger 파라미터를 발동한다.
    /// 제네릭 MonoBehaviour는 인스펙터에 직접 못 붙이므로,
    /// 프로젝트 쪽에서 한 줄짜리 구체 클래스를 만들어 쓴다.
    ///   public class JumpAnimatorTrigger : EventAnimatorTrigger&lt;PlayerJumped&gt; { }
    /// </summary>
    public abstract class EventAnimatorTrigger<TEvent> : MonoBehaviour where TEvent : IEvent
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
            animator.SetTrigger(parameterName);
        }
    }
}
