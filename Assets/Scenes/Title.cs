using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SpaceKey_SwitchSceneAsync : MonoBehaviour
{
    [Tooltip("遷移先のシーン名。空ならビルド順の次シーンへ")]
    public string sceneName = "Trolley";

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
        string targetName = sceneName;
        int targetIndex = -1;

        if (string.IsNullOrEmpty(targetName))
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                targetIndex = nextIndex;
            }
            else
            {
                targetIndex = 0;
            }
        }

        AsyncOperation op = (targetIndex >= 0)
            ? SceneManager.LoadSceneAsync(targetIndex)
            : SceneManager.LoadSceneAsync(targetName);

        op.allowSceneActivation = true;

        // ここでフェードアウトやローディングUIを制御できます
        while (!op.isDone)
        {
            // 0.0~0.9 で読み込み、アクティベーションで 1.0
            // Debug.Log(op.progress);
            yield return null;
        }
    }
}
