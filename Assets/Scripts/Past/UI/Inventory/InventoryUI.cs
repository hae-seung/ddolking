using System.Collections.Generic;
using Unity.VisualScripting;
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
    public ToolTipUI _itemTooltip;
    public PopupUI _popupUI;

    private Inventory inventory;
    private Slot _beginDragSlot;
    
    private RectTransform _beginDragIconTransform;
    private Vector2 _beginDragIconPoint;
    private Vector3 _beginDragCursorPoint;
    
    private int _originalChildIndex; 
    private RectTransform _originalParent;
    private bool _isDragging = false;

    private Slot _pointerOverSlot = null; //현재 포인터가 위치한 곳의 슬롯

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

        OnPointerEnterAndExit();
        
        ShowOrHideItemTooltip();
        
        HandlePointerDown();
        HandlePointerDrag();
        HandlePointerUp();
    }
    
    private void ShowOrHideItemTooltip()
    {
        // 마우스가 유효한 아이템 아이콘 위에 올라와 있다면 툴팁 보여주기
        bool isValid = _pointerOverSlot != null 
                       && _pointerOverSlot.IsUsing 
                       && !_isDragging;

        if (isValid)
        {
            UpdateTooltipUI(_pointerOverSlot);
            _itemTooltip.Show();
        }
        else
            _itemTooltip.Hide();
    }
    
    
    private void UpdateTooltipUI(Slot slot)
    {
        // 툴팁 정보 갱신
        _itemTooltip.SetItemInfo(inventory.GetItem(slot.SlotIdx));

        // 툴팁 위치 조정
        _itemTooltip.SetRectPosition(slot.SlotRect);
    }
    
    

    private void OnPointerEnterAndExit()
    {
        //이전 프레임의 슬롯
        var prevSlot = _pointerOverSlot;

        // 현재 프레임의 슬롯
        var curSlot = _pointerOverSlot = RaycastAndGetFirstComponent<Slot>();

        if (prevSlot == null)
        {
            // Enter
            if (curSlot != null)
            {
                OnCurrentEnter();
            }
        }
        else
        {
            // Exit
            if (curSlot == null)
            {
                OnPrevExit();
            }

            // Change
            else if (prevSlot != curSlot)
            {
                OnPrevExit();
                OnCurrentEnter();
            }
        }

        // ===================== Local Methods ===============================
        void OnCurrentEnter()
        {
            curSlot.Highlight(true);
        }
        void OnPrevExit()
        {
            prevSlot.Highlight(false);
        }
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
        if (Input.GetMouseButtonDown(0))//좌클릭
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
        else if (Input.GetMouseButtonDown(1))//우클릭
        {
            Slot slot = RaycastAndGetFirstComponent<Slot>();
            if (slot != null && slot.IsUsing)
            {
                TryUseItem(slot.SlotIdx);
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
            
            if (targetSlot == null)
            {
                TrashCan trashCan = RaycastAndGetFirstComponent<TrashCan>();
                if (trashCan != null)
                {
                    int selectedItemIndex = _beginDragSlot.SlotIdx;
                    int selectedItemCount = inventory.GetItemCount(selectedItemIndex);
                    string itemName = inventory.GetItemName(selectedItemIndex);
                    
                    _popupUI.OpenTrashPopup(itemName, selectedItemCount, (int count) =>
                    {
                        TryRemoveItem(selectedItemIndex, count);
                    });
                }
            }
            else if (targetSlot != null && targetSlot != _beginDragSlot)
            {
                SwapItems(_beginDragSlot, targetSlot);
            }
            
            ResetDraggedSlot(); // 원래 위치로 복귀

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

    private void TryRemoveItem(int index, int count)
    {
        inventory.RemoveItem(index, count);
    }

    private void TryUseItem(int index)
    {
        inventory.InteractWithItem(index);
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
        if (0 <= idx && idx <= 4)
            quickSlotUI.UpdateItemAmount(idx, amount);
        
        slots[idx].UpdateItemAmount(amount);
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
