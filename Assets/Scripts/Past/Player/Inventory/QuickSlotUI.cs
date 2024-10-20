using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    public Slot[] quickSlots; // 퀵슬롯 UI 배열
    public Transform quickSlotHolder;
    public GameObject itemPrefab;
    public Sprite slotImage;
    public Sprite slectedImage;
    

    public void InitializeQuickSlots()
    {
        // 퀵슬롯 UI 초기화
        for (int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i].GetComponent<Image>().raycastTarget = true;
            quickSlots[i].GetComponent<Image>().sprite = slotImage;
        }
    }

   
}