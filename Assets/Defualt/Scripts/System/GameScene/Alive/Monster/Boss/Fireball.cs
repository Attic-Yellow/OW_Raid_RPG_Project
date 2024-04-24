using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fireball : Effect
{
    Vector3 originalPos;
    public float speed;
    Vector3 targetPos;
    private void Awake()
    {
        targetPos = FindObjectOfType<Boss>().target.transform.position;
        ps = GetComponent<ParticleSystem>();
        originalPos = gameObject.transform.position;
    }

    private void Update()
    {

       gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, speed * Time.deltaTime);
      if(transform.position == originalPos) Destroy(gameObject);    
              
        
    }
    private void OnParticleCollision(GameObject other)
    {
        Collision(other);
    }

    private void OnParticleSystemStopped() //��ƼŬ�� ����ɶ�
    {
        Init();
    }


    protected override void Collision(GameObject obj)
    {
        if (obj.GetComponent<Alive>() != null)
        {
            Alive aliveComponent = obj.GetComponent<Alive>();
            foreach (Alive alive in damagedObjs)
            {
                if (aliveComponent == alive) return;
            }
            damagedObjs.Add(aliveComponent);
            aliveComponent.TakeDamage(gameObject, damage);
            print("�¾Ҿ�");
        }
       Destroy(gameObject);
    }
   
    protected override void Init()
    {
        ps.gameObject.transform.position = originalPos;
        damagedObjs.Clear();
        Debug.Log("��ƼŬ�� ����Ǿ� alives ����Ʈ�� Ŭ�����.");
    }

}
