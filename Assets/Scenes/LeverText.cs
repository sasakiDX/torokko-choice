using UnityEngine;
using TMPro;

public class LeverTextPresenterTMP : MonoBehaviour
{
    [Header("表示先 (Canvas上)")]
    public TextMeshProUGUI targetTMP;
    public bool clearOnExit = false;

    [Header("表示テンプレート")]
    [TextArea(2, 6)]
    public string format = "ID: {id}\n{text}\nA) {a}\nB) {b}";

    private void OnEnable() => Lever.OnLeverClickedGO += OnLeverClicked;
    private void OnDisable() => Lever.OnLeverClickedGO -= OnLeverClicked;

    private void OnLeverClicked(GameObject leverGO)
    {
        if (targetTMP == null)
        {
            Debug.LogWarning("[LeverTextPresenterTMP] targetTMP 未設定");
            return;
        }

        // LeverAssigner 取得
        var assigner = leverGO.GetComponent<LeverAssigner>();
        if (assigner == null)
        {
            Debug.LogWarning($"[LeverTextPresenterTMP] {leverGO.name} に LeverAssigner が見つかりません");
            return;
        }

        // LeverData 取得
        var lm = LeverManager.Instance;
        if (lm == null)
        {
            Debug.LogError("[LeverTextPresenterTMP] LeverManager.Instance が見つかりません");
            return;
        }

        var data = lm.GetLever(assigner.leverID);
        if (data == null)
        {
            Debug.LogWarning($"[LeverTextPresenterTMP] LeverData (ID:{assigner.leverID}) が見つかりません");
            return;
        }

        // 表示内容組み立て
        string a = (data.choices != null && data.choices.Length > 0) ? data.choices[0] : "";
        string b = (data.choices != null && data.choices.Length > 1) ? data.choices[1] : "";

        targetTMP.text = format
            .Replace("{id}", data.id.ToString())
            .Replace("{text}", data.LeverText)
            .Replace("{a}", a)
            .Replace("{b}", b);

        // ★★★ ここに 3 秒後クリア処理 ★★★
        CancelInvoke(nameof(Clear)); // 多重呼び出し防止
        Invoke(nameof(Clear), 2f);   // ← n秒後に必ずクリア
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (clearOnExit) Clear();
    }

    private void Clear()
    {
        if (targetTMP != null) targetTMP.text = string.Empty;
    }
}