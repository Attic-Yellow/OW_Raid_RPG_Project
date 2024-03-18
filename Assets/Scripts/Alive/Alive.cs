using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alive : DefalutState
{
    [SerializeField] protected float currentHP; //현재체력
    [SerializeField] protected float moveSpeed; //이동속도
    [SerializeField] protected float baseDamage; //기본공격력
    [SerializeField] protected float baseDefence; //기본방어력
    [SerializeField] protected float aggroLevel; //적개심
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
