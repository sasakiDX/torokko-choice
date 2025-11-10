using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class EventLoader : MonoBehaviour
{
    public List<EventData> eventList = new List<EventData>();

    void Awake()
    {
        LoadCSV();
    }

    void LoadCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Events");
        if (csvFile == null)
        {
            Debug.LogError("CSV が Resources にありません: Events.csv");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        // 1行目はヘッダー → i=1 から開始
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] cols = line.Split('\t');

            EventData data = new EventData();
            data.Age = cols[1];
            data.Question = cols[2];

            // Choice1～4 をループで処理
            for (int c = 0; c < 4; c++)
            {
                int baseIndex = 3 + c * 3; // Choice, Result, Cond が 3列ずつ

                if (baseIndex >= cols.Length) break;
                if (string.IsNullOrWhiteSpace(cols[baseIndex])) continue;

                ChoiceData choice = new ChoiceData();
                choice.Text = cols[baseIndex];
                choice.Result = (baseIndex + 1 < cols.Length) ? cols[baseIndex + 1] : "";
                choice.Condition = (baseIndex + 2 < cols.Length) ? cols[baseIndex + 2] : "";

                data.Choices.Add(choice);
            }

            eventList.Add(data);
        }

        Debug.Log($"イベント読み込み完了：{eventList.Count} 件");
    }
}