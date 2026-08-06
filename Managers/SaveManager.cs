using System.IO;
using UnityEngine;

namespace Framework.Core
{
    /// <summary>
    /// JSON 기반 저장/로드 매니저.
    /// 데이터는 Application.persistentDataPath 아래 파일로 저장된다.
    ///  - Windows: C:/Users/<유저>/AppData/LocalLow/<회사>/<게임>/
    ///
    /// 두 가지 방식으로 쓸 수 있다.
    ///  1) 진행 저장 전용 API: Current / Save() / Load() / HasSave() / Delete()
    ///     └ 항상 "save.json"에 SaveData를 다룬다.
    ///  2) 범용 JSON API: SaveJson/LoadJson/HasJson/DeleteJson
    ///     └ 임의의 파일명으로 아무 [Serializable] 타입이나 저장한다. (예: "settings.json")
    /// </summary>
    public class SaveManager : MonoSingleton<SaveManager>
    {
        private const string FileName = "save.json";

        /// <summary>현재 메모리에 올라온 저장 데이터. 게임은 이걸 읽고 쓴다.</summary>
        public SaveData Current { get; private set; } = new SaveData();

        private string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        protected override void OnAwake()
        {
            Load(); // 시작 시 자동 로드
        }

        // ── 진행 저장 전용 API (기존 동작 그대로 유지) ─────────────────────

        public void Save()
        {
            SaveJson(FileName, Current);
            Debug.Log($"[SaveManager] 저장 완료 -> {FilePath}");
        }

        public void Load()
        {
            if (!HasJson(FileName))
            {
                Debug.Log("[SaveManager] 저장 파일 없음. 새 데이터로 시작.");
                Current = new SaveData();
                return;
            }

            Current = LoadJson<SaveData>(FileName);
            Debug.Log($"[SaveManager] 로드 완료 (gold={Current.gold}, level={Current.level})");
        }

        public bool HasSave() => HasJson(FileName);

        public void Delete()
        {
            DeleteJson(FileName);
            Current = new SaveData();
            Debug.Log("[SaveManager] 저장 데이터 삭제.");
        }

        // ── 범용 JSON API (임의 파일명 / 임의 타입) ────────────────────────

        /// <summary>data를 fileName(persistentDataPath 아래)에 JSON으로 저장.</summary>
        public void SaveJson<T>(string fileName, T data)
        {
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(PathFor(fileName), json);
        }

        /// <summary>
        /// fileName에서 T를 로드. 파일이 없거나 파싱 실패 시 new T()를 반환한다.
        /// </summary>
        public T LoadJson<T>(string fileName) where T : new()
        {
            var path = PathFor(fileName);
            if (!File.Exists(path)) return new T();

            var json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json) ?? new T();
        }

        public bool HasJson(string fileName) => File.Exists(PathFor(fileName));

        public void DeleteJson(string fileName)
        {
            var path = PathFor(fileName);
            if (File.Exists(path)) File.Delete(path);
        }

        private static string PathFor(string fileName)
            => Path.Combine(Application.persistentDataPath, fileName);
    }
}
