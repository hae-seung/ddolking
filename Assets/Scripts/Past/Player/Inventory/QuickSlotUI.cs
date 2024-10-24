using UnityEngine;
using UnityEngine.UI;
using System;

public class QuickSlotUI : MonoBehaviour
{
    public Slot[] quickSlots;
    public Transform quickSlotHolder;
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
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].GetComponent<Image>().raycastTarget = true;
            quickSlots[i].GetComponent<Image>().sprite = slotImage;
        }
        quickSlots[curSelectedIndex].GetComponent<Image>().sprite = selectedImage;
    }

    public void UpdateQuickSlot(int slotIndex)
    {
        if (curSelectedIndex == slotIndex)
        {
            DragableItem dragableItem = quickSlots[curSelectedIndex].GetComponentInChildren<DragableItem>();

            if (dragableItem != null)
            {
                OnChangedQuickSlot?.Invoke(dragableItem.ItemInstance);
            }
            else
            {
                OnQuickSlotEmptied?.Invoke();
            }
        }
    }
    
    public Item MoveQuickSlot(int originIndex, int changeIndex)
    {
        // 퀵슬롯의 현재 선택 상태를 변경
        quickSlots[originIndex].GetComponent<Image>().sprite = slotImage; // 원래 슬롯은 기본 이미지로 변경
        quickSlots[changeIndex].GetComponent<Image>().sprite = selectedImage; // 새로운 슬롯은 선택 이미지로 변경
        curSelectedIndex = changeIndex;

        // 선택한 슬롯에 아이템이 있는지 확인
        DragableItem dragableItem = quickSlots[curSelectedIndex].GetComponentInChildren<DragableItem>();

        if (dragableItem == null)
            return null;

        // 아이템 인스턴스 반환 (아이템 이동)
        Item slotItem = dragableItem.ItemInstance;
        return slotItem;
    }

}