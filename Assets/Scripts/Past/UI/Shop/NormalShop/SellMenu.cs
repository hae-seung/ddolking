using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemPrice : SerializableDictionary<ItemData, int>{}

[CreateAssetMenu(fileName = "SellMenu", menuName = "SO/Shop/Sell")]
public class SellMenu : ScriptableObject
{
    [SerializeField] private ItemPrice sellItems;
    public ItemPrice GetItems => sellItems;
}




