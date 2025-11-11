using UnityEngine;

public class Change : MonoBehaviour
{
    private bool isTriggered = false; // 二重反応防止

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;
        if (!other.CompareTag("Player")) return;

        isTriggered = true;
        Debug.Log($"{name} にプレイヤーが接触しました。問題を出します。");

        QuestionAssigner assigner = GetComponent<QuestionAssigner>();
        if (assigner == null)
        {
            Debug.LogWarning($"{name} に QuestionAssigner が見つかりません。");
            return;
        }

        // QuestionManagerを取得して問題を取得
        var questionData = QuestionManager.Instance.GetQuestion(assigner.questionID);
        if (questionData == null)
        {
            Debug.LogError($"QuestionID {assigner.questionID} に対応する問題が存在しません。");
            return;
        }

        // Questionオブジェクトを探してUIを起動
        Question question = FindObjectOfType<Question>(true);
        if (question == null)
        {
            Debug.LogError("Question オブジェクトがシーンに存在しません。");
            return;
        }

        // 問題を開始
        question.StartQuestion(questionData, OnQuestionFinished);

        // GameManagerに現在のChangeを記録（タグ変更用など）
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangePoint = gameObject;
        }
    }

    // 問題終了時に呼ばれるコールバック
    private void OnQuestionFinished(int choice)
    {
        Debug.Log($"{name} の問題が終了しました。選択肢番号: {choice}");
        isTriggered = false; // 再び通れるように
    }
}
