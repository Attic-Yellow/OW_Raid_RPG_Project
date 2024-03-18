using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ServerSelect : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public Toggle[] toggles; // 토글 배열
    private string playerPrefsKey = "SelectedServer"; // PlayerPrefs 키 값

    void Start()
    {
        // PlayerPrefs에서 저장된 값 불러오기
        int selectedIndex = PlayerPrefs.GetInt(playerPrefsKey, 0);

        // 선택된 서버의 토글을 활성화
        toggles[selectedIndex].isOn = true;
    }

    public void OnToggleSelected()
    {
        // 선택된 토글의 인덱스 저장
        int selectedIndex = Array.IndexOf(toggles, toggleGroup.ActiveToggles().FirstOrDefault());
        PlayerPrefs.SetInt(playerPrefsKey, selectedIndex);
    }
}
