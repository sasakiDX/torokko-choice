// Assets/Scripts/System/RunData.cs
using UnityEngine;

public class RunData : MonoBehaviour
{
    public static RunData Instance { get; private set; }

    public int lastQuestionId = -1; // 直前ID。初期は -1

    [Header("周回しても引き継ぎたいデータ")]
    public int score = 0;
    public int moneyReward = 0;
    public int iqReward = 0;
    public int staminaReward = 0;
    public int senseReward = 0;

    [Header("loop 無効化設定")]
    [Tooltip("このスコア以上になったらループを無効化してリザルトへ進める")]
    public int disableLoopAtScore = 500;
    public int disableLoopAtMoney = 1000;
    public int disableLoopAtIQ = 300;
    public int disableLoopAtStamina = 50;
    public int disableLoopAtSense = 200;

    [Tooltip("true になったら loop をやめる")]
    public bool loopDisabled = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject); // これでシーン再読み込みでも残る
    }

    public void ResetForNewGame()
    {
        score = 0;
        moneyReward = 0;
        iqReward = 0;
        staminaReward = 0;
        senseReward = 0;

        loopDisabled = false;
    }

    public void AddScore(int delta)
    {
        score += delta;
        if (!loopDisabled && score >= disableLoopAtScore)
        {
            loopDisabled = true;
            Debug.Log($"[RunData] 目標スコア {disableLoopAtScore} 到達。以後 loop を無効化します。");
        }
        // UI 更新が必要ならここでイベント発火などに拡張可能
        // OnScoreChanged?.Invoke(score);
    }

    public void AddMoney(int moneyDelta)
    {
        moneyReward += moneyDelta;
        if (!loopDisabled && moneyReward >= disableLoopAtMoney)
        {
            loopDisabled = true;
            Debug.Log($"[RunData] 目標スコア {disableLoopAtMoney} 到達。以後 loop を無効化します。");
        }
    }

    public void AddIQ(int iqDelta)
    {
        iqReward += iqDelta;
        if (!loopDisabled && iqReward >= disableLoopAtIQ)
        {
            loopDisabled = true;
            Debug.Log($"[RunData] 目標スコア {disableLoopAtIQ} 到達。以後 loop を無効化します。");
        }
    }

    public void AddStamina(int staminaDelta)
    {
        staminaReward += staminaDelta;
        if (!loopDisabled && staminaReward >= disableLoopAtStamina)
        {
            loopDisabled = true;
            Debug.Log($"[RunData] 目標スコア {disableLoopAtStamina} 到達。以後 loop を無効化します。");
        }
    }

    public void AddSense(int senseDelta)
    {
        senseReward += senseDelta;
        if (!loopDisabled && senseReward >= disableLoopAtSense)
        {
            loopDisabled = true;
            Debug.Log($"[RunData] 目標スコア {disableLoopAtSense} 到達。以後 loop を無効化します。");
        }
    }

}