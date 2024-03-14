using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public LanguageManager languageManager;
    public UIManager uiManager;
    [SerializeField] private LanguageType languageType;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // PlayerPrefs.DeleteAll();
        languageType = (LanguageType)PlayerPrefs.GetInt("LanguageSetting", (int)LanguageType.Korean);
    }

    // ���� Ȱ��ȭ�� �� ����
    public void SetLanguageType(LanguageType SetLanguageType)
    {
        languageType = SetLanguageType;
        PlayerPrefs.SetInt("LanguageSetting", (int)languageType);
        PlayerPrefs.Save();
    }

    // ���� Ȱ��ȭ�� �� ��ȯ
    public LanguageType GetCurrentLanguage()
    {
        return languageType;
    }
}
