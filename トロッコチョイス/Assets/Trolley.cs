using UnityEngine;
using UnityEngine.SceneManagement;

public class TrolleyChoice : MonoBehaviour
{
    [Header("移動設定")]
   // public float moveDistance = 3f; // 上下の移動幅（1回の往復距離）
    public float RidSpeed = 2f;     // 移動速度

    private Vector2 startPos;       // 初期位置
    private int direction = 1;      // 進む方向（右:1, 左:-1）
    private bool isHitBox = false;  // 当たり判定があるなら発動
    private bool isChange = false;  // 分岐に入ったときに発動

    private BoxCollider2D HitBox;   //当たり判定



    void Start()
    {
        startPos = transform.position;// 初期位置の保存
        HitBox = GetComponent<BoxCollider2D>();//当たり判定の取得
    }


    void Update()
    {
        // レールに触れている間だけ動作
        if (isHitBox)
        {
            transform.Translate(Vector2.right * direction * RidSpeed * Time.deltaTime);// 右方向に移動
        }
        if (isChange)
        {
            //問題をだす
            //ステータスの変更
            //問題に回答したら対応するコードを発動する


            // 一度処理したらフラグを戻す
            isChange = false;
        }
    }

 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rail")) //レールに触れているとき実行
        {
            isHitBox = true;//触れている
        }
        else if (other.CompareTag("Branch"))
        {
            isChange = true;
        }
    }

   
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Rail"))//レールに触れているとき停止
        {
            isHitBox = false;//触れていない
        }
    }
}