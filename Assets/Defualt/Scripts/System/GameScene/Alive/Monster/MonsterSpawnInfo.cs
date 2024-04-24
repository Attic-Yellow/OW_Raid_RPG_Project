using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterSpawnInfo
{
    public GameObject monsterPrefab;
    public Transform spawnPoint;
    public float respawnTime = 10f;
}