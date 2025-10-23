using UnityEngine;
using System;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    public Text questionText;
    public Button[] choiceButtons;

    private Action onFinished;
    private QuestionData currentData;

    // CSVで読み込んだ問題を外部から渡して使う
    public void StartQuestion(QuestionData data, Action finishedCallback)
    {
        //if (data == null)
        //{
        //    Debug.LogError("QuestionData が null です。CSV 読み込みを確認してください。");
        //    return;
        //}



        currentData = data;
        onFinished = finishedCallback;

        Debug.Log($"Question 実行中: {data.questionText}");

        // UI表示
        questionText.text = data.questionText;

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
            button.onClick.AddListener(() => OnChoiceSelected(index));
        }

        gameObject.SetActive(true);
    }

    private void OnChoiceSelected(int selected)
    {
        if (currentData == null)
        {
            Debug.LogError("currentData が設定されていません。");
            return;
        }

        bool isCorrect = (selected == currentData.correctIndex);
        Debug.Log(isCorrect ? "正解！" : "不正解...");

        // Change タグを Rail に戻す
        if (GameManager.Instance?.ChangePoint != null)
        {
            GameObject changeObj = GameManager.Instance.ChangePoint;
            changeObj.tag = "Rail";
            Debug.Log($"{changeObj.name} のタグを Rail に変更しました。");
        }

        // UIを非表示にして終了通知
        gameObject.SetActive(false);
        onFinished?.Invoke();
    }


}
