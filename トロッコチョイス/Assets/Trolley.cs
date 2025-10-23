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

        //トロッコの状態のみ
        switch (state)
        {
            case Scene.Look:
                
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    state = Scene.Move;
                }
                break;

            case Scene.Move:
                transform.Translate(Vector2.right * RidSpeed * Time.deltaTime);//移動中
                if (isChange > 0)
                {
                    state = Scene.Look; // レールから離れたので停止状態へ
                }
                
                break;

            case Scene.Question:

                SceneManager.LoadScene("Question");//別クラスのイベントシーンへ移行 

                //問題クラスから帰ってきて動作するため問題クラス側に追加する
                //GameManager.Instance.state = GameState.Move; // 状態をMoveにセット（通知）
                //SceneManager.LoadScene("Trolley");// 切り替える
                //isChange--;

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


    private void OnTriggerEnter2D(Collider2D other)//タグに触れたとき
    { 
        // Scene が MOVE のときだけ処理を行う
        if (state != Scene.Move)
            return; // MOVE じゃなければ何もしない

        // MOVE 状態のときのみ switch 文を実行
        switch (other.tag)
        {
            case "Rail":
                isHitBox++; // 複数接触に対応
                //state = Scene.Move; // すでに MOVE なので不要
                break;

            case "Change":
                isChange++;
                if (isChange > 0)//レールの真ん中まで移動する
                {
                
                }
                    state = Scene.Question; // イベントへ移行
                //isChange--;//Question側に処理を追加する
                break;
        }
    }

  
}
