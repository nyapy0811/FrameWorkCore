namespace Framework.Core
{
    /// <summary>씬 로딩이 시작될 때 발행.</summary>
    public struct SceneLoadStarted : IEvent
    {
        public string SceneName;
    }

    /// <summary>로딩 진행률이 갱신될 때 발행. (0~1)</summary>
    public struct SceneLoadProgress : IEvent
    {
        public float Progress;
    }

    /// <summary>씬 로딩이 끝났을 때 발행.</summary>
    public struct SceneLoadCompleted : IEvent
    {
        public string SceneName;
    }
}
