using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [Header("UI参照")]
    public TextMeshProUGUI timerText;
    public Button timerButton;

    [Header("タイマー設定")]
    public float startTime = 60f;  // 初期時間（秒）

    private float currentTime;
    private bool isRunning = false;
    private readonly object StatusUI;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerText();

        // ボタンが押されたときに StartOrResetTimer を呼ぶ
        timerButton.onClick.AddListener(StartOrResetTimer);
    }

    void Update()
    {
        if (isRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                isRunning = false;
                TimerEnd();
            }

            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        timerText.text = "" + Mathf.CeilToInt(currentTime);
    }

    void TimerEnd()
    {
        //timerText.text = "時間切れ！";
    }

    void StartOrResetTimer()
    {
        // 動いてなければスタート、動いていればリセット
        if (!isRunning)
        {
            isRunning = true;
            //timerButton.GetComponentInChildren<TextMeshProUGUI>().text = "リセット";   
            //ボタンを押したら
        }
        else
        {
            isRunning = false;
            currentTime = startTime;
            UpdateTimerText();
            // timerButton.GetComponentInChildren<TextMeshProUGUI>().text = "スタート";
            //再度ボタンを押したら
        }
    }
}
