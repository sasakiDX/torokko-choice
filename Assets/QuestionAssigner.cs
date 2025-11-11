using UnityEngine;

public class QuestionAssigner : MonoBehaviour
{
    public int questionID; // CSV上のIDと対応

    private QuestionData assignedQuestion;

    void Start()
    {
        assignedQuestion = QuestionManager.Instance.GetQuestion(questionID);
        if (assignedQuestion == null)
        {
            Debug.LogWarning($"{gameObject.name} に割り当て可能な問題が見つかりません。");
        }
        else
        {
            Debug.Log($"{gameObject.name} に問題 '{assignedQuestion.questionText}' を割り当て。");
        }
    }

    public QuestionData GetAssignedQuestion()
    {
        return assignedQuestion;
    }
}