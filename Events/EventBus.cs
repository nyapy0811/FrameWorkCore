using System;
using System.Collections.Generic;

namespace Framework.Core
{
    /// <summary>
    /// 타입 기반 전역 이벤트 버스.
    /// 발행자(publisher)와 구독자(subscriber)는 서로를 전혀 모른다.
    /// 오직 "이벤트 타입 T"를 매개로만 연결된다.
    ///
    /// 사용 예:
    ///   EventBus.Subscribe<PlayerDied>(e => Debug.Log(e.Cause));
    ///   EventBus.Publish(new PlayerDied { Cause = "낙사" });
    /// </summary>
    public static class EventBus
    {
        // 이벤트 타입마다 구독자 델리게이트 목록을 보관한다.
        private static readonly Dictionary<Type, Delegate> _handlers = new();

        public static void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            var type = typeof(T);
            if (_handlers.TryGetValue(type, out var existing))
                _handlers[type] = (Action<T>)existing + handler;
            else
                _handlers[type] = handler;
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : IEvent
        {
            var type = typeof(T);
            if (!_handlers.TryGetValue(type, out var existing)) return;

            var updated = (Action<T>)existing - handler;
            if (updated == null) _handlers.Remove(type); // 마지막 구독자면 정리
            else _handlers[type] = updated;
        }

        public static void Publish<T>(T evt) where T : IEvent
        {
            if (_handlers.TryGetValue(typeof(T), out var existing))
                (existing as Action<T>)?.Invoke(evt);
        }

        /// <summary>씬 전환 등에서 모든 구독을 비우고 싶을 때.</summary>
        public static void Clear() => _handlers.Clear();
    }
}
