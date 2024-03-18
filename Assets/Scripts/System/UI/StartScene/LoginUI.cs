using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class LoginUI : LogInInfo
{
    public void OnLoginButtonClicked()
    {
        GameManager.instance.firebaseManager.auth.SignInWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text).ContinueWith(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Login Failed.");
                // UI�� ���� �޽��� ǥ�� ��
            }
            else
            {
                // �α��� ���� ó��
                Debug.Log("Logged in successfully.");
            }
        });
    }

    public void GoSignUpUI()
    {
        signUpUI.SetActive(true);
        logInUI.SetActive(false);
    }
}