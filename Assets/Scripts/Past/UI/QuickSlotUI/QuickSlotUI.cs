using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUI : MonoBehaviour 
{
    //퀵슬롯의 변화가 일어나면 플레이어 Hand도 업데이트 필수
    public List<QuickSlot> quickSlots = new List<QuickSlot>();

    public void UpdateItemAmount(int idx, int amount = 0)
    {
        quickSlots[idx].UpdateAmount(amount);
    }
    

    public void SetItemIcon(int idx, Sprite sprite)
    {
        quickSlots[idx].SetItemIcon(sprite);
    }

    public void RemoveItem(int idx)
    {
        quickSlots[idx].RemoveItem();
    }

    public void HideAmountText(int idx)
    {
        quickSlots[idx].HideAmountText();
    }
    
}
