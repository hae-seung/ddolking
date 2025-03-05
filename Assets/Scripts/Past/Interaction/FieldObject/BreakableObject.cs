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
    public FieldObjectData fieldObjectData;
    private float durability;
    private float toolWear;
    private List<DropTable> dropTable = new();

    [Header("오브젝트 체력")]
    public Slider durabilityBar;

    [SerializeField] private DrawOutline drawOutline; //  외곽선 스크립트 참조
    [SerializeField] private float breakTime;
    private Coroutine breakCoroutine;

    [Header("닷트윈")] 
    [SerializeField] private DOTweenAnimation _doTweenAnimation;

    private void Start()
    {
        SetData();
    }

    private void OnEnable()
    {
        SetData();
    }

    private void SetData()
    {
        toolWear = fieldObjectData.toolWear;
        durability = fieldObjectData.durability;
        dropTable = new List<DropTable>(fieldObjectData.dropTable);
        durabilityBar.maxValue = durability;
        durabilityBar.minValue = 0;
        durabilityBar.value = durability;
        durabilityBar.gameObject.SetActive(false);
    }

    public override void Interact(Interactor interactor, InputAction.CallbackContext context, Item currentGripItem = null)
    {
        if (!drawOutline.CanInteract) return;

        if (context.control.name.Equals("leftButton"))
        {
            if (context.started) // 한 번 클릭
            {
                GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
                BreakObject(currentGripItem); // 한 번 실행
                GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
            }
            else if (context.performed && breakCoroutine == null) // `Hold Time`이 지나서 실제로 꾹 눌림
            {
                breakCoroutine = StartCoroutine(BreakObjectRepeatedly(currentGripItem));
            }
            else if (context.canceled) // 마우스를 떼면 반복 실행 중지
            {
                StopBreakObject();
                GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
            }
        }
    }

    // 0.5초마다 BreakObject 호출하는 코루틴 (즉시 실행 후 반복)
    private IEnumerator BreakObjectRepeatedly(Item gripItem)
    {
        while (true) // 코루틴이 취소될 때까지 무한 루프
        {
            BreakObject(gripItem);
            yield return new WaitForSeconds(breakTime);
        }
    }

    // 마우스를 떼면 반복 실행 중지
    private void StopBreakObject()
    {
        if (breakCoroutine != null)
        {
            StopCoroutine(breakCoroutine);
            breakCoroutine = null;
        }
    }

    // 오브젝트 체력 감소
    private void BreakObject(Item gripItem)
    {
        if (durability > 0)
        {
            float damageAmount = GameEventsManager.Instance.calculatorEvents.CalculateMineDamage(Stat.MineSpeed);
            ReduceDurability(damageAmount, gripItem);
        }
    }

    
    private void ReduceDurability(float amount, Item gripItem)
    {
        if(_doTweenAnimation!= null)
        {
            _doTweenAnimation.DORestart();
        }
        
        
        durabilityBar.gameObject.SetActive(true);

        durability = Mathf.Max(durability - amount, 0);
        durabilityBar.value = durability;
        
        if (durability <= 0)
        {
            durabilityBar.gameObject.SetActive(false);
            DropItems();
            DestroyFieldObject();
            GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
        }

        if (gripItem != null && gripItem is ToolItem toolItem)
        {
            toolItem.ReduceDurability(toolWear);
        }
        
    }

    private void DropItems()
    {
        foreach (var drop in dropTable)
        {
            if (!ObjectPoolManager.Instance.IsPoolRegistered(drop.DropItemId))
            {
                ObjectPoolManager.Instance.RegisterPrefab(drop.DropItemId, drop.DropItemPrefab);
            }

            for (int i = 0; i < Random.Range(drop.MinAmount, drop.MaxAmount + 1); i++)
            {
                Vector3 dropPosition = transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
                GameObject dropObj = ObjectPoolManager.Instance.SpawnObject(
                    drop.DropItemId,
                    dropPosition,
                    Quaternion.identity);

                if (dropObj != null)
                {
                    dropObj.transform.DOJump(dropPosition, 1f, 1, 0.8f).SetEase(Ease.OutBounce);
                }
                else
                {
                    Debug.LogError($"ID {drop.DropItemId}의 드랍 아이템을 생성할 수 없습니다!");
                }
            }
        }
    }

    public void DestroyFieldObject()
    {
        ObjectPoolManager.Instance.ReleaseObject(fieldObjectData.id, gameObject);
    }

    public override void SetInteractState(bool state)
    {
        if(!state)
        {
            durabilityBar.gameObject.SetActive(false);
            StopBreakObject();
        }
        
        drawOutline.SetPlayerNear(state);
    }
}
