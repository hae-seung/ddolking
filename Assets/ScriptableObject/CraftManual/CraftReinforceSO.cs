using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CraftReinforceSO", menuName = "SO/Craft")]
public class CraftReinforceSO : ScriptableObject
{
    [SerializeField] private int maxLevel;
    [SerializeField] private Sprite[] toolImages;
    [SerializeField] private float[] efficients;
    [SerializeField] private ItemData[] reinforceNeedItem;

    public int MaxLevel => maxLevel;
    public Sprite[] ToolImages => toolImages;
    public float[] Efficient => efficients;
    public ItemData[] ReinforceDatas => reinforceNeedItem;
}
