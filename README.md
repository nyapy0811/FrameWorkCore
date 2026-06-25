# Framework Core

Unity 6 범용 게임 프레임워크. 네임스페이스 `Framework.Core`, 어셈블리 `Framework.Core`.

## 설치 (Git UPM)

게임 프로젝트의 `Packages/manifest.json` 의 `dependencies` 에 추가:

```json
"com.thoth.framework-core": "https://github.com/<유저명>/framework-core.git#v1.0.0"
```

또는 Unity `Window > Package Manager > + > Add package from git URL` 에
`https://github.com/<유저명>/framework-core.git#v1.0.0` 입력.

## 포함 시스템

- `MonoSingleton<T>` — 싱글톤 베이스
- `GameManager` — 게임 상태(Boot/MainMenu/Playing/Paused)
- `EventBus` / `IEvent` — 전역 이벤트 버스
- `SceneLoader` — 비동기 씬 로딩
- `AudioManager` — BGM/SFX 재생
- `PoolManager` / `ObjectPool` / `IPoolable` — 오브젝트 풀링
- `SaveManager` / `SaveData` — JSON 저장/로드

코드에서 사용:

```csharp
using Framework.Core;
```

## 버전 업데이트

코드 수정 → 커밋 → `git tag vX.Y.Z` → push.
사용하는 프로젝트는 manifest의 `#vX.Y.Z` 태그를 바꿔 원하는 버전을 받는다.
