using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// 修正: 'currentData' フィールドの名前が重複しているため、1つのフィールド名を変更します。
// 'EventData' 型のフィールドを 'currentEventData' に変更しました。

public class Question : MonoBehaviour
{
    public Text questionText;
    public Button[] choiceButtons;

    private Action<int> onFinished;
    private EventData currentEventData; // フィールド名を変更
    //private QuestionData currentData; // こちらはそのまま

    public int Choice = 0; // 仮の選択肢変数

    private string lastLeverName = null;

    // 他のコードはそのまま
    void OnEnable()
    {
        Lever.OnLeverClicked += ReceiveLeverInfo;
    }

    void OnDisable()
    {
        Lever.OnLeverClicked -= ReceiveLeverInfo;
    }

    void ReceiveLeverInfo(string leverName)
    {
        lastLeverName = leverName;
    }

    private void Update()
    {
        if (lastLeverName != null)
        {
            Debug.Log("Questionが受け取ったレバー: " + lastLeverName);

            switch (lastLeverName)
            {
                case "Lever1":
                    Choice = 0;
                    Debug.Log("プレイヤーがAを回答しました！");
                    EndQuestion(Choice);
                    break;

                case "Lever2":
                    Choice = 1;
                    Debug.Log("プレイヤーがBを回答しました！");
                    EndQuestion(Choice);
                    break;
            }
            lastLeverName = null;
        }
    }

    public void StartQuestion(EventData data, Action<int> finishedCallback)
    {
        Debug.Log("出題");

        currentEventData = data; // 修正: フィールド名を変更
        onFinished = finishedCallback;

        Debug.Log($"Question 実行中: {data.questionText}");
        Debug.Log("プレイヤーに問題を表示中... (スペースキーで回答)");

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            var button = choiceButtons[i];
            var text = button.GetComponentInChildren<Text>();

            if (i < data.choices.Length)
            {
                text.text = data.choices[i];
                button.gameObject.SetActive(true);
            }
            else
            {
                button.gameObject.SetActive(false);
            }

            int index = i;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => EndQuestion(index));
        }

        gameObject.SetActive(true);
    }

    private void EndQuestion(int selected)
    {
        Debug.Log($"EndQuestion() 呼び出し開始。選択肢番号: {selected}");
        if (currentEventData == null) // 修正: フィールド名を変更
        {
            Debug.LogError("currentEventData が設定されていません。");
            return;
        }

        if (GameManager.Instance?.ChangePoint != null)
        {
            GameObject changeObj = GameManager.Instance.ChangePoint;
            changeObj.tag = "Rail";
            Debug.Log($"{changeObj.name} のタグを Rail に変更しました。");
        }
        else
        {
            Debug.LogWarning("GameManager.Instance.ChangePoint が設定されていません。");
        }

        gameObject.SetActive(false);
        Debug.Log("【Question】コールバックを実行します（TrolleyChoiceへ）");
        onFinished?.Invoke(Choice);
        Debug.Log("【Question】onFinished.Invoke() 完了");
    }
}
