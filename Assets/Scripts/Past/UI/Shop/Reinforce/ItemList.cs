using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    [SerializeField] private ReinforceShop reinforceShop;
    [SerializeField] private RectTransform contentParent;
    [SerializeField] private GameObject slotPrefab;
    
    private List<ReinforceSlot> slots = new List<ReinforceSlot>();

    private ReinforceSlot currentSlot;
    private ReinforceSlot prevSlot;

    [SerializeField]
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList = new List<RaycastResult>();
        
        
    private void Update()
    {
        HandlePointerDown();
    }


    private void Start()
    {
        ped = new PointerEventData(EventSystem.current);
    }

    private void Init()
    {
        //모든 슬롯 끄기
        for (int i = 0; i < slots.Count; i++)
            slots[i].gameObject.SetActive(false);
    }
    
    
    public void UpdateSlot()
    {
        Init();
    
        Debug.Log("켜져라");
        
        int inventorySlotCnt = Inventory.Instance.SlotCnt;
        int currentSlotIndex = 0;

        for (int i = 0; i < inventorySlotCnt; i++)
        {
            Item item = Inventory.Instance.GetItem(i);

            if (item is EquipItem equipItem && equipItem.EquipData.isEnhanceable)
            {
                if (currentSlotIndex >= slots.Count)
                {
                    // 부족한 슬롯 생성
                    ReinforceSlot slot = Instantiate(slotPrefab, contentParent).GetComponent<ReinforceSlot>();
                    slots.Add(slot);
                }

                slots[currentSlotIndex].UpdateSlot(item);
                slots[currentSlotIndex].gameObject.SetActive(true);
                currentSlotIndex++;
            }
        }
    }

    
    private void HandlePointerDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ped.position = Input.mousePosition;

            // 슬롯 외 클릭 시 null 반환됨
            ReinforceSlot clickedSlot = RayCastAndGetFirstComponent<ReinforceSlot>();

            // 슬롯이 아닐 경우 무시하고 리턴
            if (clickedSlot == null)
                return;

            // 같은 슬롯이면 무시
            if (clickedSlot == currentSlot)
                return;

            // 현재 슬롯 꺼주고, 새 슬롯 켜기
            currentSlot?.TurnOff();
            currentSlot = clickedSlot;
            currentSlot.TurnOn();
            reinforceShop.UpdateAll(currentSlot);
        }
    }




   


    private T RayCastAndGetFirstComponent<T>() where T : Component
    {
        rrList.Clear();
        gr.Raycast(ped, rrList);

        if (rrList.Count == 0)
            return null;

        return rrList[0].gameObject.GetComponentInParent<T>();
    }
}
