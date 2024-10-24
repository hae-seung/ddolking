using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GripItem : MonoBehaviour
{
    private SpriteRenderer itemImage;
    private Item itemInstance;
    
    private void Awake()
    {
        itemImage = GetComponent<SpriteRenderer>();
    }

    public void Init(Item item)
    {
        itemInstance = item;
        itemImage.sprite = itemInstance.itemData.IconImage;
    }
}
