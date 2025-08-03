using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BreakableObject : Interactable
{
    [Header("내구도 설정")]
    [SerializeField] private FieldObjectData fieldObjectData;
    private float durability;
    private float toolWear;
    private List<DropTable> dropTable = new();

    [Header("오브젝트 체력")]
    [SerializeField] private Slider durabilityBar;
    [SerializeField] private DrawOutline drawOutline;

    private const float BreakTime = 0.41f;
    private const float HoldThreshold = 0.15f;
    
    private Coroutine breakCoroutine, waitBreakCoroutine;
    private bool  isCooldown, isHolding;

    [Header("닷트윈")]
    [SerializeField] private DOTweenAnimation _doTweenAnimation;
    
    protected ReinforceStructureItem structureItem;

    public ReinforceStructureItem StructureItem => structureItem;

    protected virtual void Start() => SetData();

    private bool isActuallyBreaking;

    private void OnEnable() => ResetState();

    private void ResetState()
    {
        SetData();
        isCooldown = false;
        isHolding = false;
        isActuallyBreaking = false;
        
        
        drawOutline.onPointerExit += HandlePointerExit;
    }
    

    private void SetData()
    {
        toolWear = fieldObjectData.toolWear;
        durability = fieldObjectData.durability;
        dropTable = new List<DropTable>(fieldObjectData.dropTable);
        durabilityBar.maxValue = durability;
        durabilityBar.value = durability;
        durabilityBar.gameObject.SetActive(false);
    }

    public override void Interact(Interactor interactor, InputAction.CallbackContext context, Item currentGripItem = null)
    {
        if (context.control.name != "leftButton")
            return;
        
        if (!drawOutline.CanInteract) return;

        float holdTime = (float)(context.time - context.startTime);

        switch (context.phase)
        {
            case InputActionPhase.Started:
                isHolding = true;
                break;

            case InputActionPhase.Performed:
                HandleAttack(holdTime, currentGripItem);
                break;

            case InputActionPhase.Canceled:
                isHolding = false;
                StopBreakObject();
                break;
        }
    }

    private void HandleAttack(float holdTime, Item gripItem)
    {
        if (isCooldown)
            return;
        
        if (holdTime >= HoldThreshold)
        {
            StartBreakCoroutine(ref breakCoroutine, BreakRepeatedly(gripItem));
        }
        else
        {
            isHolding = false;
            StartBreakCoroutine(ref breakCoroutine, BreakOnce(gripItem));
        }

        isActuallyBreaking = true;
    }

    private void StartBreakCoroutine(ref Coroutine coroutine, IEnumerator routine)
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(routine);
        }
    }

    private IEnumerator BreakOnce(Item gripItem)
    {
        StartBreakState();
        BreakObject(gripItem);

        yield return new WaitForSeconds(BreakTime);

        EndBreakState();
    }

    private IEnumerator BreakRepeatedly(Item gripItem)
    {
        StartBreakState();

        while (isHolding)
        {
            BreakObject(gripItem);
            yield return new WaitForSeconds(BreakTime);
        }

        if (waitBreakCoroutine == null)
            waitBreakCoroutine = StartCoroutine(WaitBreak());
        
        breakCoroutine = null;
    }

    private IEnumerator WaitBreak()
    {
        yield return new WaitForSeconds(BreakTime);

        if (!gameObject.activeSelf)
        {
            yield break;
        }

        EndBreakState();
        waitBreakCoroutine = null;
    }

    private void StartBreakState()
    {
        isCooldown = true;
        GameEventsManager.Instance.playerEvents.PlayAnimation("isMine", true);
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
    }

    private void EndBreakState()
    {
        GameEventsManager.Instance.playerEvents.PlayAnimation("isMine", false);
        
        if(isActuallyBreaking)
            GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
        
        isCooldown = false;
        isActuallyBreaking = false;
    }

    private void BreakObject(Item gripItem)
    {
        if (durability <= 0) return;

        float damageAmount = GameEventsManager.Instance.calculatorEvents.CalculateMineDamage(Stat.MineSpeed);
        ReduceDurability(damageAmount, gripItem);
    }

    private void ReduceDurability(float amount, Item gripItem)
    {
        _doTweenAnimation?.DORestart();

        durabilityBar.gameObject.SetActive(true);
        durability = Mathf.Max(durability - amount, 0);
        durabilityBar.value = durability;

        if (durability <= 0)
        {
            durabilityBar.gameObject.SetActive(false);
            StopBreakObject();
            
            GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
            DropItems();
            DestroyFieldObject();
        }

        if (gripItem is ToolItem toolItem)
            toolItem.ReduceDurability(toolWear);
    }

    private void StopBreakObject()
    {
        isHolding = false;

        if (!gameObject.activeSelf)
            return;
        
        if (breakCoroutine != null)
        {
            StopCoroutine(breakCoroutine);
            breakCoroutine = null;
        }

        if (waitBreakCoroutine == null)
            waitBreakCoroutine = StartCoroutine(WaitBreak());
    }
    
    private void HandlePointerExit()
    {
        StopBreakObject();
    }
    
    

    private void DropItems()
    {
        foreach (var drop in dropTable)
        {
            if (!ObjectPoolManager.Instance.IsPoolRegistered(drop.DropItemId))
                ObjectPoolManager.Instance.RegisterPrefab(drop.DropItemId, drop.DropItemPrefab);
            
            
            for (int i = 0; i < Random.Range(drop.MinAmount, drop.MaxAmount + 1); i++)
            {
                Vector3 dropPosition = transform.position + new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(-0.5f, 0.5f),
                    0);
                
                DropObject dropObj = ObjectPoolManager.Instance.SpawnObject(
                    drop.DropItemId,
                    dropPosition, 
                    Quaternion.identity).GetComponent<DropObject>();
                
                if (dropObj != null && structureItem != null)
                {
                    dropObj.OverrideItem(structureItem);
                    Debug.Log($"id {structureItem.StructureId} 드랍!");
                }
                
                dropObj?.transform.DOJump(dropPosition, 1f, 1, 0.8f).SetEase(Ease.OutBounce);
            }
        }
    }

    public void DestroyFieldObject()
    {
        StopAllCoroutines();
        breakCoroutine = null;
        waitBreakCoroutine = null;

        structureItem = null; //초기화
        
        drawOutline.onPointerExit -= HandlePointerExit;

        if (!ObjectPoolManager.Instance.IsPoolRegistered(fieldObjectData.id))
        {
            ObjectPoolManager.Instance.RegisterPrefab(fieldObjectData.id, fieldObjectData.ownObject);
        }
        
        
        EndBreakState();
        ObjectPoolManager.Instance.ReleaseObject(fieldObjectData.id, gameObject);
    }

    public override void SetInteractState(bool state)
    {
        if (!state)
        {
            durabilityBar.gameObject.SetActive(false);
            StopBreakObject();
            GameEventsManager.Instance.playerEvents.PlayAnimation("isMine", false);
        }

        drawOutline.SetPlayerNear(state);
    }

   
}
