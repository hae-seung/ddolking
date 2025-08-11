using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour
{
    [Header("창고슬롯리스트")] //지금은 12개로 고정을 박았지만 아니라면 Instanciate
    [SerializeField] private List<ChestSlot> chestSlots;

    [Header("인벤토리슬롯")] 
    [SerializeField] private GameObject invenSlotPrefab;
    [SerializeField] private Transform contentParent;

    [Header("툴팁")] 
    [SerializeField] private ToolTipUI _itemTooltip;

    [Header("버튼")] 
    [SerializeField] private Button closeBtn;

    private List<ChestSlot> inventorySlots;
    
    
    //마우스 이동 감지
    private GraphicRaycaster _gr;
    private PointerEventData _ped;
    private List<RaycastResult> _rrList;
    
    private ChestSlot _pointerOverSlot = null;

    private Chest chest;
    public bool isOpen;
    

    private void Awake()
    {
        inventorySlots = new List<ChestSlot>();
        
        _gr = GetComponent<GraphicRaycaster>();
        _ped = new PointerEventData(EventSystem.current);
        _rrList = new List<RaycastResult>();

        
        closeBtn.onClick.AddListener(() =>
        {
            isOpen = false;
            gameObject.SetActive(false);
            GameEventsManager.Instance.inputEvents.EnableInput();
            GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
        });
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
        //인벤토리 "1칸"으로부터 아이템이 빠져나가는 거임
        Item item = slot.GetItem();
        
        //상자에 넣었으나 못넣고 남은 양 && 알아서 상자 저장고 부분 UI 업데이트 처리됨
        int remain = chest.Add(item, amount);

        int moveAmount = amount - remain;
        
        //클릭된 인벤토리 슬롯의 인덱스 == 인벤토리 실제 슬롯인덱스
        Inventory.Instance.RemoveItem(slot.Index, moveAmount);
        
        //현재 인벤토리 슬롯에서 나갔음 -> 현재 클릭된 슬롯을 업데이트
        slot.UpdateInvenSlot();
    }

    private void ChestToInven(ChestSlot slot, int amount)
    {
        Item item = slot.GetItem();

        int remain = Inventory.Instance.Add(item, amount);

        int moveAmount = amount - remain;

        chest.RemoveItem(slot.Index, moveAmount);
        
        //현재 상자 슬롯에서 나갔음 -> 현재 클릭된 슬롯을 업데이트
        slot.UpdateChestSlot(chest);
    }

    
    
    public void Open(Chest chest)
    {
        if (isOpen)
            return;

        isOpen = true;
        
        this.chest = chest;
        
        OpenInvenList();
        OpenChestList();
        
        
        //미리 슬롯 초기화 해놓아야 Update로 돌아가는 슬롯에 접근할때 사고 방지가능
        gameObject.SetActive(true);
    }
    
    
    private void OpenInvenList()
    {
        //실제 인벤토리랑 상자에서 보이는 인벤토리랑 맞춤.
        //하나 넣고 업데이트 했는데 상자 내에서 UI 보이는 순서가 갑자기 달라지는거 방지.
        //그냥 아예 동기화 시켜버림.
        
        while (inventorySlots.Count < Inventory.Instance.SlotCnt)
        {
            ChestSlot slot = Instantiate(invenSlotPrefab, contentParent).GetComponent<ChestSlot>();
            inventorySlots.Add(slot);
        }

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].InitInvenSlot(i);
        }
    }
    
    
    private void OpenChestList()
    {
        for (int i = 0; i < chestSlots.Count; i++)
        {
            chestSlots[i].InitChestSlot(i, chest);
        }
    }



    public void UpdateInvenSlot(int idx)
    {
        if (!isOpen)
            return;
        
        //상자에서 인벤으로 아이템이 들어왔을 때 호출(없다가 생겼을 수도 있어서 Init)
        inventorySlots[idx].InitInvenSlot(idx);
    }
    
    public void UpdateChestSlot(int idx)
    {
        if (!isOpen)
            return;
        
        chestSlots[idx].InitChestSlot(idx, chest);
    }

    
}
