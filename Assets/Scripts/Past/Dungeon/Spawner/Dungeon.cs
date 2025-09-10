using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dungeon : MonoBehaviour
{
    public abstract void SpawnBoss(GameObject boss, Transform spawnPoint, Action ClearBoss);
}
