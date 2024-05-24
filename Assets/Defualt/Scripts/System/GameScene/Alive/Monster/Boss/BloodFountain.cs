using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFountain : Effect
{
    private void OnParticleCollision(GameObject other)
    {
        /*Collision(other);*/
    }

    private void OnParticleSystemStopped() //파티클이 종료될때
    {
        Init();
    }

    /*protected override void Collision(GameObject obj)
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
        }
    }
*/
    protected override void Init()
    {
        damagedObjs.Clear();
        Debug.Log("파티클이 종료되어 alives 리스트가 클리어됨.");
        PhotonNetwork.Destroy(gameObject);
    }
}
