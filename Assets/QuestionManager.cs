using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager Instance;

    [Tooltip("Assets フォルダ直下に置いた QuestionData の一覧 (自動読み込みされます)")]
    public List<QuestionData> questions = new List<QuestionData>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        LoadQuestionsFromAssets();
    }

    void LoadQuestionsFromAssets()
    {
        questions.Clear();

#if UNITY_EDITOR
        // Assets フォルダ直下のすべての QuestionData を取得
        string[] guids = AssetDatabase.FindAssets("t:QuestionData", new[] { "Assets" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            QuestionData data = AssetDatabase.LoadAssetAtPath<QuestionData>(path);
            if (data != null)
                questions.Add(data);
        }

        if (questions.Count == 0)
        {
            Debug.LogError("Assets フォルダに QuestionData が存在しません。");
            return;
        }

        Debug.Log($"QuestionData Asset 読込完了：{questions.Count} 問");
#else
        Debug.LogError("このコードは Editor 専用です。ビルドでは Resources を使用してください。");
#endif
    }

    public QuestionData GetQuestion(int id)
    {
        foreach (var q in questions)
        {
            if (q != null && q.id == id)
                return q;
        }

        Debug.LogWarning($"Question ID {id} は見つかりません");
        return null;
    }
}
