using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
[System.Serializable]
[CreateAssetMenu(fileName = "NewQuestionData", menuName = "Question/QuestionData")]
public class QuestionData : ScriptableObject
{
    public string questionText;
    public string[] choices;
    public int correctIndex;
}