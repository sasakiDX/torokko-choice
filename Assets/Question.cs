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
    //private EventData currentEventData; // フィールド名を変更
    private QuestionData currentData; // こちらはそのまま

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
                    Choice = 1;
                    Debug.Log("プレイヤーがAを回答しました！");
                    EndQuestion(Choice);
                    break;

                case "Lever2":
                    Choice = 2;
                    Debug.Log("プレイヤーがBを回答しました！");
                    EndQuestion(Choice);
                    break;
            }
            lastLeverName = null;
        }
    }

    public void StartQuestion(QuestionData data, Action<int> finishedCallback)
    {
        Debug.Log("出題");

        currentData = data;
        onFinished += finishedCallback;

        Debug.Log($"Question 実行中: {data.questionText}");

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
        if (currentData == null)
        {
            Debug.LogError("currentEventData が設定されていません。");
            return;
        }

        //選択肢に応じたスコアを加算
        int scoreDelta = 0;
        int moneyDelta = 0;
        int iqDelta = 0;
        int staminaDelta = 0;
        int senseDelta = 0;

        if (selected == 1)
        {
            scoreDelta = currentData.scoreWhenChoose1;
            moneyDelta = currentData.moneyReward1;
            iqDelta = currentData.iqReward1;
            staminaDelta = currentData.staminaReward1;
            senseDelta = currentData.senseReward1;

        }
        else if (selected == 2)
        {
            scoreDelta = currentData.scoreWhenChoose2;
            moneyDelta = currentData.moneyReward2;
            iqDelta = currentData.iqReward2;
            staminaDelta= currentData.staminaReward2;
            senseDelta = currentData.senseReward2;
        }

        //スコア加算しRunData到達でloopDisabled自動ON
        if (RunData.Instance != null)
        {
            RunData.Instance.AddScore(scoreDelta);
            RunData.Instance.AddMoney(moneyDelta);
            RunData.Instance.AddIQ(iqDelta);
            RunData.Instance.AddStamina(staminaDelta);
            RunData.Instance.AddSense(senseDelta);

        }
        else
        {
            Debug.LogWarning("RunData.Instance が見つかりません。");
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

        //選んだindexをメインへ渡す
        onFinished?.Invoke(selected);

        Debug.Log("【Question】onFinished.Invoke() 完了");
    }
}
