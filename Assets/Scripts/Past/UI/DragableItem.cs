using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

{
    private Transform previousParent;
    private int previousParentSlotIndex;
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private Item itemInstance;//인벤토리와의 참조를 하기 위함
    private GameObject dropObjectPrefab;
    
    public Image iconImage;
    public Text itemAmount;

    public Item ItemInstance => itemInstance;

    public Transform PreviousParent => previousParent;

    public int PreviousParentSlotIndex => previousParentSlotIndex;
  
    
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = true;
    }

    public void Init(Item item)
    {
        itemInstance = item;//인벤토리에서 받아온 아이템 그대로 참조하기 위함(정보가 연게되어 저장됨)
        iconImage.sprite = itemInstance.itemData.IconImage;
        
        dropObjectPrefab = itemInstance.itemData.DropObjectPrefab;
        //todo:혹시나 아이템 드랍을 구현한다면, 인스턴스화 후에 Countable아이템이라면 SetAmount로 수량 조절필요
        
        if (item is CountableItem ci)
            itemAmount.text = ci.Amount.ToString();
        else
            itemAmount.text = "1";
        
        //itemInstance의 변화가 Inventory의 _items에도 그대로 적용됨
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent; // 현재 부모 저장
        previousParentSlotIndex = previousParent.GetComponent<Slot>().SlotIndex;
        
        transform.SetParent(transform.root); // 아이템을 최상단 canvas로 이동
        transform.SetAsLastSibling(); // 드래그 중 다른 UI 요소 위에 표시

        canvasGroup.alpha = 0.6f; // 드래그 중 아이템을 반투명하게
        canvasGroup.blocksRaycasts = false; // 드래그 중에는 아이템 자체는 Raycast를 차단
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position; // 마우스 위치에 따라 아이템 이동
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        // 아이템이 드롭된 슬롯이 없으면 원래 부모로 복귀
        if (transform.parent == transform.root)
        {
            transform.SetParent(previousParent); // 원래 부모로 복귀
            rect.position = previousParent.GetComponent<RectTransform>().position;
        }
        else
        {
            ResizeItemImage();
        }

       
    }

    [ContextMenu("resize")]
    public void ResizeItemImage()
    {
        RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(parentRect.rect.width, parentRect.rect.height);
        transform.position = transform.parent.position;
    }
}