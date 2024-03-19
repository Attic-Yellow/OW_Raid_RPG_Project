using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Xml.Serialization;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine.UI;
using System.Linq;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;
    private readonly string version = "1.0f";
    private string userId = "user";
    public string UserID
    {
        get => userId;
        set => userId = value;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    //물리적으로 나눌뿐 로비와, 방개념과는 완전 별개로 보아야한다.

    public void NetworkSetting()
    {
        Debug.Log("네트워크 세팅");
        PhotonNetwork.AutomaticallySyncScene = true; //자동으로 같은룸의 유저에게 씬을 로딩
        PhotonNetwork.GameVersion = version; //같은버젼의 유저끼리 접속 허용
        PhotonNetwork.ConnectUsingSettings();
    }

   

    //서버에 접속하고 바로 실행되는 메서드
    public override void OnConnectedToMaster()
    {
        Debug.Log("서버에 접속");
        PhotonNetwork.JoinLobby(); //로비접속

    }

    //로비에 접속하고 바로 싱해되는 메서드
    public override void OnJoinedLobby()
    {
        Debug.Log("로비접속");
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinOrCreateRoom("RoomName", 
            new RoomOptions { MaxPlayers = 20,IsOpen = true,IsVisible = true }, null); //만들거나 입장
    }

    public override void OnJoinRoomFailed(short returnCode, string message) //룸입장이 실패했을 경우
    {
        Debug.Log($"JoinRoom Faied { returnCode} : {message}");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"방에 입장했는가? {PhotonNetwork.InRoom}");
        Debug.Log($"방에 몇명이 입장? {PhotonNetwork.CurrentRoom.PlayerCount}");
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity); //Assets/Resources 에 있는 오브젝트만 생성가능
    }
}
