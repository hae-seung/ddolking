using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HellDoor", menuName = "Entity/Boss/Pattern/HellDoor")]
public class ReaperSpawnHellDoorPattern : BossPattern
{
    [Header("데미지 필요없음")] 
    [SerializeField] private List<MonsterData> monsters;
    [SerializeField] private GameObject hellDoorPrefab;
    
    
    
    protected override void ExecutePattern(BossAI boss)
    {
        HellDoor door = Instantiate(hellDoorPrefab).GetComponent<HellDoor>();
        door.transform.position = boss.target.transform.position;
        door.Init(monsters, boss.target);
    }
    
    
}
