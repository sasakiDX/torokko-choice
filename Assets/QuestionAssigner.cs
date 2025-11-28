using UnityEngine;

public class QuestionAssigner : MonoBehaviour
{
    [Tooltip("CSV内の問題番号(ID)を指定します")]
    public int questionID = 0; // 例: 1ならCSVの1番の問題を使用
}