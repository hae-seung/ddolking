using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AmuletSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Item amulet = null;
    [SerializeField] private Image amuletIcon;
    [SerializeField] private RectTransform rect;
    [SerializeField] private ToolTipUI toolTipUI;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (amulet == null)
            return;
        toolTipUI.SetItemInfo(amulet);
        toolTipUI.SetRectPosition(rect);
        toolTipUI.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipUI.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && amulet != null)
        {
            int amount = Inventory.Instance.Add(amulet);
            if (amount == 1)
            {
                //해제 불가
                Debug.Log("인벤토리가 가득차서 해제 불가능");
                return;
            }

            Inventory.Instance.UnEquipAmulet();
            amulet = null;
            amuletIcon.gameObject.SetActive(false);
        }
    }

    public void UpdateAmulet(Item amuletItem)
    {
        amulet = amuletItem;
        amuletIcon.sprite = amuletItem.itemData.IconImage;
        amuletIcon.gameObject.SetActive(true);
    }
}
