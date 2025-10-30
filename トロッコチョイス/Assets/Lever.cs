using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.UI;




public class Lever : MonoBehaviour
{
    // レバーが引かれた時に分岐番号を通知するイベント
    public event Action<int> ChoicePoint;  // ← イベント名を変更

    public void PullLever(int choice)
    {
        // choice = 1 または 2 など
        Debug.Log("Lever pulled: choice " + choice);

        // メインに通知
        ChoicePoint?.Invoke(choice);
    }
}