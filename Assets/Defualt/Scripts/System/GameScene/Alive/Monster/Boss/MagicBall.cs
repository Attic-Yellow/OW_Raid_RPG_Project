using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagicBall : Effect
{
    public Ease thisEase;
    float distance = 30; //������� �Ÿ�
    float speed = 10; //������� �ӵ�
    float backSpeed = 5; //���ƿ��� �ӵ�
    float rotationSpeed = 50f; // ȸ�� �ӵ�
    private enum Type
    {
        Straight,//����
        Curved, //�
        Rotate, //ȸ��
        Looping,
        Cyclic, //��ȯ
        Random


    }

    private Type thisType;

    private void Awake()
    {
        var param = photonView.InstantiationData;
        if (param != null && param.Length > 0)
        {
            thisType = (Type)param[0];
        }
    }

    void Start()
    {
        /*  switch(thisType)
          {
              case Type.Straight:
                  Invoke("Straight", 1f);
                  break;
                  case Type.Curved:
                  Invoke("Curved", 1f);
                  break;
                  case Type.Rotate:
                  Invoke("Rotate", 1f);
                  break;
                  case Type.Looping:
                   Invoke("Looping", 1f);
                  break;
                  case Type.Cyclic:
                  Invoke("Cyclic", 1f);
                  break;
                  case Type.Random:
                  Invoke("Random", 1f);
                  break;

          }*/
        Straight();
        Destroy(gameObject, 10f);
    }

    void Straight() //distance��ŭ ������ٰ� ������ ��ġ�� ���ƿ���
    {

        print("����");
        transform.DOMove(transform.position + transform.forward * distance, speed)
           .SetEase(thisEase)
           .OnComplete(() => // �̵� �Ϸ� �Ŀ� ����� �۾� ����
           {
               transform.DOMove(transform.position, backSpeed) // ���� ��ġ�� �ǵ��ƿ��� �ִϸ��̼�
                   .SetEase(Ease.Linear)
                   .OnComplete(() => Destroy(gameObject));
           });

    }

    void Curved() 
    {

    }

    void Rotate()
    {

    }

    void Looping()
    {

    }

    void Cyclic()
    {

    }

     void Random()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            other.gameObject.GetComponent<Alive>().TakeDamage(gameObject, pDamage,pPhy,mDamage,mPhy);
        }
    }
}
