using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

        public MonsterSpawnInfo[] monsterSpawnInfos;

        private void Start()
        {
            // ������ Ŭ���̾�Ʈ�� ����
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (var monsterInfo in monsterSpawnInfos)
                {
                    StartCoroutine(SpawnMonster(monsterInfo));
                }
            }
        }

        private IEnumerator SpawnMonster(MonsterSpawnInfo monsterInfo)
        {
            while (true)
            {
                // ���Ͱ� ���� ��쿡�� ����
                if (monsterInfo.spawnPoint.childCount == 0)
                {
                    // ���� �ð� ��� �� ���� ����
                    yield return new WaitForSeconds(monsterInfo.respawnTime);
                    PhotonNetwork.Instantiate(monsterInfo.monsterPrefab.name, monsterInfo.spawnPoint.position, monsterInfo.spawnPoint.rotation);
                }
                else
                {
                    yield return null;
                }
            }
        }
    
}
