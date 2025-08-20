using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBossDoorBehaviour : StageDoorBehaviour
{
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] protected Dungeon dungeon;
    
    [Header("보스 클리어 후 나가는 문")]
    [SerializeField] private StageDoorBehaviour exitDoor;
    
    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        base.Interact(interactor, currentGripItem);
        if(canEnter)
            dungeon.SpawnBoss(bossPrefab, bossSpawnPoint, ClearBossStage);
    }

    private void ClearBossStage()
    {
        canEnter = false;
        exitDoor.ClearCurrentStage();
    }
    
}
