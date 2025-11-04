using UnityEngine;
using System;

public class Lever : MonoBehaviour
{
    [Header("レバー設定")]
    public string leverTriggerName = "Pull";  // AnimatorのTrigger名
    public AudioClip leverSound;              // 効果音

    public int LeverPoint = 0;//仮の選択肢変数
    private AudioSource audioSource;
    private bool canInteract = false; // Change に触れたかどうか

    // Question に通知するイベント
    public static event Action<string> OnLeverClicked;

    private bool isClicked = false; // クリックされたかどうか

    enum Secen
    {

        Block,
        Check,
    }

    private Secen state = Secen.Block; // 現在の状態

    void Start()
    {
        // AudioSource 自動取得
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリック
        {
          
            
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);// レイキャストでクリック対象を取得

                if (hit.collider != null && hit.collider.CompareTag("Lever"))
                {
                    GameObject lever = hit.collider.gameObject;
                    Debug.Log("Lever clicked: " + lever.name);

                    HandleLever(lever);

                    // 処理後にクリックフラグをリセット
                    ResetClick();

                }

           
        }
       
               
    }


    public void HandleLever(GameObject lever)
    {



        // --- アニメーション ---
        Animator anim = lever.GetComponent<Animator>();
        if (anim != null && !string.IsNullOrEmpty(leverTriggerName))
            anim.SetTrigger(leverTriggerName);

        // --- 効果音 ---
        if (leverSound != null && audioSource != null)
            audioSource.PlayOneShot(leverSound);

        // --- Questionに通知 ---
        OnLeverClicked?.Invoke(lever.name);

    }

    public void ResetClick()
    {
        isClicked = false;
    }
}