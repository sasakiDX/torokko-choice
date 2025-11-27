//using System;
//using System.Text.RegularExpressions;
//using UnityEngine;

//public class StatusManager : MonoBehaviour
//{
//    public static StatusManager Instance { get; private set; }

//    public PlayerStatus playerStatus = new();

//    public event Action<PlayerStatus> OnStatusUpdated;

//    void Awake()
//    {
//        if (Instance == null)
//            Instance = this;
//        else
//            Destroy(gameObject);
//    }

//    void Start()
//    {
//        playerStatus.Initialize();
//    }

//    // 例：「intel+3 sense-2 money+100」
//    public void ApplyResultString(string result)
//    {
//        if (string.IsNullOrEmpty(result))
//        {
//            Debug.LogWarning("結果文字列が空です");
//            return;
//        }

//        // 正規表現で「単語+数値」を抽出
//        MatchCollection matches = Regex.Matches(result, @"(\w+)([+\-]\d+)");
//        foreach (Match m in matches)
//        {
//            string key = m.Groups[1].Value.ToLower();
//            int value = int.Parse(m.Groups[2].Value);

//            switch (key)
//            {
//                case "intel": playerStatus.AddIntel(value); break;
//                case "sense": playerStatus.AddSense(value); break;
//                case "stamina": playerStatus.AddStamina(value); break;
//                case "health": playerStatus.AddHealth(value); break;
//                case "money": playerStatus.AddMoney(value); break;
//                case "age": playerStatus.AddAge(value); break;
//                default:
//                    Debug.LogWarning($"未知のステータス名: {key}");
//                    break;
//            }
//        }

//        playerStatus.UpdateAllRanks();

//        // UI更新イベント通知
//        OnStatusUpdated?.Invoke(playerStatus);

//        // 死亡判定チェック
//        DeathChecker.CheckDeath(playerStatus);
//    }
//}