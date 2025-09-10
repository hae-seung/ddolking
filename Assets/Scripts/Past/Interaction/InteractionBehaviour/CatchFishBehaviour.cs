using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CatchFishBehaviour : InteractionBehaviour
{
    [SerializeField] private FieldObjectData fishTrapData;


    private Animator animator;
    private int nextCathTime;
    private bool isCatch = false;
    private string hashString = "isCatch";
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        isCatch = false;
        CalculateNextCatchTime();
    }

    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        if (!isCatch)
            return;
        
        foreach (var drop in fishTrapData.dropTable)
        {
            if (!ObjectPoolManager.Instance.IsPoolRegistered(drop.DropItemId))
                ObjectPoolManager.Instance.RegisterPrefab(drop.DropItemId, drop.DropItemPrefab);

            for (int i = 0; i < Random.Range(drop.MinAmount, drop.MaxAmount + 1); i++)
            {
                Vector3 dropPosition = interactor.transform.position;
                GameObject dropObj = ObjectPoolManager.Instance.SpawnObject(
                    drop.DropItemId,
                    dropPosition, 
                    Quaternion.identity);

                dropObj?.transform.DOJump(dropPosition, 1f, 1, 0.8f).SetEase(Ease.OutBounce);
            }
        }

        isCatch = false;
        CalculateNextCatchTime();
    }

    private void CatchFish()
    {
        isCatch = true;
        animator.SetBool(hashString, isCatch);
    }

    private void CalculateNextCatchTime()
    {
        animator.SetBool(hashString, isCatch);
        nextCathTime  = Random.Range(5, 6);
        Invoke(nameof(CatchFish), nextCathTime);
    }
}
