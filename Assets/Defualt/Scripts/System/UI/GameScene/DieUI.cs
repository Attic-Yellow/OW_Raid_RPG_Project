using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DieUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    private int waitTime = 5;
    private int timer = 5;
    private void OnEnable()
    {
        timer = waitTime;
        StartCoroutine(TimeTextCoroutine());
    }

    private IEnumerator TimeTextCoroutine()
    {

        while(timer > 0)
        {
            timeText.text = $"{timer}초 후에 부활합니다";
            yield return new WaitForSeconds(1f);
            timer -= 1;
        }
    }

    public void YBtn()
    {
        GameManager.Instance.currentPlayerObj = PhotonNetwork.Instantiate(CharacterData.Instance.characterData["job"].ToString(),
                  GameManager.Instance.GetLastPos(), Quaternion.identity);

        // 플레이어를 따라가는 카메라 설정
        CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
        cam.Follow = GameManager.Instance.currentPlayerObj.transform.Find("PlayerCameraRoot");

        GameManager.Instance.uiManager.gameSceneUI.SetDieUI(false);
    }

    public void NBtn()
    {
        GameManager.Instance.currentPlayerObj = PhotonNetwork.Instantiate(CharacterData.Instance.characterData["job"].ToString(),
                GameManager.Instance.playerRespawnPos, Quaternion.identity);

        // 플레이어를 따라가는 카메라 설정
        CinemachineVirtualCamera cam = FindObjectOfType<CinemachineVirtualCamera>();
        cam.Follow = GameManager.Instance.currentPlayerObj.transform.Find("PlayerCameraRoot");
         GameManager.Instance.uiManager.gameSceneUI.SetDieUI(false);
    }


}
