using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using System;

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

    [SerializeField] private Question questionController; // Question UI制御
    [SerializeField] public QuestionData currentQuestion;           // 問題データ


    private Vector2 startPos;       // 初期位置
    // private int direction = 1;      // 進む方向（右:1, 左:-1）
    private int  isHitBox = 0;       // レール接触中のカウント
    private int  isChange = 0;       // 分岐に接触中
    //private bool isChange = false;

    private BoxCollider2D HitBox;   // 当たり判定
    [SerializeField] private Question questionRef;// Question参照

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
                
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    state = Scene.Move;
                }
                break;

            case Scene.Move:
                transform.Translate(Vector2.right * RidSpeed * Time.deltaTime);//移動中

                if (isChange > 0)
                {
                  RidSpeed = 0;
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    RidSpeed++;
                }
                break;

            //case Scene.Question:
            //    questionController.StartQuestion
            //        (currentQuestion, () =>
            //    {
            //        Debug.Log("Question終了後、Moveに戻る");
            //        isChange--;
            //        state = Scene.Move;
            //    });
            //    break;

        }



    }


        //transform.Translate(Vector2.right * RidSpeed * Time.deltaTime);
        //レールに触れている間だけ動作
        
        ///*
        //if (isChange > 0)
        //{
        //    Scene.Question();//イベントシーンへ移行
        //}
        //*/




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

                GameManager.Instance.ChangePoint = other.gameObject; // 直前のChangeを記録

                if (questionController != null && currentQuestion != null)
                {
                    // Questionを開始
                    questionController.StartQuestion(currentQuestion, () =>
                    {
                        Debug.Log("Question終了後、Moveに戻る");
                        isChange--;
                        state = Scene.Move;
                    });

                    
                   
                }
                else
                {
                    Debug.LogError("questionController または currentQuestion が設定されていません");
                }
                break;


                //case "Change":
                //    isChange++;

                //    GameManager.Instance.ChangePoint = other.gameObject; // 直前のChangeを記録

                //    Change changeComp = other.GetComponent<Change>();
                //    if (changeComp == null)
                //    {
                //        Debug.LogWarning($"{other.name} に Change コンポーネントがありません");
                //    }

                //    if (questionController != null && currentQuestion != null)
                //    {
                //        questionController.StartQuestion(currentQuestion, () =>
                //        {
                //            Debug.Log("Question終了後、Moveに戻る");
                //            isChange--;
                //            state = Scene.Move;
                //        });
                //    }
                //    else
                //    {
                //        Debug.LogError("questionController または currentQuestion が設定されていません");
                //        isChange--; // 念のため減らす
                //        state = Scene.Move;
                //    }
                //    break;




                /*
                if (changeComp != null && QuestionCSVLoader.Questions.Count > 0)
                {
                    int id = changeComp.questionID - 1;
                    if (id >= 0 && id < QuestionCSVLoader.Questions.Count)
                    {
                        currentQuestion = QuestionCSVLoader.Questions[id];
                        Debug.Log($"問題 {id + 1} を取得: {currentQuestion.questionText}");
                    }
                }
                */

        }
    }

  
}
