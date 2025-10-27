using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Question : MonoBehaviour
{
    public Text questionText;
    public Button[] choiceButtons;

    private Action onFinished;
    private QuestionData currentData;


    private void Update()
    {
        // 仮想回答処理：スペースキーで回答完了
        if (onFinished != null && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("プレイヤーが回答しました！");
           EndQuestion(currentData.correctIndex);
        }
    }




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
        Debug.Log("プレイヤーに問題を表示中... (スペースキーで回答)");

        //UI表示
        //questionText.text = data.questionText;

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
        else
        {
            Debug.LogWarning("GameManager.Instance.ChangePoint が設定されていません。");
        }


        gameObject.SetActive(false);
        Debug.Log("【Question】コールバックを実行します（TrolleyChoiceへ）");
        onFinished?.Invoke(); // ←ここで「Question終了後、Moveに戻る」が出力される
        Debug.Log("【Question】onFinished.Invoke() 完了"); // 呼び出し完了後に追加

    }

  
}
