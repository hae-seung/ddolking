using System;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    public Slot[] quickSlots; // 퀵슬롯 UI 배열
    public Transform quickSlotHolder;
    public GameObject itemPrefab;
    public Sprite slotImage;

    

    public void InitializeQuickSlots()
    {
        // 퀵슬롯 UI 초기화
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].GetComponent<Image>().raycastTarget = true;
            quickSlots[i].GetComponent<Image>().sprite = slotImage;
        }
    }

    // 퀵슬롯 UI 업데이트
    public void UpdateQuickSlot(int quickSlotIndex, Item item)
    {
        DragableItem existingItem = quickSlots[quickSlotIndex].GetComponentInChildren<DragableItem>();

        if (existingItem != null && item != null)
        {
            // 기존 아이템 UI 업데이트
            existingItem.Init(item);
        }
        else if (existingItem == null && item != null)
        {
            // 빈 퀵슬롯에 새로 아이템 추가
            GameObject newItem = Instantiate(itemPrefab, quickSlots[quickSlotIndex].transform);
            newItem.GetComponent<DragableItem>().Init(item);
        }
        else if (existingItem != null && item == null)
        {
            // 아이템이 사라졌을 경우 기존 아이템 UI 제거
            Destroy(existingItem.gameObject);
        }
    }
}