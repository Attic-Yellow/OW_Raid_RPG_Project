using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
        public Dictionary<string, MonsterSpawnInfo> spawnInfoDIc = new();
        public MonsterSpawnInfo[] monsterSpawnInfos;

    private void Awake()
    {
        foreach(var monster in monsterSpawnInfos)
        {
            spawnInfoDIc[monster.monsterName] = monster;
        }
    }

    private void Start()
        {
            // ������ Ŭ���̾�Ʈ�� ����
            if (PhotonNetwork.IsMasterClient)
            {
                foreach (var monsterInfo in monsterSpawnInfos)
                {
                Monster monster = PhotonNetwork.Instantiate(monsterInfo.monsterPrefab.name, monsterInfo.spawnPoint.position, monsterInfo.spawnPoint.rotation).GetComponent<Monster>();
                Player[] players = FindObjectsOfType<Player>();
                foreach (var player in players)
                {
                    player.monsters.Add(monster);
                }
                
                }
            }
        }

    public void SpawnMonsterCoroutine(string monsterName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (spawnInfoDIc.ContainsKey(monsterName)) StartCoroutine(SpawnMonster(spawnInfoDIc[monsterName]));
            else print("�ش� ���� �̸��� ����");
        }
    }

        private IEnumerator SpawnMonster(MonsterSpawnInfo monsterInfo)
        {

                    yield return new WaitForSeconds(monsterInfo.respawnTime);
                    PhotonNetwork.Instantiate(monsterInfo.monsterPrefab.name, monsterInfo.spawnPoint.position, monsterInfo.spawnPoint.rotation);

        }
    
}
