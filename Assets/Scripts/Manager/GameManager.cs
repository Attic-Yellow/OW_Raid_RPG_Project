using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public FirebaseManager firebaseManager;
    public LanguageManager languageManager;
    public UIManager uiManager;
    [SerializeField] private LanguageType languageType;

    public FirebaseAuth auth { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            firebaseManager.InitializeFirebase();
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // PlayerPrefs.DeleteAll();
        languageType = (LanguageType)PlayerPrefs.GetInt("LanguageSetting", (int)LanguageType.Korean);
    }

    // 현재 활성화된 언어를 설정
    public void SetLanguageType(LanguageType SetLanguageType)
    {
        languageType = SetLanguageType;
        PlayerPrefs.SetInt("LanguageSetting", (int)languageType);
        PlayerPrefs.Save();
    }

    // 현재 활성화된 언어를 반환
    public LanguageType GetCurrentLanguage()
    {
        return languageType;
    }
}
