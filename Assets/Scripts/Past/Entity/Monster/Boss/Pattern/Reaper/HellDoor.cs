using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HellDoor : MonoBehaviour
{
    [SerializeField] private int maxSpawnCnt;
    [SerializeField] private int fadeOutTime;
    [SerializeField] private int monsterLevel;
    
    [Header("한번 소환될때 사이의 간격")]
    [SerializeField] private float interval;
    [Header("한번 소환이 끝나면 그 다음 스폰까지 대기시간")]
    [SerializeField] private float nextSpawn;


    [SerializeField] private GameObject particleObject;
    [SerializeField] private DOTweenAnimation _doTweenAnimation;
    
    private List<MonsterData> monsters;
    private Coroutine SpawnRoutine;
    private Player target;
    
    public void Init(List<MonsterData> data, Player target)
    {
        monsters = data;
        this.target = target;
        
        for (int i = 0; i < monsters.Count; i++)
        {
            if(!ObjectPoolManager.Instance.IsPoolRegistered(monsters[i].EntityId))
                ObjectPoolManager.Instance.RegisterPrefab(monsters[i].EntityId, monsters[i].EntityPrefab);
        }
        
        SpawnRoutine = StartCoroutine(StartSpawn());
    }

    private IEnumerator StartSpawn()
    {
        yield return new WaitForSeconds(fadeOutTime);

        int spawnCnt = Random.Range(1, maxSpawnCnt + 1); // 총 스폰될 수

        while (spawnCnt > 0)
        {
            int curSpawn = Random.Range(1, 4); // 한 번에 소환될 수
            curSpawn = Mathf.Min(curSpawn, spawnCnt); // 남은 수보다 많이 소환하지 않도록

            for (int j = 0; j < curSpawn; j++)
            {
                int monsterNum = Random.Range(0, monsters.Count);
                Monster monster = ObjectPoolManager.Instance.SpawnObject(
                    monsters[monsterNum].EntityId,
                    transform.position,
                    Quaternion.identity).GetComponent<Monster>();

                monster.SetLevel(monsterLevel);
                monster.SetTarget(target, transform);

                yield return new WaitForSeconds(interval);
            }

            spawnCnt -= curSpawn;
            if (spawnCnt <= 0)
                break;
            
            yield return new WaitForSeconds(nextSpawn);
        }
        
        particleObject.SetActive(false);
        _doTweenAnimation.DORestartById("break");
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

}
