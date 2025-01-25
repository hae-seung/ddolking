using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotUI : MonoBehaviour 
{
    //퀵슬롯의 변화가 일어나면 플레이어 Hand도 업데이트 필수
    public List<QuickSlot> quickSlots = new List<QuickSlot>();
    public void CreateItem(int idx, Item item)
    {
        quickSlots[idx].CreateItem(item);
        UpdatePlayerHand();
    }

    public void UpdateItemAmount(int idx, int amount)
    {
        quickSlots[idx].UpdateAmount(amount);
    }
    
    private void UpdatePlayerHand()
    {
        
    }
    
    
}
