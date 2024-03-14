using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string localizationKey;

    // UI ��� ���� �� �ؽ�Ʈ�� ������Ʈ
    public void UpdateText()
    {
        if (GameManager.instance.languageManager != null)
        {
            string localized = GameManager.instance.languageManager.GetLocalizedValue(localizationKey);
            GetComponent<TextMeshProUGUI>().text = localized;
        }
    }
}
