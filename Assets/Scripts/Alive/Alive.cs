using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alive : DefalutState
{
    [SerializeField] protected float currentHP; //����ü��
    [SerializeField] protected float moveSpeed; //�̵��ӵ�
    [SerializeField] protected float baseDamage; //�⺻���ݷ�
    [SerializeField] protected float baseDefence; //�⺻����
    [SerializeField] protected float aggroLevel; //������
    protected Animator animator;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Die()
    {

    }

    protected virtual void TakeDamage(float damage)
    {

    }

    protected virtual void Move()
    {

    }

    protected virtual void ReplenishHPAndMana()
    {

    }
}
