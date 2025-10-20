using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem.Switch;
using UnityEngine.SceneManagement;

//トロッコ本体

public class TrolleyChoice : MonoBehaviour
{
    [Header("移動設定")]
    // public float moveDistance = 3f; // 上下の移動幅（1回の往復距離）
    public float RidSpeed = 5f;     // 移動速度(たまに反映されないため要確認)


    enum Scene
    {     
        Look,// 停止中
        Move,     // レール上で移動中
        Question    // 分岐に入った（イベント発生）

    }

    private Scene state = Scene.Look; // 現在の状態


    private Vector2 startPos;       // 初期位置
   // private int direction = 1;      // 進む方向（右:1, 左:-1）
    private int  isHitBox = 0;       // レール接触中のカウント
    private int  isChange = 0;       // 分岐に接触中

    private BoxCollider2D HitBox;   // 当たり判定

    void Start()
    {
        startPos = transform.position;          // 初期位置の保存
        HitBox = GetComponent<BoxCollider2D>(); // 当たり判定の取得
    }

    void Update()
    {

        //考えただけ
        switch (state)
        {
            case Scene.Look:
                //if()//特定の動作をしたら移動が開始する
                state = Scene.Move;
                break;

            case Scene.Move:
                transform.Translate(Vector2.right * RidSpeed * Time.deltaTime);//移動中
                if (isChange > 0)
                {
                    state = Scene.Question;
                }
                break;

            case Scene.Question:
                SceneManager.LoadScene("Question");//                   
                state = Scene.Move;
                //GameManager.Instance.state = GameState.Move; // 状態をMoveにセット（通知）
                //SceneManager.LoadScene("MainScene");         // 実際にMainSceneに切り替える

                break;


        }


        //transform.Translate(Vector2.right * RidSpeed * Time.deltaTime);
        //レールに触れている間だけ動作

        ///*
        //if (isChange > 0)
        //{
        //    Scene.Question();//イベントシーンへ移行

        //}
        //*/

    }


    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Rail")) // レールに触れているとき
    //    {
    //        isHitBox++; // 複数接触に対応
    //        Debug.Log("レールに接触");
    //    }
    //    else if (other.CompareTag("Change")) // Change に触れたとき
    //    {
    //        isChange++;
    //        Debug.Log("Change に接触");
    //        //ここで問題を出す
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.CompareTag("Rail")) // レールから離れたとき
    //    {
    //        isHitBox--; // 複数接触に対応
    //        Debug.Log("レールから離脱");
    //    }
    //    else if (other.CompareTag("Change")) // Change から離れたとき
    //    {
    //        isChange--;
    //        Debug.Log("Change から離脱");
    //        //再び移動を始める
    //    }
    //}
}