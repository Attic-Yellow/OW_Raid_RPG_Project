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

    //���������� ������ �κ��, �氳����� ���� ������ ���ƾ��Ѵ�.

    public void NetworkSetting()
    {
        Debug.Log("��Ʈ��ũ ����");
        PhotonNetwork.AutomaticallySyncScene = true; //�ڵ����� �������� �������� ���� �ε�
        PhotonNetwork.GameVersion = version; //���������� �������� ���� ���
        PhotonNetwork.ConnectUsingSettings();
    }

   

    //������ �����ϰ� �ٷ� ����Ǵ� �޼���
    public override void OnConnectedToMaster()
    {
        Debug.Log("������ ����");
        PhotonNetwork.JoinLobby(); //�κ�����

    }

    //�κ� �����ϰ� �ٷ� ���صǴ� �޼���
    public override void OnJoinedLobby()
    {
        Debug.Log("�κ�����");
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinOrCreateRoom("RoomName", 
            new RoomOptions { MaxPlayers = 20,IsOpen = true,IsVisible = true }, null); //����ų� ����
    }

    public override void OnJoinRoomFailed(short returnCode, string message) //�������� �������� ���
    {
        Debug.Log($"JoinRoom Faied { returnCode} : {message}");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"�濡 �����ߴ°�? {PhotonNetwork.InRoom}");
        Debug.Log($"�濡 ����� ����? {PhotonNetwork.CurrentRoom.PlayerCount}");
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity); //Assets/Resources �� �ִ� ������Ʈ�� ��������
    }
}
