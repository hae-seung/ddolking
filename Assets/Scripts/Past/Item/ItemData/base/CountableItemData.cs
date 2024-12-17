using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CountableItemData", menuName = "Scriptable Object/CountableItemData", order = int.MaxValue)]
public class CountableItemData : ItemData
{
    [SerializeField] private int maxAmount = 99;

    public int MaxAmount => maxAmount;
}
