using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CTSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject highLight;
    [SerializeField] private TextMeshProUGUI pb;
    
    
    public void SetSlot(ItemData data, float basicPb)
    {
        icon.sprite = data.IconImage;
        highLight.SetActive(false);
        pb.text = $"{Mathf.RoundToInt(basicPb)}%";
    }

    public void HighLight()
    {
        highLight.SetActive(true);
        pb.text = "50%";
    }

    public void OffHighLight(float basicPb)
    {
        highLight.SetActive(false);
        pb.text = $"{Mathf.RoundToInt(basicPb)}%";
    }
}
