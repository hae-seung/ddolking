using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemSet : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject upgradeAmountSet;
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private TextMeshProUGUI nextLevel;
    

    public void Init()
    {
        //끄기
        itemImage.gameObject.SetActive(false);
        upgradeAmountSet.SetActive(false);
    }

    public void UpdateSet(EquipItem item)
    {
        itemImage.sprite = item.itemData.IconImage;
        itemImage.gameObject.SetActive(true);
        
        //아이템이 5렙인경우
        if (item.curLevel >= 5)
        {
            currentLevel.text = "+5";
            nextLevel.text = "+5";
        }
        //그 이하인 경우
        else
        {
            currentLevel.text = $"+{item.curLevel}";
            nextLevel.text = $"+{item.curLevel + 1}";
        }
        
        upgradeAmountSet.SetActive(true);
    }
}
