using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
public class Player : Alive
{
    private CharacterController controller;
    private PhotonView photonView;
    private Transform tr;
    private new void Awake()
    {
        base.Awake();
        tr = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if(photonView.IsMine)
        Move();
        else
        {
           
        }
    }
    protected override void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // �Է� ���� ���� �̵� ���� ����
        Vector3 movement = new Vector3(x, 0f, z) * moveSpeed * Time.deltaTime;

        // ĳ���� ��Ʈ�ѷ��� ����Ͽ� �̵� ����
        controller.Move(movement);
    }
}
