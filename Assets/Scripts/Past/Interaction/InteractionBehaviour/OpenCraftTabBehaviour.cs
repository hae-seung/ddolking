
using System;
using UnityEngine;

public class CraftTabBehaviour : InteractionBehaviour
{
    [SerializeField] private CraftManualType craftManualType;
    [SerializeField] private Interactable interactableObject;
    [SerializeField] private MakingItemUI makingItemUI;
    [SerializeField] private GameObject operateUI;

    private bool IsMaking = false;

    private void Awake()
    {
        if(operateUI != null)
            operateUI.SetActive(false);
    }

    protected override void Interact(Interactor interactor)
    {
        if (IsMaking)
            return;
        
        UIManager.Instance.OpenCraftTab(craftManualType, (CraftItemSO craftItem, int amount) =>
        {
            makingItemUI.MakeItem(craftItem, amount, onMakingFinished);
            IsMaking = true;
            if(operateUI != null)
                operateUI.SetActive(true);
        });
    }

    private void onMakingFinished()
    {
        IsMaking = false;
        if(operateUI != null)
            operateUI.SetActive(false);
    }
    
}
