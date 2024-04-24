using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadSphere : Effect
{
    bool isGround = false;
    Boss boss;
    [SerializeField] float gravity = 9.8f; // 중력 가속도
    [SerializeField] float moveSpeed = 2f;
    public Color psStartColor;
    

    private void Awake()
    {

        var param = photonView.InstantiationData;
        if (param != null && param.Length > 0)
        {
            Vector3 colorVec = (Vector3)param[0];
            psStartColor = new Color(colorVec.x, colorVec.y, colorVec.z);
            if (psStartColor != null)
            {
                ps.startColor = psStartColor;
            }
            else print("null");
        }

     }
   

    private void Update()
    {

        if (isGround == false)
        {
            transform.Translate(Vector3.down * gravity * Time.deltaTime);
        }
        else if (boss != null)
        {
            Vector3 directionToBoss = (boss.transform.position - transform.position).normalized;
            transform.forward = directionToBoss;
            // 보스 쪽으로 일정한 속도로 다가감
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == 6 && isGround == false)
        {
            isGround = true;
            boss = FindObjectOfType<Boss>();

            return;
        }
        if (isGround == false) return;

        if (other.gameObject.CompareTag("Boss"))
        {
            StartCoroutine(CollisionBoss());

        }
        else if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(CollisionPlayer());
        }


    }

    IEnumerator CollisionBoss()
    {
        yield return new WaitUntil(() => boss.IsOkColorCheck(psStartColor));
        Destroy(gameObject);
    }

    private IEnumerator CollisionPlayer()
    {
        yield return new WaitUntil(() => boss.ColorQCheck(psStartColor));

     
        Destroy(gameObject);
    }







}
