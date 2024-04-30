using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : Effect
{
    public float rotationSpeed = 50f; // 회전 속도

    private GameObject target; // 몬스터의 Transform을 저장할 변수


    void Start()
    {
        // 몬스터의 Transform을 가져와서 저장
        target = transform.parent.gameObject;
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        // 몬스터의 위치를 기준으로 회전
        if(target != null) 
        transform.RotateAround(target.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
            other.gameObject.GetComponent<Alive>().TakeDamage(gameObject, damage);
        }
    }
}
