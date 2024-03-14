using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


// ��� ���� ������
public enum LanguageType
{
    Korean,
    English
    // ... ��Ÿ ���� ...
}

public class LanguageManager : MonoBehaviour
{
    // UI �ؽ�Ʈ�� ����ȭ�ϴ� �� ���Ǵ� Ŭ����
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

    // ��� ���� �� ȣ��
    public void LoadLocalizedText(LanguageType language)
    {
        GameManager.instance.SetLanguageType(language);
        currentLanguage = language;
        string languageFileName = language.ToString() + ".json";
        string assetBundleName = "language/json";

        // ���� ���� �ε�
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

        // ���� ���� ��ε� (�����ʹ� �޸𸮿� �ε�Ǿ� �����Ƿ�, ���� ������ �� �̻� �ʿ����� ����)
        localizedTextBundle.Unload(false);
    }


    // UI ��� ���� �� UI�ؽ�Ʈ�� ������Ʈ
    private void UpdateUI()
    {
        LocalizedText[] allLocalizedTexts = FindObjectsOfType<LocalizedText>(true);
        foreach (var localizedTextComponent in allLocalizedTexts)
        {
            localizedTextComponent.UpdateText();
        }
    }

    // Ű�� ���� ������ ������ �ؽ�Ʈ�� ������
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
