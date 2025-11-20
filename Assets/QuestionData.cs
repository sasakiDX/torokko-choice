[System.Serializable]
public class QuestionData
{
    public int id;// 問題番号
    public string questionText;// 問題文
    public string[] choices;// 選択肢
    public int correctIndex;// 正解の選択肢インデックス
}