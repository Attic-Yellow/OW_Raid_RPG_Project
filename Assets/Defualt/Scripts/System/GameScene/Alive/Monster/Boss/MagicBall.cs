using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagicBall : Effect
{
    public Ease thisEase;
    float distance = 30; //뻗어나가는 거리
    float speed = 10; //뻗어나가는 속도
    float backSpeed = 5; //돌아오는 속도
    float rotationSpeed = 50f; // 회전 속도
    private enum Type
    {
        Straight,//직진
        Curved, //곡선
        Rotate, //회전
        Looping,
        Cyclic, //순환
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

    void Straight() //distance만큼 뻗어나갔다가 원래의 위치로 돌아오게
    {

        print("직진");
        transform.DOMove(transform.position + transform.forward * distance, speed)
           .SetEase(thisEase)
           .OnComplete(() => // 이동 완료 후에 실행될 작업 설정
           {
               transform.DOMove(transform.position, backSpeed) // 원래 위치로 되돌아오는 애니메이션
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
