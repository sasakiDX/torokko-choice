//using Unity.VisualScripting;
//using UnityEngine;

//[System.Serializable]
//public class PlayerStatus
//{
//    [Header("基本ステータス（0〜100）")]
//    [Range(0, 100)] public int intel;      // 知力
//    [Range(0, 100)] public int sense;      // センス
//    [Range(0, 100)] public int stamina;    // 体力
//    [Range(0, 100)] public int health;     // 健康（内部用）

//    [Header("その他")]
//    public int money;  // 所持金
//    public int age;    // 年齢

//    [Header("ランク（自動計算）")]
//    public string rankIntel;
//    public string rankSense;
//    public string rankStamina;

//    // 初期化処理
//    public void Initialize()
//    {
//        intel = 0;
//        sense = 0;
//        stamina = 0;
//        health = 100;
//        money = 0;
//        age = 20;

//        UpdateAllRanks();
//    }

//    // 値更新後にランクを再計算
//    public void UpdateAllRanks()
//    {
//        rankIntel = RankConverter.GetRank(intel);
//        rankSense = RankConverter.GetRank(sense);
//        rankStamina = RankConverter.GetRank(stamina);
//    }

//    // 各ステータスの増減
//    public void AddIntel(int value)
//    {
//        intel = Mathf.Clamp(intel + value, 0, 100);
//        rankIntel = RankConverter.GetRank(intel);
//    }

//    public void AddSense(int value)
//    {
//        sense = Mathf.Clamp(sense + value, 0, 100);
//        rankSense = RankConverter.GetRank(sense);
//    }

//    public void AddStamina(int value)
//    {
//        stamina = Mathf.Clamp(stamina + value, 0, 100);
//        rankStamina = RankConverter.GetRank(stamina);
//    }

//    public void AddHealth(int value)
//    {
//        health = Mathf.Clamp(health + value, 0, 100);
//    }

//    public void AddMoney(int value)
//    {
//        money += value;
//    }

//    public void AddAge(int value)
//    {
//        age += value;
//    }
//}