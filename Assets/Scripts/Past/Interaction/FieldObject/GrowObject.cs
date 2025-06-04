using System;
using System.Collections;
using DamageNumbersPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class GrowObject : InteractionBehaviour
{
    private BreakableObject _breakableObject;
    private Coroutine growCoroutine;

    [Header("성장이펙트스킨")] 
    [SerializeField] private DamageType growType;
    [Header("자라는 시간")]
    [SerializeField] private float growTime;
    [Space(20)]
    [Header("소환시킬 필드오브젝트")]
    [SerializeField] private FieldObjectData spawnFieldObject;

    private float tempGrowTime;
    
    
    private void Awake()
    {
        tempGrowTime = growTime;
        _breakableObject = GetComponent<BreakableObject>();
    }

    private void OnEnable()
    {
        growTime = tempGrowTime;
        
        if(growCoroutine != null)
            StopCoroutine(growCoroutine);

        growCoroutine = StartCoroutine(GrowUpCoroutine());
    }
    

    private void GrowUp()
    {
        if (!gameObject.activeSelf)
            return;
        if (!ObjectPoolManager.Instance.IsPoolRegistered(spawnFieldObject.id))
        {
            ObjectPoolManager.Instance.RegisterPrefab(spawnFieldObject.id, spawnFieldObject.ownObject);
        }
        
        
        ObjectPoolManager.Instance.SpawnObject(
            spawnFieldObject.id,
            transform.position,
            Quaternion.identity);
        
        _breakableObject.DestroyFieldObject();
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
            growTime = Mathf.Max(0.1f, growTime - reduceTime);

            //grow 스킨 출력
            DamageNumber damageNumber = DamageManager.Instance.GetDamageSkin(growType);
            damageNumber.Spawn(transform.position);
        }
    }

    private IEnumerator GrowUpCoroutine()
    {
        float timer = 0f;
        while (timer < growTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        
        GrowUp();
    }
    
    
}
