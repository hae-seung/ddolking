using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DurabilityInfo : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI currentDu;
    [SerializeField] private TextMeshProUGUI maxDu;



    public void Init()
    {
        currentDu.text = "";
        maxDu.text = "";
    }


    public void UpdateSlot(EquipItem item)
    {
        currentDu.text = $"{item.CurDurability}";
        maxDu.text = $"{item.MaxDurability}";
    }
    
}
