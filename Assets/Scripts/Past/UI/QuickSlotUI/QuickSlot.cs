using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;
    
    private void ShowIcon() => iconImage.gameObject.SetActive(true);
    private void HideIcon() => iconImage.gameObject.SetActive(false);

    private void ShowText() => amountText.gameObject.SetActive(true);
    private void HideText() => amountText.gameObject.SetActive(false);

    public bool IsUsing { get; private set; } = false;
    
    public void CreateItem(Item item)
    {
        HideIcon();
        HideText();
        
        if (item is CountableItem citem)
        {
            amountText.text = citem.Amount.ToString();
            ShowText();
        }
        
        iconImage.sprite = item.itemData.IconImage;
        ShowIcon();

        IsUsing = true;
    }

    public void UpdateAmount(int amount)//countableItem만 실행함
    {
        amountText.text = amount.ToString();
    }
}
