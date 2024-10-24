using System;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    public Slot[] quickSlots; // 퀵슬롯 UI 배열
    public Transform quickSlotHolder;
    public GameObject itemPrefab;
    public Sprite slotImage;
    public Sprite selectedImage;
    public int curSelectedIndex;
    
    public event Action<Item> OnChangedQuickSlot;
    public event Action OnQuickSlotEmptied;

    private void Awake()
    {
        curSelectedIndex = 0;
    }

    public void InitializeQuickSlots()
    {
        // 퀵슬롯 UI 초기화
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].GetComponent<Image>().raycastTarget = true;
            quickSlots[i].GetComponent<Image>().sprite = slotImage;
        }
        quickSlots[0].GetComponent<Image>().sprite = selectedImage;//첫번째룰 디폴트로 선택하며 시작
    }
    
    public Item MoveQuickSlot(int originIndex, int changeIndex)
    {
        quickSlots[originIndex].GetComponent<Image>().sprite = slotImage;
        quickSlots[changeIndex].GetComponent<Image>().sprite = selectedImage;
        curSelectedIndex = changeIndex;

        // DragableItem을 통해서 Item에 접근하기 전에 null 체크
        DragableItem dragableItem = quickSlots[curSelectedIndex].GetComponentInChildren<DragableItem>();

        if (dragableItem == null)
            return null;

        Item slotItem = dragableItem.ItemInstance;
        return slotItem;
    }
    
    public void UpdateQuickSlot(int slotIndex)
    {
        // 현재 선택된 퀵슬롯이 변경된 슬롯과 동일할 때만 아이템 변경 처리
        if (quickSlots[curSelectedIndex].SlotIndex == slotIndex)
        {
            DragableItem dragableItem = quickSlots[curSelectedIndex].GetComponentInChildren<DragableItem>();

            if (dragableItem != null)
            {
                // 손에 든 아이템을 새로운 아이템으로 교체
                OnChangedQuickSlot?.Invoke(dragableItem.ItemInstance);
            }
            else
            {
                // 퀵슬롯이 비워졌을 경우 손에 있는 아이템을 제거
                OnQuickSlotEmptied?.Invoke();
            }
        }
    }




    
    
}