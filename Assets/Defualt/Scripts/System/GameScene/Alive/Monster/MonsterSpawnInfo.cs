using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterSpawnInfo
{
    public GameObject monsterPrefab;
    public string monsterName;
    public Transform spawnPoint;
    public float respawnTime;
}