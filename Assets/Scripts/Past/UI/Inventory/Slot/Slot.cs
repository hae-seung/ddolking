using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image _highlightImage;
    public int SlotIdx { get; private set; }
    public bool IsUsing { get; private set; } = false;
    
    private void ShowIcon() => iconImage.gameObject.SetActive(true);
    private void HideIcon() => iconImage.gameObject.SetActive(false);

    private void ShowText() => amountText.gameObject.SetActive(true);
    private void HideText() => amountText.gameObject.SetActive(false);
    
    public void SetIndex(int idx)
    {
        SlotIdx = idx;
    }

    public void CreateItem(Item item)
    {
        if (item is CountableItem citem)
        {
            amountText.text = citem.Amount.ToString();
            ShowText();
        }
        
        iconImage.sprite = item.itemData.IconImage;
        ShowIcon();

        IsUsing = true;
    }
    
    
    public void UpdateItemAmount(int amount)
    {
        amountText.text = amount.ToString();
    }
}
