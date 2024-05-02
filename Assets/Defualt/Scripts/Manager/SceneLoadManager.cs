using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private static SceneLoadManager Instance;
    public SceneLoadingUIController loadingUIController;

    [SerializeField] private GameObject loadingUIPrefab;
    [SerializeField] private bool isLoading;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GameManager.Instance.sceneLoadManager = this;
    }

    // �� �ε� �޼���
    public void LoadScene(string sceneName)
    {
        var target = GameObject.Find("LoadingScene Target");
        if(target == null)
        {
            print("LoadingScene Target�� ã�� ����");
        }
        var loadingScene = Instantiate(loadingUIPrefab, target.transform);
        loadingUIController = loadingScene.GetComponent<SceneLoadingUIController>();
        loadingUIController.Loading(sceneName);
    }

    // ���� ����� ȣ��
    public void JoiningServer(int progress)
    {
        if (!isLoading)
        {
            isLoading = true;
            var target = GameObject.Find("LoadingScene Target");
            var loadingScene = Instantiate(loadingUIPrefab, target.transform);
            loadingUIController = loadingScene.GetComponent<SceneLoadingUIController>();
        }

        loadingUIController.UpdataLoadingProgress(progress);
    }
}
