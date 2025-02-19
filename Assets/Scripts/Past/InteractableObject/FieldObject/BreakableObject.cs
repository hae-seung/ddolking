using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class BreakableObject : Interactable
{
    [Header("외곽선 설정")]
    private Color color = Color.red;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private int outlineSize = 1;

    private bool isPlayerNear = false;
    private bool isPointerHovering = false;
    private bool canInteract = false;
    
    [Header("내구도 설정")]
    public FieldObjectData fieldObjectData;
    private float durability;//오브젝트 체력
    private float toolWear;//감소시킬 내구력
    private List<DropTable> dropTable = new();
    
    
    [Header("오브젝트 체력")]
    public Slider durabilityBar;

    private void Start()
    {
        toolWear = fieldObjectData.toolWear;
        durability = fieldObjectData.durability;
        dropTable = new List<DropTable>(fieldObjectData.dropTable);
        durabilityBar.maxValue = durability;
        durabilityBar.minValue = 0;
        durabilityBar.value = durability;
        durabilityBar.gameObject.SetActive(false);
    }

    public override void Interact(Interactor interactor, InputAction.CallbackContext context)
    {
        if (canInteract && context.control.device is Mouse) //좌클릭 누름 의미
        {
            GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
            BreakObject(interactor);
            GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
        }
    }

    private void BreakObject(Interactor interactor)
    {
        if (durability > 0)
        {
            float damageAmount = 10;//todo: 계산기로 수정
            ReduceDurability(damageAmount);
        }
    }

    private void ReduceDurability(float amount)
    {
        durabilityBar.gameObject.SetActive(true);
        
        durability = Mathf.Max(durability - amount, 0);
        durabilityBar.value = durability; // Slider 값 업데이트
        
        if (durability <= 0)
        {
            durabilityBar.gameObject.SetActive(false);
            DropItems();
            DestroyFieldObject();
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
                Vector3 dropPosition = transform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
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

    private void DestroyFieldObject()
    {
        ObjectPoolManager.Instance.ReleaseObject(fieldObjectData.id, gameObject);
    }

    public override void SetInteractState(bool state)
    {
        isPlayerNear = state;
        
        if(!isPlayerNear)
            durabilityBar.gameObject.SetActive(false);
        
        if(isPointerHovering)
        {
            UpdateOutline(state);
            canInteract = state;
        }
    }

    private void OnMouseEnter()
    {
        isPointerHovering = true;
        
        if (isPlayerNear)
        {
            UpdateOutline(true);
            canInteract = true;
        }
    }

    private void OnMouseExit()
    {
        isPointerHovering = false;
        
        UpdateOutline(false);
        canInteract = false;
    }

    private void UpdateOutline(bool outline) 
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Outline", outline ? 1f : 0);
        mpb.SetColor("_OutlineColor", color);
        mpb.SetFloat("_OutlineSize", outlineSize);
        spriteRenderer.SetPropertyBlock(mpb);
    }
}
