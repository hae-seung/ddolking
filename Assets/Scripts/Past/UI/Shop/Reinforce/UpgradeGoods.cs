using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeGoods : MonoBehaviour
{
    [SerializeField] private GameObject moneySet;
    [SerializeField] private GameObject essenceSet;

    [SerializeField] private TextMeshProUGUI moneyTxt;
    [SerializeField] private TextMeshProUGUI essenceTxt;

    [Header("에센스 종류")]
    [SerializeField] private GameObject lowEssence;
    [SerializeField] private GameObject highEssence;
    [SerializeField] private ItemData lowEssenceData;
    [SerializeField] private ItemData highEssenceData;

    
    public bool CanUpgrade { get; private set; }

    private int needMoney;
    private Essence needEssence;
    private int needEssenceAmount;
    
    public void Init()
    {
        CanUpgrade = false;
        moneySet.SetActive(false);
        essenceSet.SetActive(false);
        lowEssence.SetActive(false);
        highEssence.SetActive(false);
    }


    public void UpdateSet(EquipItem item)
    {
        lowEssence.SetActive(false);
        highEssence.SetActive(false);
        
        if (!item.EquipData.isEnhanceable)
            return;

        if (item.curLevel == 5)
        {
            CanUpgrade = false;
            moneySet.SetActive(false);
            essenceSet.SetActive(false);
            return;
        }
        
        ItemEnhancementLogic logic = item.GetLogic;
        
        int curMoney = PlayerWallet.Instance.PastMoney;
        needMoney = logic.GetItemEnhanceLogic()[item.curLevel].needGold;
        
        
        moneyTxt.text = $"{curMoney} / {needMoney}";

        needEssence = logic.GetItemEnhanceLogic()[item.curLevel].essence;
        needEssenceAmount = logic.GetItemEnhanceLogic()[item.curLevel].needEssence;
        int curEssenceAmount = 0;
        
        
        switch (needEssence)
        {
            case Essence.low:
                lowEssence.SetActive(true);
                curEssenceAmount = Inventory.Instance.GetItemTotalAmount(lowEssenceData);
                break;
            case Essence.high:
                highEssence.SetActive(true);
                curEssenceAmount = Inventory.Instance.GetItemTotalAmount(highEssenceData);
                break;
        }

        essenceTxt.text = $"{curEssenceAmount} / {needEssenceAmount}";
        
        
        moneySet.SetActive(true);
        essenceSet.SetActive(true);

        CanUpgrade = curMoney >= needMoney && curEssenceAmount >= needEssenceAmount && item.curLevel < 5;

        moneyTxt.color = curMoney < needMoney ? Color.red : Color.black;
        essenceTxt.color = curEssenceAmount < needEssenceAmount ? Color.red : Color.black;
    }

    public void SpendGoods()
    {
        PlayerWallet.Instance.SpendMoney(MoneyType.past, needMoney);

        switch (needEssence)
        {
            case Essence.low:
                Inventory.Instance.RemoveItem(lowEssenceData, needEssenceAmount);
                break;
            case Essence.high:
                Inventory.Instance.RemoveItem(highEssenceData, needEssenceAmount);
                break;
        }
    }
}
