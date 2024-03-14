using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


// 언어 종류 열거형
public enum LanguageType
{
    Korean,
    English
    // ... 기타 언어들 ...
}

public class LanguageManager : MonoBehaviour
{
    // UI 텍스트를 직력화하는 데 사용되는 클래스
    [System.Serializable]
    public class UITexts
    {
        public Dictionary<string, string> uiTexts = new Dictionary<string, string>();
    }

    [SerializeField] private UITexts uiTexts;
    [SerializeField] private LanguageType currentLanguage;
    // [SerializeField] private CSVTranslator scvTranslator;

    private void Awake()
    {
        GameManager.instance.languageManager = this;
        currentLanguage = GameManager.instance.GetCurrentLanguage();
        LoadLocalizedText(currentLanguage);
    }

    // 언어 변경 시 호출
    public void LoadLocalizedText(LanguageType language)
    {
        GameManager.instance.SetLanguageType(language);
        currentLanguage = language;
        string languageFileName = language.ToString() + ".json";
        string assetBundleName = "language/json";

        // 에셋 번들 로드
        AssetBundle localizedTextBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundleName));
        if (localizedTextBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle for language: " + language.ToString());
            return;
        }

        TextAsset languageTextAsset = localizedTextBundle.LoadAsset<TextAsset>(languageFileName);
        if (languageTextAsset != null)
        {
            string dataAsJson = languageTextAsset.text;
            uiTexts = JsonConvert.DeserializeObject<UITexts>(dataAsJson);
            UpdateUI();
        }
        else
        {
            Debug.LogError("Cannot find language file in AssetBundle: " + languageFileName);
        }

        // 에셋 번들 언로드 (데이터는 메모리에 로드되어 있으므로, 에셋 번들은 더 이상 필요하지 없음)
        localizedTextBundle.Unload(false);
    }


    // UI 언어 변경 시 UI텍스트를 업데이트
    private void UpdateUI()
    {
        LocalizedText[] allLocalizedTexts = FindObjectsOfType<LocalizedText>(true);
        foreach (var localizedTextComponent in allLocalizedTexts)
        {
            localizedTextComponent.UpdateText();
        }
    }

    // 키를 통해 선택한 국가의 텍스트를 가져옴
    public string GetLocalizedValue(string key)
    {
        if (uiTexts.uiTexts != null && uiTexts.uiTexts.TryGetValue(key, out string value))
        {
            return value;
        }
        return "Missing Text";
    }

    public void ChangeLanguage(LanguageType newLanguage)
    {
        LoadLocalizedText(newLanguage);
    }
}
