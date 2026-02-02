using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LeverManager : MonoBehaviour
{
    public static LeverManager Instance;

    [Header("LeverData 一覧（Assets/Levers から自動ロード）")]
    public List<LeverData> Levers = new List<LeverData>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        LoadLevers();
    }

    private void LoadLevers()
    {
        Levers.Clear();

        // ① ビルド／実行時：Resources から読み込む
        LeverData[] loaded = Resources.LoadAll<LeverData>("Levers");
        if (loaded != null && loaded.Length > 0)
        {
            Levers.AddRange(loaded);
            Debug.Log($"LeverData(Resources) 読込完了：{Levers.Count} 件");
            return;
        }

#if UNITY_EDITOR
        // ② Editor で Resources 未整備の場合のフォールバック
        string[] guids = AssetDatabase.FindAssets("t:LeverData", new[] { "Assets" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            LeverData data = AssetDatabase.LoadAssetAtPath<LeverData>(path);
            if (data != null) Levers.Add(data);
        }

        if (Levers.Count > 0)
        {
            Debug.Log($"LeverData(Assets) 読込完了：{Levers.Count} 件（Editor専用フォールバック）");
        }
        else
        {
            Debug.LogError("LeverData が見つかりません。'Assets/Resources/Levers' に配置してください。");
        }
#else
        Debug.LogError("LeverData(Resources) が見つかりません。'Assets/Resources/Levers' に配置してください。");
#endif
    }

    public LeverData GetLever(int id)
    {
        foreach (var l in Levers)
            if (l != null && l.id == id) return l;

        Debug.LogWarning($"Lever ID {id} は見つかりません");
        return null;
    }
}