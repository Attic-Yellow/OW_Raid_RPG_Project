using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefalutState : MonoBehaviour
{
    [SerializeField] protected float power; //��
    [SerializeField] protected float luck; //��
    [SerializeField] protected float agility; //��ø
    [SerializeField] protected float intellet; //����
    [SerializeField] protected float mentality; //���ŷ�
    [SerializeField] protected float criticalRate; //ġ��Ÿ Ȯ��
    [SerializeField] protected float hitRate; //���߷�
    [SerializeField] protected float pDef; //���� ����
    [SerializeField] protected float mDef;// ���� ����
    [SerializeField] protected float castingSpeed; //��� �����ӵ�
    [SerializeField] protected float magicCastingSpeed;// ���� �����ӵ�
    [SerializeField] protected float manaRegen; //���� ȸ����
    [SerializeField] protected float hpRegen; //ü�� ȸ����
    [SerializeField] protected float damageReduc; //������ ������
    [SerializeField] protected float maxHP;//�ִ� ü��
    public float Power
    {
        get { return power; }
        set { power = value; }
    }

    // luck ������Ƽ
    public float Luck
    {
        get { return luck; }
        set { luck = value; }
    }

    // agility ������Ƽ
    public float Agility
    {
        get { return agility; }
        set { agility = value; }
    }

    // intellect ������Ƽ
    public float Intellect
    {
        get { return intellet; }
        set { intellet = value; }
    }

    // mentality ������Ƽ
    public float Mentality
    {
        get { return mentality; }
        set { mentality = value; }
    }

    // criticalRate ������Ƽ
    public float CriticalRate
    {
        get { return criticalRate; }
        set { criticalRate = value; }
    }

    // hitRate ������Ƽ
    public float HitRate
    {
        get { return hitRate; }
        set { hitRate = value; }
    }

    // pDef ������Ƽ
    public float PDef
    {
        get { return pDef; }
        set { pDef = value; }
    }

    // mDef ������Ƽ
    public float MDef
    {
        get { return mDef; }
        set { mDef = value; }
    }

    // castingSpeed ������Ƽ
    public float CastingSpeed
    {
        get { return castingSpeed; }
        set { castingSpeed = value; }
    }

    // magicCastingSpeed ������Ƽ
    public float MagicCastingSpeed
    {
        get { return magicCastingSpeed; }
        set { magicCastingSpeed = value; }
    }

    // manaRegen ������Ƽ
    public float ManaRegen
    {
        get { return manaRegen; }
        set { manaRegen = value; }
    }

    // hpRegen ������Ƽ
    public float HPRegen
    {
        get { return hpRegen; }
        set { hpRegen = value; }
    }

    // damageReduc ������Ƽ
    public float DamageReduc
    {
        get { return damageReduc; }
        set { damageReduc = value; }
    }

    // maxHP ������Ƽ
    public float MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
}
