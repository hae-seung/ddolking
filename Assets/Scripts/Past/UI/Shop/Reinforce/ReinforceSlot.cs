using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReinforceSlot : MonoBehaviour
{
    [SerializeField] private GameObject highLightImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI upgradeAmoundtTxt;
    
    private Item equipItem;
    
    
    public Item GetItem()
    {
        return equipItem;
    }


    public void TurnOn()
    {
        highLightImage.SetActive(true);   
    }

    public void TurnOff()
    {
        highLightImage.SetActive(false);
    }
    
    
    public void UpdateSlot(Item item)
    {
        equipItem = item;
        iconImage.sprite = item.itemData.IconImage;
        
        EquipItem eItem = item as EquipItem;
        if (eItem.curLevel == 0)
            upgradeAmoundtTxt.text = "";
        else
            upgradeAmoundtTxt.text = $"+{eItem.curLevel}";
    }

    public void LevelUp()
    {
        EquipItem eItem = equipItem as EquipItem;
        upgradeAmoundtTxt.text = $"+{eItem.curLevel}";
    }
}
