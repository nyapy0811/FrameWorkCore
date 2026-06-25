using System;

namespace Framework.Core
{
    /// <summary>
    /// 저장되는 게임 데이터. JsonUtility로 직렬화되므로
    ///  - [Serializable] 필수
    ///  - public 필드(또는 [SerializeField])만 저장됨. 프로퍼티는 저장 안 됨.
    /// 게임에 맞게 필드를 추가해 쓴다.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public int level = 1;
        public int gold;
        public float playTime;
        public string lastScene = "SampleScene";
    }
}
