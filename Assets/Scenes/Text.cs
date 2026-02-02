// ShowQuestionOnTouch2D_TMP.cs
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class ShowQuestionOnTouch2D_TMP : MonoBehaviour
{
    [Header("表示先: TextMeshProUGUI")]
    public TextMeshProUGUI targetTMP; // Canvas上の TMPテキストを割り当て

    [Header("表示テンプレート")]
    [TextArea(2, 4)]
    public string format = "ID: {id}\n{text}\nA) {a}\nB) {b}";

    [Header("接触判定")]
    public string playerTag = "Player";
    public bool clearOnExit = false;
    public float autoHideSeconds = 0f;

    private QuestionAssigner _assigner;

    private void Awake()
    {
        _assigner = GetComponent<QuestionAssigner>();
        if (_assigner == null)
        {
            Debug.LogWarning($"[{nameof(ShowQuestionOnTouch2D_TMP)}] QuestionAssigner が見つかりません（{name}）。");
        }
        if (targetTMP != null) targetTMP.text = string.Empty;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (_assigner == null) return;

        var qm = QuestionManager.Instance;
        if (qm == null)
        {
            Debug.LogError($"[{nameof(ShowQuestionOnTouch2D_TMP)}] QuestionManager.Instance が見つかりません。");
            return;
        }

        var data = qm.GetQuestion(_assigner.questionID);
        if (data == null)
        {
            Debug.LogError($"[{nameof(ShowQuestionOnTouch2D_TMP)}] ID {_assigner.questionID} の QuestionData が見つかりません。");
            return;
        }




        if (targetTMP != null)
        {
            string a = (data.choices != null && data.choices.Length > 0) ? data.choices[0] : "";
            string b = (data.choices != null && data.choices.Length > 1) ? data.choices[1] : "";
            targetTMP.text = format
                .Replace("{id}", data.id.ToString())
                .Replace("{text}", data.questionText)
                .Replace("{a}", a)
                .Replace("{b}", b);

            if (autoHideSeconds > 0f)
            {
                CancelInvoke(nameof(Hide));
                Invoke(nameof(Hide), autoHideSeconds);
            }
        }
        else
        {
            Debug.LogWarning($"[{nameof(ShowQuestionOnTouch2D_TMP)}] targetTMP が未設定です。Canvas上の TextMeshProUGUI を割り当ててください。");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (clearOnExit) Hide();
    }

    private void Hide()
    {
        if (targetTMP != null) targetTMP.text = string.Empty;
    }
}