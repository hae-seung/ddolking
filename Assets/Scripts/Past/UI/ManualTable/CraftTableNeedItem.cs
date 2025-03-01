using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftTableNeedItem : MonoBehaviour
{
    [Header("배경")] 
    [SerializeField] private Image backGroundImage;
    
    [Header("표시 아이템")]
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameTxt;
    [SerializeField] private TextMeshProUGUI itemAmountTxt;

    public bool HasEnoughItems { get; private set; } = false;
    public int ItemTotalAmount { get; private set; }
    
    public void SetNeedItem(CraftNeedItem craftNeedItem)
    {
        itemImage.sprite = craftNeedItem.itemData.IconImage;
        itemNameTxt.text = craftNeedItem.itemData.Name;
    }

    public void SetItemAmount(CraftNeedItem craftNeedItem, int multiplier) //todo : 갯수 부족한지 충분한지 색으로 표시
    {
        ItemTotalAmount = craftNeedItem.amount * multiplier;
        
        int amount = Inventory.Instance.GetItemTotalAmount(craftNeedItem.itemData);
        itemAmountTxt.text = $"{amount} / {ItemTotalAmount}";

        HasEnoughItems = amount >= ItemTotalAmount;
        backGroundImage.color = HasEnoughItems ? Color.white : Color.red;
    }
}
