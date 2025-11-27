using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance;
    [Tooltip("問題データが含まれるCSVファイルを指定します")]
    public TextAsset csvFile;
    private List<QuestionData> questions = new List<QuestionData>();

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
        questions.Clear();
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
            if (values.Length < 6) continue;

            QuestionData q = new QuestionData();
            q.questionText = values[1];
            q.choices = new string[] { values[2], values[3], values[4] };
            int.TryParse(values[5], out q.correctIndex);
            questions.Add(q);
        }

        Debug.Log($"問題数: {questions.Count}");
    }

    public QuestionData GetQuestion(int id)
    {
        if (id >= 0 && id < questions.Count)
            return questions[id];
        return null;
    }
}