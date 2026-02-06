// LeverToChoiceUIBridge.cs（旧 LeverTextPresenterTMP を置き換え）
using UnityEngine;
using TMPro;

public class LeverToChoiceUIBridge : MonoBehaviour
{
    [Header("出力先（ChoiceUI を集中管理に）")]
    [SerializeField] private ChoiceUI choiceUI;//
    // public TextMeshProUGUI targetTMP;だと違うレバーを押しても同じものを読んでしまう問題があった
    [SerializeField] private ChoiceUI.Side side = ChoiceUI.Side.Left; // このレバーが更新する側

    [Header("動作オプション")]
    [SerializeField] private bool clearOnExit = false;         // レバーから離れたら消す
    [SerializeField] private float autoClearSeconds = 2f;      // 自動クリア秒（0以下で無効）

    [Header("表示テンプレート")]
    [TextArea(2, 6)]
    [SerializeField] private string format = "ID: {id}\n{text}\nA) {a}\nB) {b}";

    private void OnEnable()
    {
        Lever.OnLeverClickedGO += OnLeverClicked;
    }

    private void OnDisable()
    {
        Lever.OnLeverClickedGO -= OnLeverClicked;
        CancelInvoke(nameof(Clear));
    }

    /// <summary>
    /// どのレバーが押されたかイベントで飛んでくる
    /// </summary>
    private void OnLeverClicked(GameObject leverGO)
    {
        // このスクリプトが付いているレバー以外の通知は無視
        if (leverGO != this.gameObject) return;

        if (!choiceUI)
        {
            Debug.LogError($"[{nameof(LeverToChoiceUIBridge)}] choiceUI 未設定", this);
            return;
        }

        // LeverAssigner 取得
        var assigner = leverGO.GetComponent<LeverAssigner>();
        if (!assigner)
        {
            Debug.LogWarning($"[{nameof(LeverToChoiceUIBridge)}] {leverGO.name} に LeverAssigner が見つかりません", this);
            return;
        }

        // LeverData 取得
        var lm = LeverManager.Instance;
        if (!lm)
        {
            Debug.LogError($"[{nameof(LeverToChoiceUIBridge)}] LeverManager.Instance が見つかりません", this);
            return;
        }

        var data = lm.GetLever(assigner.leverID);
        if (data == null)
        {
            Debug.LogWarning($"[{nameof(LeverToChoiceUIBridge)}] LeverData (ID:{assigner.leverID}) が見つかりません", this);
            return;
        }

        // 表示文字列を組み立て
        string a = (data.choices != null && data.choices.Length > 1) ? data.choices[1] : "";
        string b = (data.choices != null && data.choices.Length > 2) ? data.choices[2] : "";
        //選択肢と数字の違いが多かった 例 choices1が0になっていた(本来は1がレバー1)


        string text = format
            .Replace("{id}", data.id.ToString())
            .Replace("{text}", data.LeverText)
            .Replace("{a}", a)
            .Replace("{b}", b);

        // ★ UI への書き込みは ChoiceUI に委譲（パターンB）
        choiceUI.SetSideAnswer(side, text);

        // 自動クリア
        CancelInvoke(nameof(Clear)); // 多重防止
        if (autoClearSeconds > 0f)
            Invoke(nameof(Clear), autoClearSeconds);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (clearOnExit) Clear();
    }

    private void Clear()
    {
        if (!choiceUI) return;
        choiceUI.ClearSide(side);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!choiceUI)
        {
            // 過剰にログが出るのが嫌ならコメントアウト可
            // Debug.LogWarning($"[{nameof(LeverToChoiceUIBridge)}] choiceUI が未設定です。Canvas 上の ChoiceUI を割り当ててください。", this);
        }
    }
#endif
}
