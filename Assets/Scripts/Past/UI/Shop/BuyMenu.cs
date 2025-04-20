using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BuyMenu", menuName = "SO/Shop/Buy")]
public class BuyMenu : ScriptableObject
{
    [SerializeField] private List<BuyItem> buyItems;
    public List<BuyItem> GetItems => buyItems;
}


[Serializable]
public class BuyItem
{
    public ItemData buyItem;
    public int price;
    public int maxAmount;
}


public class ShopBuyItem //BuyItem을 실체화 시키는 실제 객체
{
    public ItemData itemData;
    public int price;
    public int remainAmount;
    
    
    public ShopBuyItem(BuyItem buyItem)
    {
        itemData = buyItem.buyItem;
        this.price = buyItem.price;
        remainAmount = buyItem.maxAmount;
    }
}