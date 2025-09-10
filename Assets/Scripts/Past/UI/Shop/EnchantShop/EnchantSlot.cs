using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnchantSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ToolTipUI toolTipUI;
    [SerializeField] private Image image;
    [SerializeField] private RectTransform rect;
    private Item amulet;
    
    public void Init()
    {
        image.gameObject.SetActive(false);
        amulet = null;
    }

    
    //강화 안된 아뮬렛이 계속 올라올거임
    public void SwapTo(Item amulet)
    {
        image.gameObject.SetActive(true);
        this.amulet = amulet;
        image.sprite = amulet.itemData.IconImage;
    }

    
    //인챈트된 아뮬렛이 올라올거임
    public void SetEnchantAmulet(Item enchantAmulet)
    {
        amulet = enchantAmulet;
        image.sprite = enchantAmulet.itemData.IconImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (amulet == null)
            return;
        toolTipUI.SetItemInfo(amulet);
        toolTipUI.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipUI.Hide();
    }
}
