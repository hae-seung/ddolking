using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftManualItemBtn : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameTxt;
    [SerializeField] private Image buttonImage;
    

    public void SetBtn(CraftItemSO craftItem, CraftTableLog tableLog, Color buttonColor)
    {
        itemImage.sprite = craftItem.CraftItemData.IconImage;
        itemNameTxt.text = craftItem.CraftItemData.Name;
        buttonImage.color = buttonColor;
        
        button.onClick.AddListener(() =>
        {
            tableLog.SetCraftItemMenu(craftItem);
        });
    }

   
}
