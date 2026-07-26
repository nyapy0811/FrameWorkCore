using System;
using System.Collections.Generic;

namespace Framework.Core
{
    /// <summary>
    /// 저장되는 게임 데이터. JsonUtility로 직렬화되므로
    ///  - [Serializable] 필수
    ///  - public 필드(또는 [SerializeField])만 저장됨. 프로퍼티는 저장 안 됨.
    ///  - List는 지원되지만 Dictionary는 안 됨.
    /// 게임에 맞게 필드를 추가해 쓴다.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public int level = 1;
        public int gold;
        public float playTime;
        public string lastScene = "SampleScene";

        /// <summary>해금된 챕터 수. 최소 1개 챕터는 항상 열려 있다.</summary>
        public int unlockedChapterCount = 1;

        /// <summary>클리어한 스테이지 ID 목록. 중복 없이 관리한다(아래 헬퍼 사용).</summary>
        public List<string> clearedStages = new();

        /// <summary>해당 스테이지를 클리어했는지 확인.</summary>
        public bool IsStageCleared(string stageId) => clearedStages.Contains(stageId);

        /// <summary>스테이지를 클리어 처리. 이미 있으면 무시(중복 방지).</summary>
        public void MarkStageCleared(string stageId)
        {
            if (!clearedStages.Contains(stageId))
                clearedStages.Add(stageId);
        }
    }
}
