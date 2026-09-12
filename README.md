# Framework Core

Unity 6 범용 게임 프레임워크. 네임스페이스 `Framework.Core`, 어셈블리 `Framework.Core`.

## 의존성 (필수)

이 패키지는 [UniTask](https://github.com/Cysharp/UniTask)에 의존한다. **먼저 UniTask를 설치**해야 컴파일된다.
게임 프로젝트의 `Packages/manifest.json` 에 추가:

```json
"com.cysharp.unitask": "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask"
```

## 설치 (Git UPM)

UniTask 설치 후, 게임 프로젝트의 `Packages/manifest.json` 의 `dependencies` 에 추가:

```json
"com.nyapy.framework-core": "https://github.com/nyapy0811/FrameWorkCore.git"
```

또는 Unity `Window > Package Manager > + > Add package from git URL` 에
`https://github.com/nyapy0811/FrameWorkCore.git` 입력.

특정 버전을 고정하려면 끝에 태그를 붙인다 (먼저 `git tag v1.0.0 && git push --tags` 필요):

```json
"com.nyapy.framework-core": "https://github.com/nyapy0811/FrameWorkCore.git#v1.0.0"
```

## 폴더 구조

```
FrameWorkCore/            (패키지 루트)
├── package.json
├── Framework.Core.asmdef
├── Singleton/   MonoSingleton.cs
├── Events/      IEvent.cs, EventBus.cs
└── Managers/    GameManager.cs, SceneLoader.cs, SceneEvents.cs,
                 AudioManager.cs, PoolManager.cs, ObjectPool.cs,
                 IPoolable.cs, SaveManager.cs, SaveData.cs
```

## 포함 시스템

- `MonoSingleton<T>` — 싱글톤 베이스
- `GameManagerBase<TSelf, TState>` — 게임 상태머신 베이스. 상태 enum은 프레임워크가 모르며,
  사용 프로젝트가 자기 `TState` enum과 `GameManager` 구체 클래스를 정의해서 상속한다.
- `EventBus` / `IEvent` — 전역 이벤트 버스
- `SceneLoader` / `SceneEvents` — 비동기 씬 로딩 (진행률 이벤트)
- `AudioManager` — BGM/SFX 재생
- `PoolManager` / `ObjectPool` / `IPoolable` — 오브젝트 풀링
- `SaveManager` / `SaveData` — JSON 저장/로드

모든 매니저는 싱글톤이라 `XxxManager.Instance` 로 접근한다. 코드에서 사용:

```csharp
using Framework.Core;
```

`GameManagerBase<TSelf, TState>`만 예외로, 프로젝트 쪽에서 자기 상태 enum과 구체
`GameManager` 클래스를 직접 정의해야 한다:

```csharp
public enum GameState { Boot, MainMenu, Playing, Paused }

public class GameManager : GameManagerBase<GameManager, GameState>
{
    protected override void OnAwake() => ChangeState(GameState.MainMenu);
    public void StartGame() => ChangeState(GameState.Playing);
    // Pause/Resume/StageClear/BeginLoading/Quit 등 프로젝트별 편의 메서드도 여기 정의
}
```

## 빠른 예시

```csharp
GameManager.Instance.StartGame(); // 위에서 정의한 프로젝트 쪽 GameManager
AudioManager.Instance.PlayBGM(bgmClip);
PoolManager.Instance.Spawn(bulletPrefab, pos, Quaternion.identity);
EventBus.Publish(new ScoreChanged { NewScore = 100 });
SaveManager.Instance.Current.gold += 50;
SaveManager.Instance.Save();

SceneLoader.Instance.Load("Stage2");          // 발사 후 잊기
await SceneLoader.Instance.LoadAsync("Stage2"); // 완료까지 대기 (UniTask)
```

## 버전 업데이트

코드 수정 → 커밋 → `git tag vX.Y.Z` → `git push --tags`.
사용하는 프로젝트는 manifest의 `#vX.Y.Z` 태그를 바꿔 원하는 버전을 받는다.
