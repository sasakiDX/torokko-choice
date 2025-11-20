using UnityEngine;

public class Change : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"{name} にプレイヤーが接触しました。問題を出します。");


        var assigner = GetComponent<QuestionAssigner>();
        if (assigner == null)
        {
            Debug.LogWarning($"{name} に QuestionAssigner が見つかりません。");
            return;
        }

        var questionData = QuestionManager.Instance.GetQuestion(assigner.questionID);
        if (questionData == null)
        {
            Debug.LogError($"QuestionID {assigner.questionID} に対応する問題が存在しません。");
            return;
        }

        var question = FindObjectOfType<Question>(true);
        if (question == null)
        {
            Debug.LogError("Question オブジェクトがシーンに存在しません。");
            return;
        }

        question.StartQuestion(questionData, choice =>
        {
            Debug.Log($"{name} の問題が終了しました。選択肢番号: {choice}");
        });

        if (GameManager.Instance != null)
            GameManager.Instance.ChangePoint = gameObject;
    }
}