using UnityEngine;
using TMPro;

/// <summary>
/// RunData の各種値を Canvas 上の TextMeshProUGUI に表示するマネージャ（2D用）
/// RunData は DontDestroyOnLoad を想定。軽量ポーリングで定期更新します。
/// </summary>
public class RunDataLifeManagerTMP : MonoBehaviour
{
    [Header("表示先 (Canvas上の TextMeshProUGUI)")]
    public TextMeshProUGUI targetTMP;

    [Header("表示テンプレート")]
    [TextArea(3, 10)]
    [Tooltip("差し込み可能: {score} {money} {iq} {stamina} {sense}")]
    public string format =
        "Score : {score}\n" +
        "Money : {money}\n" +
        "IQ    : {iq}\n" +
        "Stamina: {stamina}\n" +
        "Sense : {sense}";

    [Header("更新間隔（秒）")]
    [Range(0.02f, 2f)]
    public float updateInterval = 0.2f;

    [Header("RunDataが未生成の間に表示する文言（空なら非表示）")]
    public string fallbackWhenNoRunData = "(RunData not found)";

    private void Awake()
    {
       
    }
    private void OnEnable()
    {
        CancelInvoke(nameof(Refresh));// 定期更新開始
        InvokeRepeating(nameof(Refresh), 0f, updateInterval);// 定期更新開始のインターバル
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Refresh));// 定期更新停止
    }
    private void Refresh()// 定期更新の内容
    {
        if (targetTMP == null) return;

        var rd = RunData.Instance;
        if (rd == null)
        {
            targetTMP.text = string.IsNullOrEmpty(fallbackWhenNoRunData) ? string.Empty : fallbackWhenNoRunData;
            return;
        }

        // 表示する内容
        targetTMP.text = format
            .Replace("{score}", rd.score.ToString())
            .Replace("{money}", rd.moneyReward.ToString())
            .Replace("{iq}", rd.iqReward.ToString())
            .Replace("{stamina}", rd.staminaReward.ToString())
            .Replace("{sense}", rd.senseReward.ToString());
    }
}
