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

    private void OnParticleSystemStopped() //��ƼŬ�� ����ɶ�
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
        Debug.Log("��ƼŬ�� ����Ǿ� alives ����Ʈ�� Ŭ�����.");
        PhotonNetwork.Destroy(gameObject);
    }
}
