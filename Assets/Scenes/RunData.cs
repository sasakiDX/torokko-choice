// Assets/Scripts/System/RunData.cs
using UnityEngine;

public class RunData : MonoBehaviour
{
    public static RunData Instance { get; private set; }

    [Header("周回しても引き継ぎたいデータ")]
    public int score = 0;

    [Header("loop 無効化設定")]
    [Tooltip("このスコア以上になったらループを無効化してリザルトへ進める")]
    public int disableLoopAtScore = 500;
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
}