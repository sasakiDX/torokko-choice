using UnityEngine;

[CreateAssetMenu(fileName = "QuestionData", menuName = "Questions/QuestionData", order = 0)]
public class QuestionData : ScriptableObject
{
    public int id;
    [TextArea(2, 6)]
    public string questionText;
    public string[] choices;
    public int correctIndex;
}