using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftManualSO", menuName = "SO/CraftItem/ManualSO")]
public class CraftManualSO : ScriptableObject
{
    [SerializeField] private CraftItemSO craftItemSo;
}
