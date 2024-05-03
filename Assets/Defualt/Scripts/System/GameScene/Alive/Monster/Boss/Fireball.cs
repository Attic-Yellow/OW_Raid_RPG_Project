using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : Effect
{
    public int initObjID; //���̾�� ������ ������Ʈ�� �� ���̵�
    public float speed;
    Vector3 targetPos;
    private void Awake()
    {
     
        var param = photonView.InstantiationData;
        if (param != null && param.Length > 0)
        {
            targetPos = (Vector3)param[0];
            initObjID = (int)param[1];
        }
    }

    private void Update()
    {

       gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, speed * Time.deltaTime);
      if(transform.position == targetPos) Destroy(gameObject);    
              
        
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<PhotonView>() == null || 
            other.GetComponent<Alive>() ==null ||
            other.GetComponent<PhotonView>().ViewID == initObjID) return;

     
            Alive aliveComponent = other.GetComponent<Alive>();
            /*foreach (Alive alive in damagedObjs)
            {
                if (aliveComponent == alive) return;
            }
            damagedObjs.Add(aliveComponent);*/
            aliveComponent.TakeDamage(gameObject, damage);
            print("�¾Ҿ�");
        
        Destroy(gameObject);
    }

    private void OnParticleSystemStopped() //��ƼŬ�� ����ɶ�
    {
        Init();
    }


    protected override void Collision(GameObject obj)
    {
      
    }
   
    protected override void Init()
    {
        damagedObjs.Clear();
        Debug.Log("��ƼŬ�� ����Ǿ� alives ����Ʈ�� Ŭ�����.");
    }

}
