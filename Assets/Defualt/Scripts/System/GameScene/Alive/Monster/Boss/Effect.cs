using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Effect : MonoBehaviourPun
{
    public float damage;
    protected List<Alive> damagedObjs = new List<Alive>(); //데미지를 입었었던
    public ParticleSystem ps;

     
    
   
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
