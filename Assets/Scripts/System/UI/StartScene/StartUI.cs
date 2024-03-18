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
        //if () // �α��� ������ ������
        //{
        //    // �α��� ������ ������
        //    // �α��� ������ ���� �α��� ó��
        //}
        //else
        //{
        //    // �α��� ������ ������
        //    // �α��� ȭ���� ������
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
