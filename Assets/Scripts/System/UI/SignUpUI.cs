using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks; // �񵿱� �۾��� ���� Task�� ����ϱ� ���� �߰�
using TMPro;
using System.Text.RegularExpressions;

public class SignUpUI : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public GameObject logInUI;
    public GameObject signUpUI;

    public void OnSignUpButtonClicked()
    {
        if (!Regex.IsMatch(passwordInputField.text, @"[!@#$%^&*(),.?"":{}|<>]") || passwordInputField.text.Length < 8)
        {
            // Ư�����ڰ� ���Ե��� ���� ��� ���� �޽��� ���
            Debug.LogError("Ư������ �����ؼ� 8�ڸ� �̻� ���� ��");
            return; // ���⼭ �Լ� ������ �ߴ��մϴ�.
        }

        // CreateUserWithEmailAndPasswordAsync ȣ�� ����� ó���ϴ� �񵿱� �޼���
        RegisterUser(emailInputField.text, passwordInputField.text);
    }

    private async void RegisterUser(string email, string password)
    {
        try
        {
            // �񵿱� �۾��� ��ٸ��鼭 ����� AuthResult Ÿ������ �޽��ϴ�.
            AuthResult result = await GameManager.instance.firebaseManager.auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseUser newUser = result.User; // AuthResult���� FirebaseUser�� �����մϴ�.

            if (newUser != null)
            {
                // ȸ������ ����
                Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                logInUI.SetActive(true);
                signUpUI.SetActive(false);
            }
        }
        catch (System.Exception e)
        {
            // ���� ó��
            Debug.LogError("SignUp Failed: " + e.Message);
        }
    }
}
