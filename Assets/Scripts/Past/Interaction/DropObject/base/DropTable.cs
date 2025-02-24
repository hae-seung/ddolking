using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropTable
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private int minAmount;
    [SerializeField] private int maxAmount;

    public int DropItemId => _itemData.ID;
    public int MinAmount => minAmount;
    public int MaxAmount => maxAmount;
    public GameObject DropItemPrefab => _itemData.DropObjectPrefab;
}