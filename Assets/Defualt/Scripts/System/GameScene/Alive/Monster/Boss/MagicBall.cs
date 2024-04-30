using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : Effect
{
    public float rotationSpeed = 50f; // ȸ�� �ӵ�

    private GameObject target; // ������ Transform�� ������ ����


    void Start()
    {
        // ������ Transform�� �����ͼ� ����
        target = transform.parent.gameObject;
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        // ������ ��ġ�� �������� ȸ��
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
