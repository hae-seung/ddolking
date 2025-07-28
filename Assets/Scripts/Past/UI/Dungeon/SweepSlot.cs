using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SweepSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amount;
    
    public void SetData(ItemData item, int amount)
    {
        itemImage.sprite = item.IconImage;
        this.amount.text = amount.ToString();
    }
    
}
