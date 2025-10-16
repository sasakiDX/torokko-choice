using UnityEngine;
using UnityEngine.InputSystem.Switch;
using UnityEngine.SceneManagement;

//トロッコ本体

public class TrolleyChoice : MonoBehaviour
{
    [Header("移動設定")]
    // public float moveDistance = 3f; // 上下の移動幅（1回の往復距離）
    public float RidSpeed = 5f;     // 移動速度

    private Vector2 startPos;       // 初期位置
   // private int direction = 1;      // 進む方向（右:1, 左:-1）
    private int  isHitBox = 0;       // レール接触中のカウント
    private int  isChange = 0;       // Change 接触中かどうか

    private BoxCollider2D HitBox;   // 当たり判定

    void Start()
    {
        startPos = transform.position;          // 初期位置の保存
        HitBox = GetComponent<BoxCollider2D>(); // 当たり判定の取得
    }

    void Update()
    {
       
        //考えただけ
        /*switch(true)
        {
            case bool isHitBox:

                transform.Translate(Vector2.right * RidSpeed * Time.deltaTime);

                break;

        }
        */


        // レールに触れている間だけ動作
       //if (isHitBox > 0)
       //{
         
            transform.Translate(Vector2.right * RidSpeed * Time.deltaTime);
       //}


        // Change に触れたときにシーン移動
        if (isChange>0)
        {
            Debug.Log("Change に触れた！シーン移動");
            SceneManager.LoadScene("NextScene"); // 今後追加予定のシーン名
            isChange = 0; // 一度処理したらフラグを戻す
        }
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