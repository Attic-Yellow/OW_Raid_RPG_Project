using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject startUI;
    [SerializeField] protected GameObject logInUI;
    [SerializeField] protected GameObject signUpUI;

    private void Start()
    {
        //if () // 로그인 정보가 있으면
        //{
        //    // 로그인 정보가 있으면
        //    // 로그인 정보를 통해 로그인 처리
        //}
        //else
        //{
        //    // 로그인 정보가 없으면
        //    // 로그인 화면을 보여줌
        //    ShowStartUI();
        //}

        if (startUI != null)
        {
            startUI.SetActive(true);
        }

        if (logInUI != null)
        {
            logInUI.SetActive(false);
        }

        if (signUpUI != null)
        {
            signUpUI.SetActive(false);
        }
    }

    public void GoLogInUI()
    {
        startUI.SetActive(false);
        logInUI.SetActive(true);
    }
}
