using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonTest : MonoBehaviourPunCallbacks
{

    public GameObject playerResource;
    void Awake()
    {
        // Photon 서버에 연결
        DontDestroyOnLoad(gameObject);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server!");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join random room. Creating a new room...");

        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room " + PhotonNetwork.CurrentRoom.Name);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        PhotonNetwork.LoadLevel("Test");

        // 씬이 로드될 때까지 대기
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Test");

        GameObject player = PhotonNetwork.Instantiate(playerResource.name, Vector3.zero, Quaternion.identity);
        Camera.main.transform.SetParent(player.transform.Find("CamPos"));
        Camera.main.transform.localPosition = Vector3.zero;
        Camera.main.transform.localRotation = Quaternion.identity;
    }
}
