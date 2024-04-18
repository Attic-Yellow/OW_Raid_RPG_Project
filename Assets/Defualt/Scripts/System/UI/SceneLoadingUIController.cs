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

    // �ε� �̹��� ���� �ο�
    private void SetImage()
    {
        if (loadingImages.Length > 0)
        {
            Image renderer = gameObject.GetComponent<Image>();
            int index = Random.Range(0, loadingImages.Length);
            renderer.sprite = loadingImages[index];
        }
    }

    // �ε� ȣ��
    public void Loading(string sceneName)
    {
        StartCoroutine(LoadAsyncScene(sceneName));
    }

    // �α��� > ���� �� ��ȯ �ε� �ڷ�ƾ
    IEnumerator LoadAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // �ε尡 �Ϸ�� ������ ���
        while (!asyncLoad.isDone)
        {
            int progressPercentage = (int)(asyncLoad.progress * 100.0f / 0.9f); // ���� ��Ȳ�� �ۼ�Ʈ�� ��ȯ�Ͽ� �ؽ�Ʈ�� ǥ��
            loadingText.text = progressPercentage.ToString() + "%";

            yield return null;
        }
    }

    // ���� > ���� �� ��ȯ �ε� ȣ�� �޼���
    public void UpdataLoadingProgress(int progress)
    {
        StartCoroutine(UpdataLoadingProgressCoroutine(progress));
    }

    // ���� > ���� �� ��ȯ �ε� �ڷ�ƾ (1��: 0% ~ 30% 2��: 30% ~ 70%)
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

    // ���� > ���� �� ��ȯ �ε� �ڷ�ƾ (70% ~ 100%)
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
