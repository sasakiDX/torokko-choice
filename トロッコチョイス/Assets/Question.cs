using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Question : MonoBehaviour
{
    public Text questionText;
    public Button[] choiceButtons;

    private Action<int> onFinished;
    private QuestionData currentData;

   public int Choice = 0;//仮の選択肢変数


    private string lastLeverName = null;

    void OnEnable()
    {
        // ClickLeverイベント登録
        Lever.OnLeverClicked += ReceiveLeverInfo;
    }

    void OnDisable()
    {
        // 登録解除（メモリリーク防止）
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

            // クリックされたレバーごとの分岐処理
            switch (lastLeverName)
            {
                case "Lever1":
                    Choice = 0;
                    Debug.Log("プレイヤーがAを回答しました！");
                    EndQuestion(currentData.correctIndex);
                    break;


                case "Lever2":
                    Choice = 1;
                    Debug.Log("プレイヤーがBを回答しました！");
                    EndQuestion(currentData.correctIndex);
                    break;
            }

            // 処理後にリセット
            lastLeverName = null;
        }


        //if (onFinished != null && Input.GetKeyDown(KeyCode.Space))//青(そのまま)
        //{
        //    Debug.Log("プレイヤーがAを回答しました！");
        //    EndQuestion(currentData.correctIndex);
        //}
        //
        //if (onFinished != null && Input.GetKeyDown(KeyCode.RightArrow))//赤(レール変更)
        //{
        //    Choice = 1;
        //    Debug.Log("プレイヤーがBを回答しました！");
        //    EndQuestion(currentData.correctIndex);
        //
        //}

       
       

    }




    // CSVで読み込んだ問題を外部から渡して使う
    public void StartQuestion(QuestionData data, Action<int> finishedCallback)
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

        for (int i = 0; i < choiceButtons.Length; i++)// 選択肢の設定
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
            button.onClick.RemoveAllListeners();//
            button.onClick.AddListener(() => EndQuestion(index));// 選択肢がクリックされたときに EndQuestion を呼び出す
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
        onFinished?.Invoke(Choice);// ←ここで「Question終了後、Moveに戻る」が出力される
        Debug.Log("【Question】onFinished.Invoke() 完了"); // 呼び出し完了後に追加

    }

  
}
