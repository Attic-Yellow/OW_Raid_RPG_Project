using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Effect : MonoBehaviourPun
{
    public float pDamage; //물리공격력
    public float pPhy; //물리관통력
    public float mDamage; //마법공격력
    public float mPhy; //마법 관통력

    protected List<Alive> damagedObjs = new List<Alive>(); //데미지를 입었었던
    public ParticleSystem ps;
    public int createCount;
     
    
   
    protected virtual void PlayPs()
    {

    }
    public virtual void PlayPs(GameObject obj)
    {

    }
    protected virtual void Collision(GameObject obj)
    {

    }
  
    protected virtual void Init()
    {

    }
}
