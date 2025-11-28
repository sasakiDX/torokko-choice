using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusUI : MonoBehaviour
{
    [Header("UI参照")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI statusText;
    public Slider intelGauge;
    public Slider senseGauge;
    public Slider staminaGauge;

    [Header("ランク表示用")]
    public TextMeshProUGUI intelRankText;
    public TextMeshProUGUI senseRankText;
    public TextMeshProUGUI staminaRankText;

    int money = 0;

    float intel = 500;
    float sense = 230;
    float stamina = 10;

    float age = 1;
    int job;
    int name;

    // ランク
    string[] ranks = { "F", "E", "D", "C", "B", "A", "S" };
    int intelRank = 20;
    int senseRank = 200;
    int staminaRank = 0;

    float maxGauge = 100f;

    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        moneyText.text = "所持金:" + money + "円";

        intelGauge.value = intel;
        senseGauge.value = sense;
        staminaGauge.value = stamina;

        statusText.text = age + "歳/" + job + "/" + name;

        intelRankText.text = "知力：" + ranks[intelRank];
        senseRankText.text = "センス：" + ranks[senseRank];
        staminaRankText.text = "体力：" + ranks[staminaRank];
    }

    // ★ 数値を加算 → ゲージ増加 → ランクチェック
    public void AddIntel(float amount)
    {
        intel += amount;
        CheckRank(ref intel, ref intelRank);
        UpdateUI();
    }

    public void AddSense(float amount)
    {
        sense += amount;
        CheckRank(ref sense, ref senseRank);
        UpdateUI();
    }

    public void AddStamina(float amount)
    {
        stamina += amount;
        CheckRank(ref stamina, ref staminaRank);
        UpdateUI();
    }

    // ★ ランクアップを判定
    void CheckRank(ref float value, ref int rank)
    {
        if (value >= maxGauge)
        {
            value -= maxGauge;   // ← 余りを持ち越す（リセットしたい場合は「value = 0」に変更）
            rank = Mathf.Min(rank + 1, ranks.Length - 1);
        }
    }

    // 他の既存関数
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    public void Addage(int years)
    {
        age += years;
        UpdateUI();
    }

    public void SetJob(int jobId)
    {
        job = jobId;
        UpdateUI();
    }
}
