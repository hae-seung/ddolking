using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CraftNeedItem
{
    public ItemData itemData;
    public int amount;
}

[CreateAssetMenu(fileName = "CraftItemSO", menuName = "SO/CraftItem/CraftItemSO")]
public class CraftItemSO : ScriptableObject
{
    [Header("만들 아이템데이터")] 
    [SerializeField] private ItemData itemData;
    [SerializeField] private GameObject itemPrefab;

    [Header("만들기 위해 필요한 아이템 목록 데이터")] 
    [SerializeField] private List<CraftNeedItem> craftNeedItems;
}
