using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string localizationKey;

    // UI 언어 변경 시 텍스트를 업데이트
    public void UpdateText()
    {
        if (GameManager.instance.languageManager != null)
        {
            string localized = GameManager.instance.languageManager.GetLocalizedValue(localizationKey);
            GetComponent<TextMeshProUGUI>().text = localized;
        }
    }
}
