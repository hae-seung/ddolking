using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private AmuletSlot amuletSlot;


    public void UpdateAmulet(Item amuletItem)
    {
        amuletSlot.UpdateAmulet(amuletItem);
    }
}
