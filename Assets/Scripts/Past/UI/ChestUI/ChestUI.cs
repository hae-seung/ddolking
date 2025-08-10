using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour
{
    [Header("창고슬롯리스트")] 
    [SerializeField] private List<ChestSlot> chestSlots;

    [Header("인벤토리슬롯")] 
    [SerializeField] private GameObject invenSlotPrefab;
    [SerializeField] private Transform contentParent;

    [Header("툴팁")] 
    [SerializeField] private ToolTipUI _itemTooltip;

    private List<ChestSlot> inventorySlots;
    
    
    //마우스 이동 감지
    private GraphicRaycaster _gr;
    private PointerEventData _ped;
    private List<RaycastResult> _rrList;
    
    private ChestSlot _pointerOverSlot = null;

    private Chest chest;

    private void Awake()
    {
        inventorySlots = new List<ChestSlot>();
        
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
    }

    private T RaycastAndGetFirstComponent<T>() where T : Component
    {
        _rrList.Clear();
        _gr.Raycast(_ped, _rrList);

        if (_rrList.Count == 0)
            return null;

        return _rrList[0].gameObject.GetComponentInParent<T>();
    }
    
    
    
    
    private void OnPointerEnterAndExit()
    {
        //이전 프레임의 슬롯
        var prevSlot = _pointerOverSlot;

        // 현재 프레임의 슬롯
        var curSlot = _pointerOverSlot = RaycastAndGetFirstComponent<ChestSlot>();

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
    
    private void ShowOrHideItemTooltip()
    {
        // 마우스가 유효한 아이템 아이콘 위에 올라와 있고 슬롯이 아이템을 담고 있다면
        bool isValid = _pointerOverSlot != null
                       && _pointerOverSlot.IsUsing;

        if (isValid)
        {
            UpdateTooltipUI(_pointerOverSlot);
            _itemTooltip.Show();
        }
        else
            _itemTooltip.Hide();
    }

    private void UpdateTooltipUI(ChestSlot slot)
    {
        // 툴팁 정보 갱신
        _itemTooltip.SetItemInfo(slot.GetItem());

        // 툴팁 위치 조정
        _itemTooltip.SetRectPosition(slot.SlotRect);
    }
    
    private void HandlePointerDown()
    {
        if (Input.GetMouseButtonDown(0)) //좌클릭
        {
            var slot = RaycastAndGetFirstComponent<ChestSlot>();

            //현재 클릭한 곳이 유효한지 확인
            if (slot == null || !slot.IsUsing)
                return;

            var curSlotType = slot.GetSlotType;
            Item curItem = slot.GetItem();
            
            //장비아이템이면 그냥 넘겨주기
            if (curItem is EquipItem eitem)
            {
                if (curSlotType == ChestSlotType.inven)
                {
                    InvenToChest(slot, 1);
                }
                else
                {
                    ChestToInven(slot, 1);
                }
                return;
            }
            
            //쉬프트 + 좌클릭 => 아이템 전부 넘기기
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                CountableItem citem = curItem as CountableItem;
                
                if (curSlotType == ChestSlotType.inven)
                {
                    InvenToChest(slot, citem.Amount);
                }
                else
                {
                    ChestToInven(slot, citem.Amount);
                }
            }
            else //그냥 좌클릭 => 1개 넘기기
            {
                if (curSlotType == ChestSlotType.inven)
                {
                    InvenToChest(slot, 1);
                }
                else
                {
                    ChestToInven(slot, 1);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1)) //우클릭 => 절반 넘기기
        {
            var slot = RaycastAndGetFirstComponent<ChestSlot>();

            //현재 우클릭한 곳이 유효한지 확인
            if (slot == null || !slot.IsUsing)
                return;

            var curSlotType = slot.GetSlotType;
            Item curItem = slot.GetItem();
            
            //장비아이템이면 그냥 넘겨주기
            if (curItem is EquipItem eitem)
            {
                if (curSlotType == ChestSlotType.inven)
                {
                    InvenToChest(slot, 1);
                }
                else
                {
                    ChestToInven(slot, 1);
                }
                return;
            }
            
            CountableItem citem = curItem as CountableItem;
            if (curSlotType == ChestSlotType.inven)
            {
                InvenToChest(slot, Mathf.RoundToInt(citem.Amount/2));
            }
            else
            {
                ChestToInven(slot, Mathf.RoundToInt(citem.Amount/2));
            }
            
        }
    }
    
    
    private void InvenToChest(ChestSlot slot, int amount)
    {
        Item item = slot.GetItem();
        //상자에 넣었으나 못넣고 남은 양
        int remain = chest.Add(item, amount);

        int moveAmount = amount - remain;
        
    }

    private void ChestToInven(ChestSlot slot, int amount)
    {
        
    }

    
    
    
    
    public void Open(Chest chest)
    {
        this.chest = chest;

        UpdateInvenList();
        UpdateChestList();
        
        
        //미리 슬롯 초기화 해놓아야 Update로 돌아가는 슬롯에 접근할때 사고 방지가능
        gameObject.SetActive(true);
    }
    
    private void UpdateInvenList()
    {
        
    }
    
    private void UpdateChestList()
    {
        
    }

    
}
