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
            // 마스터 클라이언트만 실행
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
                // 몬스터가 죽은 경우에만 생성
                if (monsterInfo.spawnPoint.childCount == 0)
                {
                    // 일정 시간 대기 후 몬스터 생성
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
