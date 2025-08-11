using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ChestSlotType
{
    inven,
    chest
}


public class ChestSlot : MonoBehaviour
{
    [SerializeField] private RectTransform slotRect;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject highlight;
    [SerializeField] private TextMeshProUGUI amountTxt;
        
    private ChestSlotType type;
    
    
    public ChestSlotType GetSlotType => type;
    public RectTransform SlotRect => slotRect;
    public bool IsUsing => item != null;
    
    
    //인벤토리 슬롯이라면 인벤토리의 실제 인덱스를
    //상자라면 상자의 실제 인덱스를 가짐
    public int Index { get; private set; }
    
    private Item item;
    

    /// <summary>
    /// 현재 아이템이 슬롯에 있는지 없는지 모를때 호출
    /// </summary>
    /// <param name="index"></param>
    public void InitInvenSlot(int index)
    {
        type = ChestSlotType.inven;
        Index = index;
        UpdateInvenSlot();
    }

    public void InitChestSlot(int index, Chest chest)
    {
        type = ChestSlotType.chest;
        Index = index;
        UpdateChestSlot(chest);
    }
    
    public Item GetItem()
    {
        return item;
    }
    
    public void Highlight(bool state)
    {
        highlight.SetActive(state);
    }
    

    public void UpdateChestSlot(Chest chest)
    {
        item = chest.GetItem(Index);

        if (item == null)
        {
            icon.gameObject.SetActive(false);
        }
        else
        {
            icon.sprite = item.itemData.IconImage;

            if (item is CountableItem citem)
                amountTxt.text = citem.Amount.ToString();
            else
                amountTxt.text = "1";
            
            icon.gameObject.SetActive(true);
        }
    }

    public void UpdateInvenSlot()
    {
        item = Inventory.Instance.GetItem(Index);

        if (item == null)
        {
            icon.gameObject.SetActive(false);
        }
        else
        {
            icon.sprite = item.itemData.IconImage;

            if (item is CountableItem citem)
                amountTxt.text = citem.Amount.ToString();
            else
                amountTxt.text = "1";
            
            icon.gameObject.SetActive(true);
        }
    }
}
