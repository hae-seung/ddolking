using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstablishItem : CountableItem
{
    public EstablishItemData EstablishData { get; private set; }
    private RebuildItem rebuildItem;
    
    public EstablishItem(EstablishItemData data, int amount = 1) : base(data, amount)
    {
        EstablishData = data;
        rebuildItem = null;
    }

    
    protected override CountableItem CreateItem()
    {
        EstablishItem eitem =  new EstablishItem(CountableData as EstablishItemData);
        eitem.SetRebuildItem(rebuildItem);
        return eitem;
    }

    public void Use()
    {
        Amount--;
    }

    public void SetRebuildItem(RebuildItem item = null)
    {
        rebuildItem = item;
    }

    public RebuildItem GetRebuildItem()
    {
        return rebuildItem;
    }
    
   
}
