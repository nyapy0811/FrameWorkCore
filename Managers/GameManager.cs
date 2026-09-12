using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    public abstract class GameManagerBase<TSelf, TState> : MonoSingleton<TSelf>
        where TSelf : GameManagerBase<TSelf, TState>
        where TState : struct, Enum
    {
        public TState State { get; private set; }
        public event Action<TState, TState> OnStateChanged;

        public void ChangeState(TState newState)
        {
            if (EqualityComparer<TState>.Default.Equals(State, newState)) return;
            var previous = State;
            State = newState;
            Debug.Log($"[GameManager] {previous} -> {newState}");
            OnStateChanged?.Invoke(previous, newState);
        }
    }
}
