
using UnityEngine;

[CreateAssetMenu(fileName = "LeverData", menuName = "Lever/LeverData", order = 0)]
public class LeverData : ScriptableObject
{
    public int id;

    [TextArea(2, 6)]
    public string LeverText;

    // 2択を想定（レバー1 / レバー2）
    public string[] choices = new string[2];

    // 正解インデックス（必要なければ使わなくてもOK）
    public int correctIndex = 0;

}
