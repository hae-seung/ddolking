using System;
using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class GrowAnimal : InteractionBehaviour
{
    private Coroutine growCoroutine;
    
    [Header("성장이펙트스킨")] 
    [SerializeField] private DamageType growType;
    [Header("자라는 시간")]
    [SerializeField] private float growTime;
    [Space(20)]
    [Header("소환시킬 오브젝트")]
    [SerializeField] private EntityData animalData;

    [Header("자신 데이터")] 
    [SerializeField] private EntityData data;
    
    [Header("최상위 오브젝트 : 풀반환용")] 
    [SerializeField] private GameObject go;

    private float needGrowTime;
    
    
    public void Enable()
    {
        needGrowTime = growTime;
        
        if(growCoroutine != null)
            StopCoroutine(growCoroutine);

        growCoroutine = StartCoroutine(GrowUpCoroutine());
    }
    
    private IEnumerator GrowUpCoroutine()
    {
        float timer = 0f;
        while (timer < needGrowTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        
        GrowUp();
    }

    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        if (currentGripItem == null)
            return;
        
        if (currentGripItem.itemData is IGrowable growableItem && 
            growableItem.GetTypeCorrect(growType))
        {
            //grow시간 단축
            float reduceTime = Random.Range(1f, growableItem.GetMaxGrowTime());
            needGrowTime = Mathf.Max(0.1f, needGrowTime - reduceTime);

            //grow 스킨 출력
            DamageNumber damageNumber = DamageManager.Instance.GetDamageSkin(growType);
            damageNumber.Spawn(transform.position);
        }
    }
    
    private void GrowUp()
    {
        if (!go.activeSelf)
            return;
        
        //부모 오브젝트 등록
        if(!ObjectPoolManager.Instance.IsPoolRegistered(animalData.EntityId))
           ObjectPoolManager.Instance.RegisterPrefab(animalData.EntityId, animalData.EntityPrefab);

        //자기 자신 오브젝트 등록
        if(!ObjectPoolManager.Instance.IsPoolRegistered(data.EntityId))
            ObjectPoolManager.Instance.RegisterPrefab(data.EntityId, data.EntityPrefab);
        
        NavMeshAgent agent = ObjectPoolManager.Instance.SpawnObject(
            animalData.EntityId,
            transform.position,
            Quaternion.identity).GetComponent<NavMeshAgent>();

        agent.Warp(transform.position);
        
        
        //자기자신 오브젝트 릴리즈
        ObjectPoolManager.Instance.ReleaseObject(data.EntityId, go);
    }
}
