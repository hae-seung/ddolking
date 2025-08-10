using System;
using UnityEngine;

public class CraftTabBehaviour : InteractionBehaviour
{
    [SerializeField] private CraftManualType craftManualType;
    [SerializeField] private EstablishItemData structureItemData;
    [SerializeField] private MakingItemUI makingItemUI;
    
    private Animator animator;
    private SpriteRenderer sr;
    private Sprite idleSprite;

    private InterBreakableObject interBreakableObject;

    private bool IsMaking = false;
    private readonly string operate = "make";


    private ReinforceStructureItem structureItem;
    private bool isInit = false;
    private int id;
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        interBreakableObject = GetComponent<InterBreakableObject>();
        idleSprite = sr.sprite;
        
        //데이터 저장 불러오기 고민해보기
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        if(animator)
            animator.enabled = false;
        
        //저장 된 데이터가 없이 필드에 설치되는 경우 == 새롭게 객체 생성 필요  
        if (!isInit)
        {
            if (interBreakableObject)
            {
                //게임 시작되고 처음부터 씬에 배치된 구조물인 경우
                structureItem = new ReinforceStructureItem(structureItemData);
                
                //breakableObject에도 저장
                interBreakableObject.SetEstablishItem(structureItem);
            }
        }
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


    //구조물이 설치될때마다 호출될거임.
    public void SetStructureData(ReinforceStructureItem reinforceStructureItem)
    {
        structureItem = reinforceStructureItem;

        RegisterStructure();

        Debug.Log($"{gameObject.name}의 id : {id}");
        isInit = true;
    }

    private void RegisterStructure()
    {
        id = ReinforceManager.Instance.RegisterStructure(structureItem.StructureId);
        structureItem.SetStructureId(id);
        Debug.Log($"id {id} 설치완료");
    }


    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        if (IsMaking)
            return;
        
        UIManager.Instance.OpenCraftTab(craftManualType, id,
            (CraftItemSO craftItem, int amount) =>
        {
            makingItemUI.MakeItem(craftItem, amount, id ,onMakingFinished);
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
