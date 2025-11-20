using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance;
    public TextAsset csvFile; // インスペクタでCSVを指定

    // 変更前（保持）
    // private List<QuestionData> questions = new List<QuestionData>();
    // private List<ChoiceData> choiseDatas = new List<ChoiceData>();

    // 変更後（EventData）
    private List<EventData> events = new List<EventData>(); // ★イベントデータ格納用
    private List<ChoiceData> choiseDatas = new List<ChoiceData>(); // 変更前の名残を保持

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        LoadQuestionsFromCSV();
    }

    void LoadQuestionsFromCSV()
    {
        // 変更前（保持）
        // questions.Clear();
        // choiseDatas.Clear();

        // 変更後
        events.Clear();
        choiseDatas.Clear();

        if (csvFile == null)
        {
            Debug.LogError("CSVファイルが指定されていません。");
            return;
        }

        StringReader reader = new StringReader(csvFile.text);
        bool headerSkipped = false;

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            if (!headerSkipped) { headerSkipped = true; continue; } // 1行目はヘッダー

            string[] values = line.Split(',');

            // EventData は最低8列必要
            if (values.Length < 8) continue;

            // ★★★ 変更後：EventData 作成 ★★★
            EventData data = new EventData();
            data.ID = int.Parse(values[0]);
            data.Question = values[1];

            // Choice1
            ChoiceData c1 = new ChoiceData();
            c1.Text = values[2];
            c1.Result = values[3];
            c1.Condition = values[4];
            data.Choices.Add(c1);

            // Choice2
            ChoiceData c2 = new ChoiceData();
            c2.Text = values[5];
            c2.Result = values[6];
            c2.Condition = values[7];
            data.Choices.Add(c2);

            // リストに追加
            events.Add(data);
        }

        // 変更前（保持）
        // Debug.Log($"問題数: {questions.Count}");

        // 変更後
        Debug.Log($"イベント数: {events.Count}");
    }

    // 変更前の関数保持
    /*
    public QuestionData GetQuestion(int id)
    {
        if (id >= 0 && id < questions.Count)
            return questions[id];
        return null;
    }
    */

    // 変更後
    public EventData GetEvent(int id)
    {
        if (id >= 0 && id < events.Count)
            return events[id];
        return null;
    }
}