using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SpaceKey_SwitchSceneAsync : MonoBehaviour
{
    [Tooltip("遷移先のシーン名。空ならビルド順の次シーンへ")]
    public string sceneName = "Trolley";// 遷移先シーン名

    [Tooltip("Time.timeScale==0 でも受け付けるか")]
    [SerializeField] private bool acceptWhenPaused = true;

    private bool isLoading = false;

    void Update()
    {
        if (isLoading) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isLoading = true;

            if (acceptWhenPaused && Time.timeScale == 0f)
                Time.timeScale = 1f;

            StartCoroutine(LoadAsyncByNameOrIndex());
        }
    }

    private IEnumerator LoadAsyncByNameOrIndex()
    {
        string targetName = sceneName;// 遷移先シーン名
        int targetIndex = -1;

        if (string.IsNullOrEmpty(targetName))// 次のシーンへ
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;// 次のシーン
            if (nextIndex < SceneManager.sceneCountInBuildSettings)// シーン数内
            {
                targetIndex = nextIndex;// 次のシーンへ
            }
            else
            {
                targetIndex = 0;// 最初のシーンへ戻る
            }
        }

        AsyncOperation op = (targetIndex >= 0)
            ? SceneManager.LoadSceneAsync(targetIndex)
            : SceneManager.LoadSceneAsync(targetName);// シーン読み込み開始

        op.allowSceneActivation = true;// シーンアクティベーション許可

        // ここでフェードアウトやローディングUIを制御できます
        while (!op.isDone)
        {
            // 0.0~0.9 で読み込み、アクティベーションで 1.0
            // Debug.Log(op.progress);
            yield return null;
        }
    }
}
