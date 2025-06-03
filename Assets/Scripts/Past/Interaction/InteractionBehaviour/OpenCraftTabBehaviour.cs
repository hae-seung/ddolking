using System;
using UnityEngine;

public class CraftTabBehaviour : InteractionBehaviour
{
    [SerializeField] private CraftManualType craftManualType;
    [SerializeField] private Interactable interactableObject;
    [SerializeField] private MakingItemUI makingItemUI;
    
    private Animator animator;
    
    private SpriteRenderer sr;
    private Sprite idleSprite;

    private bool IsMaking = false;
    private readonly string operate = "make";

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        idleSprite = sr.sprite;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        if(animator)
            animator.enabled = false;
    }

    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        if (IsMaking)
            return;
        
        UIManager.Instance.OpenCraftTab(craftManualType, (CraftItemSO craftItem, int amount) =>
        {
            makingItemUI.MakeItem(craftItem, amount, onMakingFinished);
            IsMaking = true;
            
            
            if(animator)
            {
                SetAnimator(IsMaking);
            }
        });
    }

    private void onMakingFinished()
    {
        IsMaking = false;
        
        if(animator)
        {
            SetAnimator(IsMaking);
        }

        sr.sprite = idleSprite;
    }

    private void SetAnimator(bool state)
    {
        if (state)
        {
            animator.enabled = true;
            animator.SetBool(operate, IsMaking);
        }
        else
        {
            animator.SetBool(operate,IsMaking);
            animator.enabled = false;
        }
    }
    
}
