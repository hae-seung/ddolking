using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShopBehaviour : InteractionBehaviour
{
    [SerializeField] private string shopType;
    
    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        UIManager.Instance.OpenShop(shopType);
    }
}
