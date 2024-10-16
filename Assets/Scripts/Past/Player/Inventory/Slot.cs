using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private Image image;
    private RectTransform rect;
    private int slotIndex;//슬롯 내부의 number
    public InventoryUI inventoryUI;

    public int SlotIndex
    {
        get => slotIndex;
        private set { slotIndex = value; }
    }

    public void SetIndex(int idx)
    {
        SlotIndex = idx;
    }
    
    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        // Ensure that Raycast Target is enabled
        if (!image.raycastTarget)
        {
            image.raycastTarget = true;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.red; // 마우스가 슬롯에 들어오면 색상 변경
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white; // 마우스가 슬롯에서 나가면 원래 색상으로 복원
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null) // 현재 드래그하고 있는 대상이 있다면
        {
            Transform draggedItem = eventData.pointerDrag.transform; // 드래그 중인 아이템
            DragableItem draggedItemScript = draggedItem.GetComponent<DragableItem>();

            int previousSlotIndex = draggedItemScript.PreviousParentSlotIndex;

            if (transform.childCount > 0) // 현재 슬롯에 이미 아이템이 있다면
            {
                // 현재 슬롯에 있는 아이템
                Transform itemInSlot = transform.GetChild(0);
                
                // 드래그된 아이템의 원래 부모 슬롯
                Transform originalParent = draggedItemScript.PreviousParent;

                // 1. 현재 슬롯에 있는 아이템을 드래그된 아이템의 원래 부모로 이동
                itemInSlot.SetParent(originalParent);
                itemInSlot.localPosition = Vector3.zero; // 위치 초기화

                // 2. 드래그된 아이템을 현재 슬롯으로 이동
                draggedItem.SetParent(transform);
                draggedItem.localPosition = Vector3.zero; // 위치 초기화
            }
            else
            {
                // 슬롯이 비어 있으면 드래그된 아이템을 이 슬롯에 넣기
                draggedItem.SetParent(transform);
                draggedItem.localPosition = Vector3.zero;
            }

            inventoryUI.SwapIndex(previousSlotIndex, SlotIndex);
        }
    }

}