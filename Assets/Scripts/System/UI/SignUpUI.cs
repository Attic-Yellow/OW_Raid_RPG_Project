using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks; // 비동기 작업에 대한 Task를 사용하기 위해 추가
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
            // 특수문자가 포함되지 않은 경우 에러 메시지 출력
            Debug.LogError("특수문자 포함해서 8자리 이상 만들 것");
            return; // 여기서 함수 실행을 중단합니다.
        }

        // CreateUserWithEmailAndPasswordAsync 호출 결과를 처리하는 비동기 메서드
        RegisterUser(emailInputField.text, passwordInputField.text);
    }

    private async void RegisterUser(string email, string password)
    {
        try
        {
            // 비동기 작업을 기다리면서 결과를 AuthResult 타입으로 받습니다.
            AuthResult result = await GameManager.instance.firebaseManager.auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseUser newUser = result.User; // AuthResult에서 FirebaseUser를 추출합니다.

            if (newUser != null)
            {
                // 회원가입 성공
                Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                logInUI.SetActive(true);
                signUpUI.SetActive(false);
            }
        }
        catch (System.Exception e)
        {
            // 에러 처리
            Debug.LogError("SignUp Failed: " + e.Message);
        }
    }
}
