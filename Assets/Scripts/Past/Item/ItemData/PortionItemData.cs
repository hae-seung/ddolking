using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PortionItemData", menuName = "Scriptable Object/PortionItemData", order = int.MaxValue)]
public class PortionItemData : CountableItemData
{
    [SerializeField] private float value;

    public float Value => value;
}
