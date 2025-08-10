using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChestSlotType
{
    inven,
    chest
}


public class ChestSlot : MonoBehaviour
{
    [SerializeField] private RectTransform slotRect;
    private ChestSlotType type;
    
    
    public ChestSlotType GetSlotType => type;
    public RectTransform SlotRect => slotRect;
    public bool IsUsing => item != null;
    
    public int Index { get; private set; }
    
    
    private Item item;
    
    
    public Item GetItem()
    {
        return item;
    }
    
    public void Highlight(bool state)
    {
        
    }
}
