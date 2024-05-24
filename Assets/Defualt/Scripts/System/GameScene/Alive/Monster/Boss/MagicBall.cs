using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MagicBall : Effect
{

    private enum Type
    {
        Straight,//����
        Rotate, //ȸ��
        Random
    }

    private Vector3 oriPos;
    private float speed = 5f; // �̵� �ӵ�
    private float rotateSpeed = 180f; // ȸ�� �ӵ�

    private Type thisType;
    private float lifeTime = 12f;

    private void Awake()
    {
        var param = photonView.InstantiationData;
        if (param != null && param.Length > 0)
        {
            thisType = (Type)param[0];
        }

        if(thisType == Type.Rotate) oriPos = transform.position;
    }

    void Start()
    {
          switch(thisType)
          {
              case Type.Straight:
                  Invoke("Straight", 1f);
                  break;
   
          
                  case Type.Rotate:
                  Invoke("Rotate", 1f);
                  break;
            
                  case Type.Random:
                  Invoke("RandomMove", 1f);
                  break;

          }
        Destroy(gameObject, lifeTime);
    }

    void Straight()
    {
        // 5�� ���� ������ �̵� ��, 5�� ���� �ڷ� �̵�
        StartCoroutine(StraightRoutine());
    }

    IEnumerator StraightRoutine()
    {
        Vector3 forward = transform.forward * speed * Time.deltaTime;

        for (float t = 0; t < lifeTime/2; t += Time.deltaTime)
        {
            transform.Translate(forward);
            yield return null;
        }

        Vector3 backward = -transform.forward * speed * Time.deltaTime;

        for (float t = 0; t < lifeTime/2; t += Time.deltaTime)
        {
            transform.Translate(backward);
            yield return null;
        }
    }

    void Rotate()
    {
        // oriPos�� �߽����� ȸ��
        transform.RotateAround(oriPos, Vector3.up, rotateSpeed * Time.deltaTime);
    }

    void RandomMove()
    {
        // �ֺ��� �������� �̵�
        float moveX = Random.Range(-1f, 1f) * speed * Time.deltaTime;
        float moveZ = Random.Range(-1f, 1f) * speed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, 0, moveZ));
    }

    /* private void OnTriggerEnter(Collider other)
     {
         if(other.gameObject.layer == 3)
         {
             other.gameObject.GetComponent<Alive>().TakeDamage(gameObject, pDamage,pPhy,mDamage,mPhy);
         }
     }*/
}
