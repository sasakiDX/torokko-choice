
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionData", menuName = "Questions/QuestionData", order = 0)]
public class QuestionData : ScriptableObject
{
    public int id;

    [TextArea(2, 6)]
    public string questionText;

    // 2択を想定（レバー1 / レバー2）
    public string[] choices = new string[2];

    // 正解インデックス（必要なければ使わなくてもOK）
    public int correctIndex = 0;

    [Header("スコア設定（選択肢ごと）")]
    [Tooltip("choices[0]（レバー1）を選んだ場合に加算するスコア")]
    public int scoreWhenChoose0 = 100;

    [Tooltip("choices[1]（レバー2）を選んだ場合に加算するスコア")]
    public int scoreWhenChoose1 = 50;
}
