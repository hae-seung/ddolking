using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCraftTabBehaviour : InteractionBehaviour
{
    [SerializeField] CraftManualType craftManualType;
    
    
    protected override void Interact(Interactor interactor)
    {
        UIManager.Instance.ToggleCraftTab(craftManualType);
    }
}
