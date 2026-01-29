
using UnityEngine;
using UnityEngine.UI;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;



//やることまとめ
//初期状態を保存する
//出題レールの値をランダム化する⇒ランダムな問題が出るようにする
//UIの表示
//シーン切り替えの作成⇒いったんカメラ座標を合わせる方向
//



public class TrolleyChoice : MonoBehaviour
{
    private GameObject lastChangeObject = null;
    private string changeOriginalTag = "Change";

    [Header("移動設定")]
    public float RidSpeed = 10f;     // 移動速度
    public float upSpeed = 10f;      // 上下移動速度
    public float EndSlope = 124;     // スロープを出る位置
    public float slopeAngle = 0.0f;  // 坂レールの角度

    public Vector2 slopeEndPos;      // スロープ終了位置
    public float slopeExitRange = 0.1f; // 誤差許容

    enum Scene
    {
        Start,
        Look,
        Move,
        UPRail
    }

    private Scene state = Scene.Look;

    [SerializeField] private Question questionController; // Question UI制御
    [SerializeField] public QuestionData currentQuestion; // 問題データ
    [SerializeField] private Lever leverController;       // Lever 制御
    [SerializeField] public Lever currentLever;          // Lever データ
    [SerializeField] public int Choice = 0;              // 仮の選択肢変数

    private Vector2 startPos;       // 初期位置
    private int isHitBox = 0;       // レール接触中のカウント
    private int isChange = 0;       // 分岐に接触中

    private BoxCollider2D HitBox;   // 当たり判定
    public GameObject ChoicePointObject;  // Lever がアタッチされたオブジェクト
    private Lever lever;                  // Lever コンポーネント

    void Start()
    {
        startPos = transform.position;

        HitBox = GetComponent<BoxCollider2D>();

        // questionController が未設定の場合はシーン内から検索
        if (questionController == null)
        {
            questionController = FindObjectOfType<Question>();
        }

        // currentQuestion が未設定の場合は QuestionManager から1問目を自動設定
        if (currentQuestion == null && QuestionManager.Instance != null && QuestionManager.Instance.questions.Count > 0)
        {
            currentQuestion = QuestionManager.Instance.questions[0];
        }
    }

    void ResetActionParameters()
    {
        RidSpeed = 10f;
        upSpeed = 10f;
        slopeAngle = 0f;
        isChange = 0;
        isHitBox = 0;
        state = Scene.Move;
    }

    void loopPoint(Vector2 newPos)
    {
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    void Update()
    {
        switch (state)
        {
            case Scene.Start:
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    state = Scene.Look;
                break;

            case Scene.Look:
                if (Input.GetKeyDown(KeyCode.RightArrow))
                    state = Scene.Move;
                break;

            case Scene.Move:

                transform.Translate(Vector2.right * RidSpeed * Time.deltaTime);

                if (isChange > 0)
                {
                    RidSpeed = 0;
                    state = Scene.Look;
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    RidSpeed++;
                }
                break;

            case Scene.UPRail:

                float rad = slopeAngle * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

                transform.Translate(dir * upSpeed * Time.deltaTime);

                // ---- スロープ終了座標に到達したら通常レールへ戻す ----
                if (Vector3.Distance(transform.position, slopeEndPos) <= slopeExitRange)
                {
                    upSpeed = 0.0f;
                    slopeAngle = 0f;
                    transform.rotation = Quaternion.identity;

                    Debug.Log("スロープ終了座標に到達 → Move へ切り替え");

                    state = Scene.Move;
                    break;
                }

                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (state != Scene.Move)
            return;

        switch (other.tag)
        {
            case "Rail":
                isHitBox++;
                break;

            case "Change":
                isChange++;
                Debug.Log("Changeに触れた");

                GameManager.Instance.ChangePoint = other.gameObject;

                if (questionController != null && currentQuestion != null)
                {
                    questionController.StartQuestion(currentQuestion, (choiceResult) =>
                    {
                        Choice = choiceResult;
                        Debug.Log("TrolleyChoice で受け取った Choice: " + Choice);
                        Debug.Log("Question終了後、Moveに戻る");

                        //速度が戻った
                        isChange = 0;

                        RidSpeed = 10f;

                        Choice = choiceResult; // 結果を保持
                        if (Choice == 0)
                        {
                            state = Scene.Move;   // 下のルート
                        }

                        isChange = 0;
                    });
                }
                else
                {
                    Debug.LogError("questionController または currentQuestion が設定されていません");
                }
                break;


            case "slope":
                if (Choice == 1)
                {
                    slopeAngle = other.transform.eulerAngles.z;
                    state = Scene.UPRail;
                }
                break;

            case "loop Rail":
                //loopPoint(new Vector2(startPos.x, startPos.y));//ループ先
                slopeEndPos = new Vector2(115f, -88.15f);//ループ後坂終わり
                                                         //115
                                                         //-88


                // ★ ここでスコアによる判定を挟む（閾値以上なら再読み込みしない）
                if (RunData.Instance != null &&
                    RunData.Instance.score >= RunData.Instance.disableLoopAtScore)
                {
                    Debug.Log("[Loop] スコア閾値到達のため、シーン再読み込みをスキップします。");
                    break; // ← 再読み込みせず抜ける（以降の処理は行わない）
                }

                // ★ 多重発火対策（任意）
               

                // ポーズ解除（必要に応じて）
                if (Time.timeScale == 0f) Time.timeScale = 1f;

                // 既存：シーン自体を再読み込み
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

                break;




        }
    }

    public void ChoicePoint(int choice)
    {
        switch (choice)
        {
            case 0:
                state = Scene.Move;
                break;
            case 1:
                // Choice 1 の処理（必要に応じて坂など）
                break;
        }
    }
}