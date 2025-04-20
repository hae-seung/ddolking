using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftManualItemBtn : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameTxt;
    [SerializeField] private Image buttonImage;

    private CraftItemSO _craftItem;

    public void SetBtn(CraftItemSO craftItem, Color buttonColor)
    {
        itemImage.sprite = craftItem.CraftItemData.IconImage;
        itemNameTxt.text = craftItem.CraftItemData.Name;
        buttonImage.color = buttonColor;
        _craftItem = craftItem;
    }

    public void Init(CraftTableLog tableLog)
    {
        button.onClick.AddListener(() =>
        {
            tableLog.SetCraftItemMenu(_craftItem);
        });
    }
   
}
