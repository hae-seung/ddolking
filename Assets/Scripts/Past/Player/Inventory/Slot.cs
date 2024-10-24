using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private Image image;
    private RectTransform rect;
    public InventoryUI inventoryUI;
    public QuickSlotUI quickSlotUI;
    public bool isQuickSlot = false;

    public int SlotIndex { get; private set; } // 슬롯 인덱스는 UI에서 해당 슬롯의 인덱스

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
        if (eventData.pointerDrag != null) // 드래그된 아이템이 존재하는 경우
        {
            Transform draggedItem = eventData.pointerDrag.transform; // 드래그 중인 아이템
            DragableItem draggedItemScript = draggedItem.GetComponent<DragableItem>();

            Slot previousSlot = draggedItemScript.PreviousParent.GetComponent<Slot>();
            if (previousSlot != null)
            {
                int previousSlotIndex = previousSlot.SlotIndex;

                // 1. 현재 슬롯에 이미 아이템이 있는 경우 처리
                if (transform.childCount > 0)
                {
                    Transform itemInSlot = transform.GetChild(0); // 현재 슬롯에 있는 아이템
                    itemInSlot.SetParent(previousSlot.transform);
                    itemInSlot.localPosition = Vector3.zero; // 위치 초기화
                }

                draggedItem.SetParent(transform);
                draggedItem.localPosition = Vector3.zero; // 위치 초기화

                // 2. 인벤토리에서 아이템 스왑 처리 (아이템 교체 후 처리)
                inventoryUI.SwapItem(previousSlotIndex, SlotIndex);

                // 3. 퀵슬롯에서 나가는 경우 처리 (드래그된 아이템의 이전 부모가 퀵슬롯인 경우)
                if (previousSlot.isQuickSlot)
                {
                    quickSlotUI.UpdateQuickSlot(previousSlotIndex); // 퀵슬롯에서 나간 경우 업데이트
                }

                // 4. 퀵슬롯으로 아이템이 들어오는 경우 처리
                if (isQuickSlot)
                {
                    quickSlotUI.UpdateQuickSlot(SlotIndex); // 퀵슬롯에 아이템이 들어왔음을 알림
                }
            }
        }
    }
}
