using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ServerSelect : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public Toggle[] toggles; // ��� �迭
    private string playerPrefsKey = "SelectedServer"; // PlayerPrefs Ű ��

    void Start()
    {
        // PlayerPrefs���� ����� �� �ҷ�����
        int selectedIndex = PlayerPrefs.GetInt(playerPrefsKey, 0);

        // ���õ� ������ ����� Ȱ��ȭ
        toggles[selectedIndex].isOn = true;
    }

    public void OnToggleSelected()
    {
        // ���õ� ����� �ε��� ����
        int selectedIndex = Array.IndexOf(toggles, toggleGroup.ActiveToggles().FirstOrDefault());
        PlayerPrefs.SetInt(playerPrefsKey, selectedIndex);
    }
}
