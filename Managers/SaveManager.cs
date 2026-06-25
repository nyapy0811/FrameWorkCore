using System.IO;
using UnityEngine;

namespace Framework.Core
{
    /// <summary>
    /// JSON 기반 저장/로드 매니저.
    /// 데이터는 Application.persistentDataPath 아래 파일로 저장된다.
    ///  - Windows: C:/Users/<유저>/AppData/LocalLow/<회사>/<게임>/
    /// 사용:
    ///   SaveManager.Instance.Current.gold += 100;
    ///   SaveManager.Instance.Save();
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

        public void Save()
        {
            // prettyPrint = true 로 사람이 읽기 좋은 형태로 저장.
            var json = JsonUtility.ToJson(Current, true);
            File.WriteAllText(FilePath, json);
            Debug.Log($"[SaveManager] 저장 완료 -> {FilePath}");
        }

        public void Load()
        {
            if (!File.Exists(FilePath))
            {
                Debug.Log("[SaveManager] 저장 파일 없음. 새 데이터로 시작.");
                Current = new SaveData();
                return;
            }

            var json = File.ReadAllText(FilePath);
            Current = JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
            Debug.Log($"[SaveManager] 로드 완료 (gold={Current.gold}, level={Current.level})");
        }

        public bool HasSave() => File.Exists(FilePath);

        public void Delete()
        {
            if (File.Exists(FilePath)) File.Delete(FilePath);
            Current = new SaveData();
            Debug.Log("[SaveManager] 저장 데이터 삭제.");
        }
    }
}
