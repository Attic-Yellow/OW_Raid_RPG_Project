using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("�Ŵ���")]
    public FirebaseManager firebaseManager;
    public AuthManager authManager;
    public PhotonManager photonManager;
    public DataManager dataManager;
    public UIManager uiManager;
    public SceneLoadManager sceneLoadManager;

    [Header("���� ������")]
    private bool isUserGuest = false;
    private bool isEmailAuthentication = false;
    private bool isManager = false;
    public GameObject currentPlayerObj;

    [Header("���� ������")]
    private bool isDataLoaded;
    private bool isSignInSuccess;
    private bool isRebinding;
    public Vector3 playerRespawnPos = new Vector3(1250 , -106.5f, 1020); //�÷��̾� ������ ����
    private List<GameObject> onMouseCheck = new();

    [Serializable]
    private class UserData
    {
        public bool guestUser { get; set; }
        public bool emailAuthentication { get; set; }
        public bool manager { get; set; }
    }

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

        // PlayerPrefs.DeleteAll(); // �׽�Ʈ�� �ڵ�
    }

    private void Start()
    {
        
    }

    // �α��� ���� �� ȣ��Ǵ� �ݹ� �޼���
    public void OnLoginSuccess()
    {
        StartCoroutine(CheckAutoLoginWhenReady());
    }

    // ���̾� ���̽� �Ŵ��� �ʱ�ȭ Ȯ�� �޼���
    private IEnumerator CheckAutoLoginWhenReady()
    {
        yield return new WaitUntil(() => firebaseManager != null && firebaseManager.IsFirebaseInitialized); // FirebaseManager�� �ʱ�ȭ�� ������ ���

        if (firebaseManager.auth != null) // FirebaseAuth�� �غ�Ǿ����� Ȯ��
        {
            StartCoroutine(WaitForUserData());
        }
        else
        {

        }
    }

    // ����� ������ �ҷ�����
    public async void LoadCurrentUserProfile()
    {

        if (firebaseManager.auth.CurrentUser != null)
        {
            var user = FirebaseAuth.DefaultInstance.CurrentUser;
            await firebaseManager.LoadUserData(user.UserId, user.Email, OnUserDataLoaded);
        }
        else
        {
            Debug.LogError("����ڰ� �α��εǾ� ���� ����");
        }
    }

    // ����� ������ �ε� �� ȣ��Ǵ� �ݹ� �޼���
    private void OnUserDataLoaded(Dictionary<string, object> userData)
    {
        if (userData != null)
        {
            try
            {
                string json = JsonConvert.SerializeObject(userData); // userData ������ �ٽ� JSON ���ڿ��� ��ȯ
                UserData deserializedUserData = JsonConvert.DeserializeObject<UserData>(json); // JSON ���ڿ��� UserData Ŭ������ ������ȭ

                if (deserializedUserData != null) // ������ȭ�� ��ü�� �Ӽ��� ���� ����
                {
                    isUserGuest = deserializedUserData.guestUser;
                    isEmailAuthentication = deserializedUserData.emailAuthentication;
                    isManager = deserializedUserData.manager;

                    isDataLoaded = true;
                    isSignInSuccess = true;
                }
                else
                {
                    print("����� ������ ������ȭ ����");
                }
            }
            catch (Exception ex)
            {
                print($"����� ������ ó�� �� ���� �߻�: {ex.Message}");
            }
        }
        else
        {
            //ToDo : �α��� ��õ� ���� �ʿ�
        }
    }

    private IEnumerator WaitForUserData()
    {
        isDataLoaded = false;
        LoadCurrentUserProfile();
        yield return new WaitUntil(() => isDataLoaded);

        if (isEmailAuthentication || isUserGuest)
        {
        }
        else
        {
            yield return new WaitUntil(() => isEmailAuthentication);
        }
    }

    // �α׾ƿ�
    public void LogOut()
    {
        firebaseManager.SignOut();
        // sceneManager.LoadSceneForLogin();
    }

    public void SetIsUserGuest(bool isUserGuest)
    {
        this.isUserGuest = isUserGuest;
    }

    public bool GetIsUserGuest()
    {
        return isUserGuest;
    }

    // �̸��� ���� ���� ����
    public void SetIsEmailAuthentication(bool isEmailAuthentication)
    {
        this.isEmailAuthentication = isEmailAuthentication;
    }

    // �̸��� ���� ���� ��ȯ
    public bool GetIsEmailAuthentication()
    {
        return isEmailAuthentication;
    }

    // �α��� ���� ���� ����
    public void SetIsSignInSuccess(bool isSignInSuccess)
    {
        this.isSignInSuccess = isSignInSuccess;
    }

    // �α��� ���� ���� ��ȯ
    public bool GetIsSignInSuccess()
    {
        return isSignInSuccess;
    }

    // �Ŵ������� ��ȯ
    public bool GetIsManager()
    {
        return isManager;
    }

    // �����ε� ���� ����
    public void SetIsRebinding(bool isRebind)
    {
        isRebinding = isRebind;
    }

    // �����ε� ���� ��ȯ
    public bool GetIsRebinding()
    {
        return isRebinding;
    }

    public List<GameObject> GetAlives()
    {
        return onMouseCheck;
    }

    public void AddOnMousePointer(GameObject obj)
    {
        onMouseCheck.Add(obj);
    }

    public void RemoveOnMousePointer(GameObject obj)
    {
        onMouseCheck.Remove(obj);
    }
 
}
