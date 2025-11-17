using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // シングルトン用のインスタンス
    public static EventManager Instance { get; private set; }

    public EventLoader loader;
    private EventData currentEvent;

    void Awake()
    {
        // シングルトンのインスタンス設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移しても破棄されないようにする（必要なら）
        }
        else
        {
            Debug.LogWarning("EventManagerのインスタンスが重複しているため破棄します。");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 仮：最初のイベントを選択
        PickRandomEvent();
    }

    public void PickRandomEvent()
    {
        if (loader == null || loader.eventList.Count == 0)
        {
            Debug.LogError("イベントが読み込まれていません！");
            return;
        }

        // 条件処理は未実装（別担当に渡すため）
        currentEvent = loader.eventList[Random.Range(0, loader.eventList.Count)];

        // UI担当にイベント送信（仮想）
        SendEventToUI(currentEvent);
    }

    void SendEventToUI(EventData ev)
    {
        Debug.Log($"【イベント発生】{ev.Question}");

        for (int i = 0; i < ev.Choices.Count; i++)
        {
            var c = ev.Choices[i];
            Debug.Log($"選択肢{i + 1}: {c.Text} [条件: {c.Condition}]");
        }
    }

    public void OnChoiceSelected(int choiceIndex)
    {
        if (currentEvent == null || choiceIndex >= currentEvent.Choices.Count)
        {
            Debug.LogError("不正な選択インデックス");
            return;
        }

        ChoiceData choice = currentEvent.Choices[choiceIndex];

        // ステータス担当に結果文字列を渡す（仮想）
        SendResultToStatusSystem(choice.Result);
    }

    void SendResultToStatusSystem(string result)
    {
        Debug.Log($"[結果送信] → ステータス管理：{result}");
        // if (StatusManager.Instance != null)
        //{
        //     StatusManager.Instance.ApplyResultString(result);
        //}
    }
}