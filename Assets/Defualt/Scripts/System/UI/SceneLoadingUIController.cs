using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class SceneLoadingUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Sprite[] loadingImages;

    private int currentProgress = 0;

    private void Start()
    {
        SetImage();
    }

    // 로딩 이미지 랜덤 부여
    private void SetImage()
    {
        if (loadingImages.Length > 0)
        {
            Image renderer = gameObject.GetComponent<Image>();
            int index = Random.Range(0, loadingImages.Length);
            renderer.sprite = loadingImages[index];
        }
    }

    // 로딩 호출
    public void Loading(string sceneName)
    {
        StartCoroutine(LoadAsyncScene(sceneName));
    }

    // 로그인 > 메인 씬 전환 로딩 코루틴
    IEnumerator LoadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 로드가 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            int progressPercentage = (int)(asyncLoad.progress * 100.0f / 0.9f); // 진행 상황을 퍼센트로 변환하여 텍스트로 표시
            loadingText.text = progressPercentage.ToString() + "%";

            yield return null;
        }
    }

    // 메인 > 게임 씬 전환 로딩 호출 메서드
    public void UpdataLoadingProgress(int progress)
    {
        StartCoroutine(UpdataLoadingProgressCoroutine(progress));
    }

    // 메인 > 게임 씬 전환 로딩 코루틴 (1차: 0% ~ 30% 2차: 30% ~ 70%)
    IEnumerator UpdataLoadingProgressCoroutine(int targetProgress)
    {
        while (currentProgress < targetProgress)
        {
            currentProgress++;
            loadingText.text = $"{currentProgress}%";
            yield return new WaitForSeconds(Random.Range(0.01f, 0.05f));
        }
        if (currentProgress == 70)
        {
            StartCoroutine(LoadAsyncGameScene("GameScene"));
        }
    }

    // 메인 > 게임 씬 전환 로딩 코루틴 (70% ~ 100%)
    IEnumerator LoadAsyncGameScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            if (currentProgress < 100)
            {
                currentProgress++;
            }
            loadingText.text = $"{currentProgress}%";
            yield return new WaitForSeconds(Random.Range(0.01f, 0.05f));
        }

        SceneManager.LoadSceneAsync(sceneName);
    }
}
