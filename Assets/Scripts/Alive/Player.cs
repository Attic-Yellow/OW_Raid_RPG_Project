using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using System;
using Cinemachine;

public class Player : Alive
{
    private string serverName;
    private string charName = "loopy";
    private CharacterController controller;
    private PhotonView photonView;
    private Transform tr;
    private CinemachineVirtualCamera cam;
    public DataManager data;
    private new void Awake()
    {
        base.Awake();
        tr = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        photonView = GetComponent<PhotonView>();
        cam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
    }
    private void Start()
    {
        if(photonView.IsMine)
        {
            data.SavePlayerData(charName, this);
            cam.Follow = transform;
            cam.LookAt = transform;
        }
    }

    private void Update()
    {
        if(photonView.IsMine) //내 캐릭터 일 경우
        Move();
     
    }
    protected override void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(x, 0f, z) * moveSpeed * Time.deltaTime;

        controller.Move(movement);
    }
}
