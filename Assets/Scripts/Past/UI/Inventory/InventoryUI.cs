using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<Slot> slots = new List<Slot>();

    public QuickSlotUI quickSlotUI;
    public RectTransform contentParent;  // 기존 부모
    public RectTransform dragParent;     // 드래그 아이콘을 이동할 부모(Canvas 하위의 지정 오브젝트)
    [SerializeField] private ScrollRect inventoryScrollRect; // 스크롤뷰 참조 추가
    public GameObject slotPrefab;

    private Inventory inventory;
    private Slot _beginDragSlot;
    
    private RectTransform _beginDragIconTransform;
    private Vector2 _beginDragIconPoint;
    private Vector3 _beginDragCursorPoint;
    
    private int _originalChildIndex; 
    private RectTransform _originalParent;
    private bool _isDragging = false;

    private GraphicRaycaster _gr;
    private PointerEventData _ped;
    private List<RaycastResult> _rrList;

    public void Init(int count, Inventory inven)
    {
        inventory = inven;
        for (int i = 0; i < count; i++)
        {
            slots[i].SetIndex(i);
        }

        _gr = GetComponent<GraphicRaycaster>();
        _ped = new PointerEventData(EventSystem.current);
        _rrList = new List<RaycastResult>();
    }

    private void Update()
    {
        _ped.position = Input.mousePosition;

        HandlePointerDown();
        HandlePointerDrag();
        HandlePointerUp();
    }

// 드래그 중일 때 휠 스크롤 처리 및 자동 스크롤
    private void HandleScrollWhileDragging()
    {
        if (_isDragging)
        {
            float scrollAmount = Input.mouseScrollDelta.y * 0.3f;
            inventoryScrollRect.verticalNormalizedPosition = Mathf.Clamp(
                inventoryScrollRect.verticalNormalizedPosition + scrollAmount, 0f, 1f
            );
        }
    }
    

    private T RaycastAndGetFirstComponent<T>() where T : Component
    {
        _rrList.Clear();
        _gr.Raycast(_ped, _rrList);

        if (_rrList.Count == 0)
            return null;

        return _rrList[0].gameObject.GetComponentInParent<T>();
    }

    private void HandlePointerDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _beginDragSlot = RaycastAndGetFirstComponent<Slot>();

            if (_beginDragSlot != null && _beginDragSlot.IsUsing)
            {
                _beginDragIconTransform = _beginDragSlot.IconRect;
                _beginDragIconPoint = _beginDragIconTransform.anchoredPosition;
                _beginDragCursorPoint = Input.mousePosition;

                _originalParent = _beginDragIconTransform.parent as RectTransform;
                _originalChildIndex = _beginDragIconTransform.GetSiblingIndex();

                _beginDragIconTransform.SetParent(dragParent); // Canvas 내 드래그 부모로 이동
                _beginDragIconTransform.SetAsLastSibling();

                HandleScrollDrag(false);
                _isDragging = true;
            }
        }
    }

    private void HandlePointerDrag()
    {
        if (_isDragging && Input.GetMouseButton(0))
        {
            // 스크린 포인트를 UI 캔버스 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                dragParent, Input.mousePosition, null, out Vector2 localPoint);

            _beginDragIconTransform.anchoredPosition = localPoint;
            
            HandleScrollWhileDragging();
        }
    }

    private void HandlePointerUp()
    {
        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            Slot targetSlot = RaycastAndGetFirstComponent<Slot>();

            ResetDraggedSlot(); // 원래 위치로 복귀

            if (targetSlot != null && targetSlot != _beginDragSlot)
            {
                SwapItems(_beginDragSlot, targetSlot);
            }

            _beginDragSlot = null;
            _beginDragIconTransform = null;
            _isDragging = false;

            HandleScrollDrag(true);
        }
    }
    
    private void HandleScrollDrag(bool state)
    {
        inventoryScrollRect.horizontal = state;
        inventoryScrollRect.vertical = state;
    }
    

    private void SwapItems(Slot fromSlot, Slot toSlot)
    {
        if (fromSlot == toSlot || !fromSlot.IsUsing)
            return;

        // 슬롯 간 아이콘 교환 처리
        fromSlot.SwapOrMoveIcon(toSlot);
        
        // 인벤토리 데이터 업데이트
        inventory.SwapItem(fromSlot.SlotIdx, toSlot.SlotIdx);
    }

    private void ResetDraggedSlot()
    {
        _beginDragIconTransform.SetParent(_originalParent);
        _beginDragIconTransform.SetSiblingIndex(_originalChildIndex);
        _beginDragIconTransform.anchoredPosition = _beginDragIconPoint;
    }

    public void AddSlot(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Slot slot = Instantiate(slotPrefab, contentParent).GetComponent<Slot>();
            slot.SetIndex(slots.Count);
            slots.Add(slot);
        }
    }

    
    public void UpdateItemAmount(int idx, int amount = 0)
    {
        if (0 <= idx && idx < slots.Count)
        {
            slots[idx].UpdateItemAmount(amount);
            if (idx <= 4)
            {
                quickSlotUI.UpdateItemAmount(idx, amount);
            }
        }
    }


    public void SetItemIcon(int idx, Sprite sprite)
    {
        if (0 <= idx && idx <= 4)
            quickSlotUI.SetItemIcon(idx, sprite);

        slots[idx].SetItemIcon(sprite);
    }

    public void RemoveItem(int idx)
    {
        if (0 <= idx && idx <= 4)
            quickSlotUI.RemoveItem(idx);

        slots[idx].RemoveItem();
    }

    public void HideItemAmountText(int idx)
    {
        if (0 <= idx && idx <= 4)
            quickSlotUI.HideAmountText(idx);

        slots[idx].HideAmountText();
    }
}
