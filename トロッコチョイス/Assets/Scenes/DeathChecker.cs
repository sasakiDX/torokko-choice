using UnityEngine;

public static class DeathChecker
{
    public static void CheckDeath(PlayerStatus status)
    {
        // シンプルな例：健康が0、または年齢が100以上で死亡
        if (status.health <= 0)
        {
            Debug.Log("死亡判定：健康が尽きました。");
            GameOver("健康が0になった");
        }
        else if (status.age >= 100)
        {
            Debug.Log("死亡判定：老衰により死亡。");
            GameOver("年齢が上限に達した");
        }
    }

    static void GameOver(string reason)
    {
        // 実際にはUI遷移やシーン変更など
        Debug.Log($"【ゲームオーバー】理由：{reason}");
    }
}