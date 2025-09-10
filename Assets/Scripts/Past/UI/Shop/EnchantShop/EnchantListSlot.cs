using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnchantListSlot : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;
        
    
    public void Init(ItemData amuletData, Action ClickBtn)
    {
        image.gameObject.SetActive(true);
        image.sprite = amuletData.IconImage;
        
        button.onClick.AddListener(ClickBtn.Invoke);
    }
}
