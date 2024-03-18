using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefalutState : MonoBehaviour
{
    [SerializeField] protected float power; //힘
    [SerializeField] protected float luck; //운
    [SerializeField] protected float agility; //민첩
    [SerializeField] protected float intellet; //지능
    [SerializeField] protected float mentality; //정신력
    [SerializeField] protected float criticalRate; //치명타 확률
    [SerializeField] protected float hitRate; //명중률
    [SerializeField] protected float pDef; //물리 방어력
    [SerializeField] protected float mDef;// 마법 방어력
    [SerializeField] protected float castingSpeed; //기술 시전속도
    [SerializeField] protected float magicCastingSpeed;// 마법 시전속도
    [SerializeField] protected float manaRegen; //마나 회복력
    [SerializeField] protected float hpRegen; //체력 회복력
    [SerializeField] protected float damageReduc; //데미지 감소율
    [SerializeField] protected float maxHP;//최대 체력
}
