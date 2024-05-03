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

    [Header("매니저")]
    public FirebaseManager firebaseManager;
    public AuthManager authManager;
    public PhotonManager photonManager;
    public DataManager dataManager;
    public UIManager uiManager;
    public SceneLoadManager sceneLoadManager;

    [Header("유저 데이터")]
    private bool isUserGuest = false;
    private bool isEmailAuthentication = false;
    private bool isManager = false;
    public GameObject currentPlayerObj;

    [Header("게임 데이터")]
    private bool isDataLoaded;
    private bool isSignInSuccess;
    private bool isRebinding;
    public Vector3 playerRespawnPos = new Vector3(50, 10, 150);

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

        // PlayerPrefs.DeleteAll(); // 테스트용 코드
    }

    private void Start()
    {
        
    }

    // 로그인 성공 시 호출되는 콜백 메서드
    public void OnLoginSuccess()
    {
        StartCoroutine(CheckAutoLoginWhenReady());
    }

    // 파이어 베이스 매니저 초기화 확인 메서드
    private IEnumerator CheckAutoLoginWhenReady()
    {
        yield return new WaitUntil(() => firebaseManager != null && firebaseManager.IsFirebaseInitialized); // FirebaseManager가 초기화될 때까지 대기

        if (firebaseManager.auth != null) // FirebaseAuth가 준비되었는지 확인
        {
            StartCoroutine(WaitForUserData());
        }
        else
        {

        }
    }

    // 사용자 데이터 불러오기
    public async void LoadCurrentUserProfile()
    {

        if (firebaseManager.auth.CurrentUser != null)
        {
            var user = FirebaseAuth.DefaultInstance.CurrentUser;
            await firebaseManager.LoadUserData(user.UserId, user.Email, OnUserDataLoaded);
        }
        else
        {
            Debug.LogError("사용자가 로그인되어 있지 않음");
        }
    }

    // 사용자 데이터 로드 후 호출되는 콜백 메서드
    private void OnUserDataLoaded(Dictionary<string, object> userData)
    {
        if (userData != null)
        {
            try
            {
                string json = JsonConvert.SerializeObject(userData); // userData 사전을 다시 JSON 문자열로 변환
                UserData deserializedUserData = JsonConvert.DeserializeObject<UserData>(json); // JSON 문자열을 UserData 클래스로 역직렬화

                if (deserializedUserData != null) // 역직렬화된 객체의 속성에 직접 접근
                {
                    isUserGuest = deserializedUserData.guestUser;
                    isEmailAuthentication = deserializedUserData.emailAuthentication;
                    isManager = deserializedUserData.manager;

                    isDataLoaded = true;
                    isSignInSuccess = true;
                }
                else
                {
                    print("사용자 데이터 역직렬화 실패");
                }
            }
            catch (Exception ex)
            {
                print($"사용자 데이터 처리 중 오류 발생: {ex.Message}");
            }
        }
        else
        {
            //ToDo : 로그인 재시도 로직 필요
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

    // 로그아웃
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

    // 이메일 인증 여부 저장
    public void SetIsEmailAuthentication(bool isEmailAuthentication)
    {
        this.isEmailAuthentication = isEmailAuthentication;
    }

    // 이메일 인증 여부 반환
    public bool GetIsEmailAuthentication()
    {
        return isEmailAuthentication;
    }

    // 로그인 성공 여부 저장
    public void SetIsSignInSuccess(bool isSignInSuccess)
    {
        this.isSignInSuccess = isSignInSuccess;
    }

    // 로그인 성공 여부 반환
    public bool GetIsSignInSuccess()
    {
        return isSignInSuccess;
    }

    // 매니저인지 반환
    public bool GetIsManager()
    {
        return isManager;
    }

    // 리바인딩 상태 저장
    public void SetIsRebinding(bool isRebind)
    {
        isRebinding = isRebind;
    }

    // 리바인딩 상태 반환
    public bool GetIsRebinding()
    {
        return isRebinding;
    }

    public void AddPowerCoroutine(float _value, float addPowerValue)
    {
        StartCoroutine(AddPower(_value, addPowerValue));
    }

    IEnumerator AddPower(float _value, float addPowerValue)
    {
        if (currentPlayerObj != null)
         currentPlayerObj.GetComponent<Player>().Power += addPowerValue;

        yield return new WaitForSeconds(_value);

        if (currentPlayerObj != null)
           currentPlayerObj.GetComponent<Player>().Power -= addPowerValue;

    }
}
