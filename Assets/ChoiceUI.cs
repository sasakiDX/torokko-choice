using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceUI : MonoBehaviour
{
    [Header("UI参照")]
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI choiceAText;
    public TextMeshProUGUI choiceBText;
    public Button decideButton;  // ← さっきのボタンを使う

    private int selected = 0; // 0 = 未選択, 1 = A, 2 = B

    void Start()
    {
        // ボタンが押された時の処理を登録
        decideButton.onClick.AddListener(OnDecide);
    }

    // 質問と選択肢を表示する
    public void ShowQuestion(string question, string choiceA, string choiceB)
    {
        questionText.text = question;
        choiceAText.text = "A: " + choiceA;
        choiceBText.text = "B: " + choiceB;

        selected = 0;
        Highlight();
    }

    // Aを選択
    public void SelectA()
    {
        selected = 1;
        Highlight();
    }

    // Bを選択
    public void SelectB()
    {
        selected = 2;
        Highlight();
    }

    // どちらが選ばれているか色で強調表示
    void Highlight()
    {
        choiceAText.color = (selected == 1) ? Color.yellow : Color.white;
        choiceBText.color = (selected == 2) ? Color.yellow : Color.white;
    }

    // 決定ボタンが押された時
    void OnDecide()
    {
        if (selected == 1)
        {
            Debug.Log("Aを選んだ！");
            // ここにAの処理
        }
        else if (selected == 2)
        {
            Debug.Log("Bを選んだ！");
            // ここにBの処理
        }
        else
        {
            Debug.Log("まだ選ばれていません");
        }
    }
}