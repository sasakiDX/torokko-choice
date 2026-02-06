using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceUI : MonoBehaviour
{
    [Header("UI参照")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI choiceAText;
    [SerializeField] private TextMeshProUGUI choiceBText;
    [SerializeField] private Button decideButton;

    private int selected = 0; // 0 = 未選択, 1 = A, 2 = B




    public enum Side { Left, Right }

    public void SetLeftAnswer(string text) => choiceAText.text = text;
    public void SetRightAnswer(string text) => choiceBText.text = text;

    public void SetSideAnswer(Side side, string text)
    {
        if (side == Side.Left) SetLeftAnswer(text);
        else SetRightAnswer(text);
    }

    public void ClearLeft() => choiceAText.text = string.Empty;
    public void ClearRight() => choiceBText.text = string.Empty;

    public void ClearSide(Side side)
    {
        if (side == Side.Left) ClearLeft();
        else ClearRight();
    }

    private void Awake()
    {
        // インスペクター未設定なら「自分の子」から限定的に取得
        if (!questionText)
            questionText = transform.Find("QuestionText/TQuestion")?.GetComponent<TextMeshProUGUI>();
        if (!choiceAText)
            choiceAText = transform.Find("QuestionText/TextA/TextAanswer")?.GetComponent<TextMeshProUGUI>();
        if (!choiceBText)
            choiceBText = transform.Find("QuestionText/TextB/TextBanswer")?.GetComponent<TextMeshProUGUI>();
        if (!decideButton)
            decideButton = transform.Find("DecideButton")?.GetComponent<Button>(); // ボタン名に合わせて

        // 参照検証
        if (!questionText || !choiceAText || !choiceBText || !decideButton)
        {
            Debug.LogError("[ChoiceUI] 参照が不足しています。ヒエラルキーのパス/名前を再確認してください。", this);
        }

        // ここで左右が別インスタンスか念のためチェック
        if (choiceAText && choiceBText && choiceAText == choiceBText)
        {
            Debug.LogError("[ChoiceUI] choiceAText と choiceBText が同じオブジェクトを参照しています。割り当てを修正してください。", this);
        }
    }

    private void Start()
    {
        decideButton.onClick.AddListener(OnDecide);// ボタンにイベント登録
    }

    public void ShowQuestion(string question, string choiceA, string choiceB)
    {
        questionText.text = question;
        choiceAText.text = "A: " + choiceA;
        choiceBText.text = "B: " + choiceB;

        selected = 0;
        Highlight();
    }

    public void SelectA()
    {
        selected = 1;
        Highlight();
    }

    public void SelectB()
    {
        selected = 2;
        Highlight();
    }

    private void Highlight()
    {
        choiceAText.color = (selected == 1) ? Color.yellow : Color.white;
        choiceBText.color = (selected == 2) ? Color.yellow : Color.white;
    }

    private void OnDecide()
    {
        if (selected == 1)
        {
            Debug.Log("Aを選んだ！");
            // Aの処理
        }
        else if (selected == 2)
        {
            Debug.Log("Bを選んだ！");
            // Bの処理
        }
        else
        {
            Debug.Log("まだ選ばれていません");
        }
    }
}
