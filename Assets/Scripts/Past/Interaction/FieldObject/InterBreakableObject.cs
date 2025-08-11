using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class InterBreakableObject : BreakableObject
{
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private InteractionBehaviour interactionBehaviour;


    private EstablishItem item;
    

    protected override void Start()
    {
        base.Start();
        SetInteractState(false);
    }
    
    public override void Interact(Interactor interactor, InputAction.CallbackContext context, Item currentGripItem = null)
    {
        if (context.control.name.Equals("f"))
        {
            // F키 눌림 의미 todo: 상호작용 : NPC 대화, 장소이동, CraftTab 열기 등등
            interactionBehaviour.Operate(interactor, currentGripItem);
        }
        else
        {
            //부수기 동작
            base.Interact(interactor, context, currentGripItem);
        }
    }
    
    
    protected override void DropItems()
    {
        if (item == null || item.GetRebuildItem() == null)
        {
            base.DropItems();
        }
        else
        {
            if (!ObjectPoolManager.Instance.IsPoolRegistered(item.itemData.ID))
                ObjectPoolManager.Instance.RegisterPrefab(item.itemData.ID, item.itemData.DropObjectPrefab);
            
            
            Vector3 dropPosition = transform.position + new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f),
                0);
            
            DropObject dropObj = ObjectPoolManager.Instance.SpawnObject(
                item.itemData.ID,
                dropPosition, 
                Quaternion.identity).GetComponent<DropObject>();
            
            dropObj.OverrideItem(item);
                
            dropObj?.transform.DOJump(dropPosition, 1f, 1, 0.8f).SetEase(Ease.OutBounce);
        }
        
        item = null;
    }
    
    
    public override void SetInteractState(bool state)
    {
        base.SetInteractState(state);
        interactionUI.SetActive(state);
    }
    
    

    public void SetEstablishItem(EstablishItem item)
    {
        this.item = item;
    }
}
