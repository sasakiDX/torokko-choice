using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject ChangePoint; // 直前のChangeを保存するための変数
    private Lever lever;            // Leverコンポーネント

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも保持
        }
        else
        {
            Destroy(gameObject);
        }
    }
}