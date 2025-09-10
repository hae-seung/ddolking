using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using UnityEngine;
using UnityEngine.AI;

public class FeedingInteraction : InteractionBehaviour
{
    //기본 루틴은 먹이를 먹고 새끼 낳는 쿨타임 감소
    private Coroutine feedCoroutine;
    
    [Header("피딩스킨")] 
    [SerializeField] protected DamageType growType;
    [Header("새끼 낳는 시간")]
    [SerializeField] private float growTime;
    [Space(20)]
    [Header("소환시킬 오브젝트")]
    [SerializeField] private EntityData animalData;

    [Header("최상위 오브젝트 : 풀반환용")] 
    [SerializeField] private GameObject go;

    private float needGrowTime;
    
    
    public void Enable()
    {
        needGrowTime = growTime;
        
        if(feedCoroutine != null)
            StopCoroutine(feedCoroutine);

        feedCoroutine = StartCoroutine(FeedCoroutine());
    }
    
    private IEnumerator FeedCoroutine()
    {
        needGrowTime = growTime;
        
        float timer = 0f;
        while (timer < needGrowTime) // growTime이 바뀌면 즉시 반영
        {
            timer += Time.deltaTime;
            yield return null;
        }

        LayBaby();
        
        feedCoroutine = StartCoroutine(FeedCoroutine());
    }

    private void LayBaby()
    {
        if (!go.activeSelf)
            return;

        
        //새끼 오브젝트 등록
        if (!ObjectPoolManager.Instance.IsPoolRegistered(animalData.EntityId))
            ObjectPoolManager.Instance.RegisterPrefab(animalData.EntityId, animalData.EntityPrefab);

        NavMeshAgent agent = ObjectPoolManager.Instance
            .SpawnObject(animalData.EntityId, transform.position, Quaternion.identity)
            .GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
                agent.Warp(hit.position);
            else
                agent.Warp(transform.position);
        }
    }


    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        if (currentGripItem == null)
            return;
        
        if (currentGripItem.itemData is IGrowable growableItem && 
            growableItem.GetTypeCorrect(growType))
        {
            //feeding시간 단축
            //새끼를 낳는 시간 단축은 고정으로
            needGrowTime = Mathf.Max(0.1f, needGrowTime - growableItem.GetMaxGrowTime());

            //grow 스킨 출력
            DamageNumber damageNumber = DamageManager.Instance.GetDamageSkin(growType);
            damageNumber.Spawn(transform.position);
        }
    }
}
