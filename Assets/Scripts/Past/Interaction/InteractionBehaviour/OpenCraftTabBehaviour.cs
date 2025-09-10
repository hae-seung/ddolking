using System;
using UnityEngine;


public interface IReBuild
{
    public abstract void SetRebuildItem(EstablishItem item);
}



public class OpenCraftTabBehaviour : InteractionBehaviour, IReBuild
{
    [SerializeField] private CraftManualType craftManualType;
    [SerializeField] private EstablishItemData establishData;
    [SerializeField] private ReinforceStructureData reinforceData;
    [SerializeField] private MakingItemUI makingItemUI;
    
    private Animator animator;
    private SpriteRenderer sr;
    private Sprite idleSprite;

    private InterBreakableObject interBreakableObject;

    private bool IsMaking = false;
    private readonly string operate = "make";

    private EstablishItem eitem;
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        interBreakableObject = GetComponent<InterBreakableObject>();
        idleSprite = sr.sprite;


        if (!interBreakableObject)
            return;
        eitem = new EstablishItem(establishData);
        ReinforceStructureItem ritem = new ReinforceStructureItem(reinforceData);
        eitem.SetRebuildItem(ritem);
        interBreakableObject.SetEstablishItem(eitem);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        if(animator)
            animator.enabled = false;
    }

    private void OnEnable()
    {
        idleSprite = sr.sprite;
    }

    private void OnDisable()
    {
        IsMaking = false;
        makingItemUI.StopMaking();
    }
    

    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        if (IsMaking)
            return;
        
        ReinforceStructureItem ritem = null;
        
        if(interBreakableObject)
            ritem = eitem.GetRebuildItem() as ReinforceStructureItem; 
        
        UIManager.Instance.OpenCraftTab(craftManualType, ritem,
            (CraftItemSO craftItem, int amount) =>
            {
                makingItemUI.MakeItem(craftItem, amount, ritem ,onMakingFinished);
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


    public void SetRebuildItem(EstablishItem item)
    {
        if (item.GetRebuildItem() == null)
        {
            ReinforceStructureItem ritem = new ReinforceStructureItem(reinforceData);
            item.SetRebuildItem(ritem);
        }
        eitem = item;
        interBreakableObject.SetEstablishItem(eitem);
    }
}
