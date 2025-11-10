using System.Collections.Generic;
using UnityEngine;

public class EventData
{
    public int ID;
    public string Question;
    public string Choice1;
    public string Result1;
    public string Cond1;
    public string Choice2;
    public string Result2;
    public string Cond2;
    public List<ChoiceData> Choices = new();

    public string Age { get; internal set; }
}
