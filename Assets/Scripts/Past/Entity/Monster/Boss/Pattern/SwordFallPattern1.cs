using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "SwordFallPattern1", menuName = "Entity/Boss/Pattern/SwordFall1")]
public class SwordFallPattern1 : BossPattern
{ 
    [SerializeField] private int minSpawnCount = 1;           // 최소 소환 개수
    [SerializeField] private int maxSpawnCount = 5;           // 최대 소환 개수

    protected override void ExecutePattern(BossAI boss)
    {
        List<SwordPos> swordPosPatterns = SpringSpawner.Instance.swordPosPatterns1;
        
        // 이번에 소환할 개수 (랜덤)
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            // SwordPos 리스트 중 하나 랜덤 선택
            SwordPos posPattern = swordPosPatterns[Random.Range(0, swordPosPatterns.Count)];

            // 랜덤 위치 가져오기
            Vector3 spawnPos = GetRandomSpawnPos(posPattern);

            // 칼 소환
            SwordFall sword = Instantiate(
                    posPattern.sword, 
                    spawnPos, 
                    posPattern.sword.transform.rotation)
                .GetComponent<SwordFall>();

            sword.SetTarget(boss.target, damage);
        }
    }

    /// <summary>
    /// SwordPos의 범위 안에서 랜덤 위치 반환
    /// </summary>
    private Vector3 GetRandomSpawnPos(SwordPos posPattern)
    {
        Vector3 min = posPattern.spawnMinPos.position;
        Vector3 max = posPattern.spawnMaxPos.position;

        float randX = min.x;
        float randY = min.y;

        if (Mathf.Approximately(min.x, max.x))
        {
            // X 동일 → Y만 랜덤
            randY = Random.Range(min.y, max.y);
        }
        else if (Mathf.Approximately(min.y, max.y))
        {
            // Y 동일 → X만 랜덤
            randX = Random.Range(min.x, max.x);
        }

        return new Vector3(randX, randY, 0f);
    }
}