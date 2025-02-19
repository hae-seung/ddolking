using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour 
{
    //퀵슬롯의 변화가 일어나면 플레이어 Hand도 업데이트 필수
    [SerializeField] private PlayerHand playerHand;
    [SerializeField] private List<QuickSlot> quickSlots = new List<QuickSlot>();
    [SerializeField] private Color quickSlotNormalColor;
    [SerializeField] private Color quickSlotSelectColor;

    public void SelectSlot(int prevSlot, int currentSlot = 1)
    {
        quickSlots[prevSlot - 1].SetSlotImage(quickSlotNormalColor);
        quickSlots[currentSlot - 1].SetSlotImage(quickSlotSelectColor);
    }
    
    
    public void UpdateItemAmount(int idx, int amount = 0)
    {
        // Debug.Log($"{amount}");
        quickSlots[idx].UpdateAmount(amount);
    }
    
    #region UpdatePlayerHandImage

    public void SetItemIcon(int idx, Sprite sprite)
    {
        quickSlots[idx].SetItemIcon(sprite);
        playerHand.UpdateHand(idx + 1);
    }

    public void RemoveItem(int idx)
    {
        quickSlots[idx].RemoveItem();
        playerHand.UpdateHand(idx + 1);
    }
    
    #endregion
  

    public void HideAmountText(int idx)
    {
        quickSlots[idx].HideAmountText();
    }
    
    
    
}
