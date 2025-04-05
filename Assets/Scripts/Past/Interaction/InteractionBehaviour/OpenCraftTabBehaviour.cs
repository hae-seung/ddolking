
using UnityEngine;

public class CraftTabBehaviour : InteractionBehaviour
{
    [SerializeField] private CraftManualType craftManualType;
    [SerializeField] private Interactable interactableObject;
    [SerializeField] private MakingItemUI makingItemUI;
    
    public bool IsMaking { get; private set; } = false;
    
    protected override void Interact(Interactor interactor)
    {
        if (IsMaking)
            return;
        
        UIManager.Instance.OpenCraftTab(craftManualType, (CraftItemSO craftItem, int amount) =>
        {
            makingItemUI.MakeItem(craftItem, amount, onMakingFinished);
            IsMaking = true;
        });
    }

    private void onMakingFinished()
    {
        IsMaking = false;
    }
    
}
