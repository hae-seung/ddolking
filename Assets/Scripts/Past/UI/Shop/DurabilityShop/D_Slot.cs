using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class D_Slot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemClass;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemUpgradeAmount;

    [SerializeField] private GameObject itemSlot;

    
    public void Init()
    {
        //숨기기
        itemClass.text = "";
        itemSlot.SetActive(false);
    }


    public void UpdateSlot(EquipItem item)
    {
        itemImage.sprite = item.itemData.IconImage;
        SetItemClassText(item.EquipData.itemclass);
        
        itemUpgradeAmount.text = item.curLevel == 0 ? "" : $"+{item.curLevel}";
    }
    
    
    private void SetItemClassText(ItemClass itemClass)
    {
        switch (itemClass)
        {
            case ItemClass.Normal:
                this.itemClass.text = "일반";
                this.itemClass.color = Color.gray;
                break;
            case ItemClass.Epic:
                this.itemClass.text = "고급";
                this.itemClass.color = new Color32(138, 43, 226, 255);
                break;
            case ItemClass.Unique:
                this.itemClass.text = "희귀";
                this.itemClass.color = Color.yellow;
                break;
            case ItemClass.Legend:
                this.itemClass.text = "전설";
                this.itemClass.color = Color.green;
                break;
        }
        itemSlot.SetActive(true);
    }
}
