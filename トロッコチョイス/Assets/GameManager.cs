//using UnityEngine;

//public enum GameState
//{
//    Look,
//    Move,
//    Question
//}

//public class GameManager : MonoBehaviour
//{
//    public static GameManager Instance;
//    public GameState state = GameState.Look; // 初期状態

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject); // シーンをまたいでも破棄されない
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }
//}