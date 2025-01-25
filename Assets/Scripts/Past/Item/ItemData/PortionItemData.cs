using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PortionItemData", menuName = "SO/CountableItemData/PortionItemData", order = int.MaxValue)]
public class PortionItemData : CountableItemData
{
    [SerializeField] private float value;

    public float Value => value;
    
}
