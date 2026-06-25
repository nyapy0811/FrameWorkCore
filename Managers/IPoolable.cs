namespace Framework.Core
{
    /// <summary>
    /// 풀링되는 오브젝트가 선택적으로 구현하는 인터페이스.
    /// 꺼내질 때(OnSpawn)와 반납될 때(OnDespawn) 상태를 초기화/정리할 수 있다.
    /// 예: 총알이 꺼질 때 속도를 0으로, 켜질 때 수명 타이머 리셋.
    /// </summary>
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}
