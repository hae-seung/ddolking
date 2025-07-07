using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShopBehaviour : InteractionBehaviour
{
    [SerializeField] private ShopType shopType;
    
    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        UIManager.Instance.OpenShop(shopType);
    }
}

public enum ShopType
{
    past,
    future,
    reinforce,
    enchant,
    durability
}