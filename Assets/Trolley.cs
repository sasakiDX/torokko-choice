using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SocialPlatforms.Impl;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

// やることまとめ（元コメントを残します）
// 初期状態を保存する
// 出題レールの値をランダム化する⇒ランダムな問題が出るようにする
// UIの表示
// シーン切り替えの作成⇒いったんカメラ座標を合わせる方向

public class TrolleyChoice : MonoBehaviour
{
    private GameObject lastChangeObject = null;
    private string changeOriginalTag = "Change";

    [Header("移動設定")]
    public float RidSpeed = 10f;     // 移動速度
    public float upSpeed = 10f;      // 上下移動速度
    public float EndSlope = 124;     // スロープを出る位置
    public float slopeAngle = 0.0f;  // 坂レールの角度

    public Vector2 slopeEndPos;         // スロープ終了位置
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
    [SerializeField] public QuestionData currentQuestion; // 問題データ（互換のため残すが使用しない）
    [SerializeField] private Lever leverController;       // Lever 制御
    [SerializeField] public Lever currentLever;           // Lever データ
    [SerializeField] public int Choice = 0;               // 仮の選択肢変数（1/2仕様を維持）

    private Vector2 startPos;       // 初期位置
    private int isHitBox = 0;       // レール接触中のカウント
    private int isChange = 0;       // 分岐に接触中

    private BoxCollider2D HitBox;   // 当たり判定
    public GameObject ChoicePointObject;  // Lever がアタッチされたオブジェクト
    private Lever lever;                  // Lever コンポーネント

    // （任意）二重出題ガード：OnTriggerEnter2D が連続発火する環境での二重StartQuestion対策
    private bool asking = false;

    void Start()
    {
        startPos = transform.position;

        HitBox = GetComponent<BoxCollider2D>();

        // questionController が未設定の場合はシーン内から検索
        if (questionController == null)
        {
            questionController = FindObjectOfType<Question>();
        }

        // CHANGED: 「最初の1問を固定」する処理は削除
        // if (currentQuestion == null && QuestionManager.Instance != null && QuestionManager.Instance.questions.Count > 0)
        // {
        //     currentQuestion = QuestionManager.Instance.questions[0];
        // }
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
                state = Scene.Move;
                break;

            case Scene.Look:
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

                //ChangeレールからQuestionDataを取り直す
                if (questionController == null)
                {
                    Debug.LogError("questionController が設定されていません");
                    break;
                }

                //二重出題ガード
                if (asking)
                {
                    Debug.LogWarning("StartQuestion を二重に呼ぶのを抑止しました（asking==true）");
                    break;
                }
                asking = true; // 出題開始

                //Changeに付与されたQuestionAssignerを取得


                var assigner = other.GetComponent<QuestionAssigner>();
                if (assigner == null)
                {
                    Debug.LogError("この Change に QuestionAssigner が付いていません。");
                    asking = false;
                    break;
                }

                //QuestionManagerからQuestionDataを取得


                //ランダムの場合コメント
                var qm = QuestionManager.Instance;
                if (qm == null)
                {
                    Debug.LogError("QuestionManager.Instance が見つかりません。");
                    asking = false;
                    break;
                }

                var data = qm.GetQuestion(assigner.questionID);
                if (data == null)
                {
                    Debug.LogError($"Question ID {assigner.questionID} の QuestionData が見つかりません。");
                    asking = false;
                    break;
                }


                Debug.Log($"[StartQuestion] id={data.id}, text={data.questionText}");

                //取得したdataをそのまま渡して出題
                questionController.StartQuestion(data, (choiceResult) =>
                {
                    Choice = choiceResult;
                    Debug.Log("TrolleyChoice で受け取った Choice: " + Choice);
                    Debug.Log("Question終了後、Moveに戻る");

                    //速度復帰
                    isChange = 0;
                    RidSpeed = 10f;

                    if (Choice == 1)
                    {
                        state = Scene.Move;
                    }

                    //出題完了
                    asking = false;
                });
                break;

            case "slope":
                if (Choice == 2)
                {
                    slopeAngle = other.transform.eulerAngles.z;
                    state = Scene.UPRail;
                }
                break;

            case "loop Rail":
                //loopPoint(new Vector2(startPos.x, startPos.y));//ループ先
                slopeEndPos = new Vector2(115f, -88.15f);//ループ後坂終わり

                //switch文に直す 仮置き
                // ここでスコアによる判定を挟む
                if 
                (
                    RunData.Instance != null &&
                    RunData.Instance.score >= RunData.Instance.disableLoopAtScore
                    || 
                    RunData.Instance != null &&
                    RunData.Instance.moneyReward >= RunData.Instance.disableLoopAtMoney
                    ||
                    RunData.Instance != null &&
                    RunData.Instance.iqReward >= RunData.Instance.disableLoopAtIQ
                    ||
                    RunData.Instance != null &&
                    RunData.Instance.staminaReward >= RunData.Instance.disableLoopAtStamina
                    ||
                    RunData.Instance != null &&
                    RunData.Instance.senseReward >= RunData.Instance.disableLoopAtSense
                )

                {

                    Debug.Log("[Loop] スコア到達のため、シーン再読み込みをスキップします。");

                    break; // ← 再読み込みせず抜ける（以降の処理は行わない）
                }

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
            case 1:
                state = Scene.Move;
                break;
            case 2:
                // Choice 2 の処理（必要に応じて坂など）
                break;
        }
    }
}