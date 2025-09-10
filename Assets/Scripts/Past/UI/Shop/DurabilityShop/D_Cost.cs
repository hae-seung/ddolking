using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class D_Cost : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cost;
    
    [Header("클래스별 내구도 10당 가격")]
    [SerializeField] private int normalCost;
    [SerializeField] private int epicCost;
    [SerializeField] private int uniqueCost;
    [SerializeField] private int legendCost;

    public void Init()
    {
        cost.text = "";
    }

    
    public void UpdateSlot(EquipItem item)
    {
        switch (item.EquipData.itemclass)
        {
            case ItemClass.Normal:
                cost.text = $"x {normalCost.ToString()}";
                break;
            case ItemClass.Epic:
                cost.text = $"x {epicCost.ToString()}";
                break;
            case ItemClass.Unique:
                cost.text = $"x {uniqueCost.ToString()}";
                break;
            case ItemClass.Legend:
                cost.text = $"x {legendCost.ToString()}";
                break;
        }
    }
    
}
