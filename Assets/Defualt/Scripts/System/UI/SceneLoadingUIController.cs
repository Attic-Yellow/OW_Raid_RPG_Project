using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Photon.Pun;

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
        int progressStep = 0;

        while (progressStep > Random.Range(30, 70))
        {
            progressStep++;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // �ε尡 �Ϸ�� ������ ���
        while (!asyncLoad.isDone)
        {
            if (progressStep < 100)
            {
                progressStep++;

                if (progressStep > 100)
                {
                    progressStep = 100;
                }

                loadingText.text = $"{progressStep}%";
                yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
            }
            else
            {
                asyncLoad.allowSceneActivation = true;
                yield return null;
            }
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
            yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
        }
        if (targetProgress > 30 && currentProgress == targetProgress)
        {
            StartCoroutine(LoadAsyncGameScene("GameScene"));
        }
        else
        {
            yield return null;
        }
    }

    // ���� > ���� �� ��ȯ �ε� �ڷ�ƾ (70% ~ 100%)
    IEnumerator LoadAsyncGameScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (currentProgress < 100)
            {
                int progressStep = 1;
                currentProgress += progressStep;
                if (currentProgress > 100)
                {
                    currentProgress = 100;
                }
                loadingText.text = $"{currentProgress}%";
                yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
            }
            else
            {
                asyncLoad.allowSceneActivation = true;
                yield return null;
            }
        }
     /* CharacterData.Instance.currentCharObj = PhotonNetwork.Instantiate(CharacterData.Instance.characterData["Job"].ToString(), Vector3.zero, Quaternion.identity);*/

    }
}
