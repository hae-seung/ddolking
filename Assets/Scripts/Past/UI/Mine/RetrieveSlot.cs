using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RetrieveSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amount;
    
    
    public void SetSlot(Item item)
    {
        icon.sprite = item.itemData.IconImage;
        CountableItem citem = item as CountableItem;
        amount.text = citem.Amount.ToString();
    }
}
