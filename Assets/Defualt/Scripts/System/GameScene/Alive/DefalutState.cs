using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DefalutState : MonoBehaviourPun
{
    [Header("Status")]
    //스테이터스들은 스크랩터블로 관리하는게 좋아보임
    [SerializeField] protected string thisName;
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
    [SerializeField] protected float level;
    [SerializeField] protected int id;
    /*  플레이어 1
        몬스터 10 ~ 199
        NPC 200~ 299
        아이템 300~ 500*/
    public float Level { get => level; set => level = value; }
    public int ID
    {
              get { return id; }
      private  set { id = value; }
    }
    public string Name
    {
        get { return thisName; }
        set { thisName = value; }
    }
    public float Power
    {
        get { return power; }
        set { power = value; }
    }

    // luck 프로퍼티
    public float Luck
    {
        get { return luck; }
        set { luck = value; }
    }

    // agility 프로퍼티
    public float Agility
    {
        get { return agility; }
        set { agility = value; }
    }

    // intellect 프로퍼티
    public float Intellect
    {
        get { return intellet; }
        set { intellet = value; }
    }

    // mentality 프로퍼티
    public float Mentality
    {
        get { return mentality; }
        set { mentality = value; }
    }

    // criticalRate 프로퍼티
    public float CriticalRate
    {
        get { return criticalRate; }
        set { criticalRate = value; }
    }

    // hitRate 프로퍼티
    public float HitRate
    {
        get { return hitRate; }
        set { hitRate = value; }
    }

    // pDef 프로퍼티
    public float PDef
    {
        get { return pDef; }
        set { pDef = value; }
    }

    // mDef 프로퍼티
    public float MDef
    {
        get { return mDef; }
        set { mDef = value; }
    }

    // castingSpeed 프로퍼티
    public float CastingSpeed
    {
        get { return castingSpeed; }
        set { castingSpeed = value; }
    }

    // magicCastingSpeed 프로퍼티
    public float MagicCastingSpeed
    {
        get { return magicCastingSpeed; }
        set { magicCastingSpeed = value; }
    }

    // manaRegen 프로퍼티
    public float ManaRegen
    {
        get { return manaRegen; }
        set { manaRegen = value; }
    }

    // hpRegen 프로퍼티
    public float HPRegen
    {
        get { return hpRegen; }
        set { hpRegen = value; }
    }

    // damageReduc 프로퍼티
    public float DamageReduc
    {
        get { return damageReduc; }
        set { damageReduc = value; }
    }

    // maxHP 프로퍼티
    public float MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
}
