using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private RectTransform iconRect;
    [SerializeField] private TextMeshProUGUI amountText;
    
    public int SlotIdx { get; private set; }
    public bool IsUsing { get; private set; } = false;
    
    private void ShowIcon() => iconImage.gameObject.SetActive(true);
    private void HideIcon() => iconImage.gameObject.SetActive(false);
    private void ShowText() => amountText.gameObject.SetActive(true);
    private void HideText() => amountText.gameObject.SetActive(false);
    
    public RectTransform IconRect => iconRect;

    public void SetIndex(int idx)
    {
        SlotIdx = idx;
    }

    public void CreateItem(Item item)
    {
        if (item is CountableItem citem)
        {
            amountText.text = citem.Amount.ToString();
            ShowText();
        }
        
        iconImage.sprite = item.itemData.IconImage;
        ShowIcon();

        IsUsing = true;
    }

    public void UpdateItemAmount(int amount = 0)
    {
        if(amount == 0)
            HideText();
        else
        {
            amountText.text = amount.ToString();
            ShowText();
        }
    }

    public void SwapOrMoveIcon(Slot other)
    {
        if (other == null) return;

        Sprite tempSprite = iconImage.sprite;

        SetItem(other.iconImage.sprite);
        other.SetItem(tempSprite);
    }

    private void SetItem(Sprite sprite)
    {
        if (sprite != null)
        {
            iconImage.sprite = sprite;
            IsUsing = true;
            ShowIcon();
        }
        else
        {
            RemoveItem();
        }
    }

    private void RemoveItem()
    {
        HideIcon();
        HideText();
        IsUsing = false;
        iconImage.sprite = null;
    }

    public void SetItemIcon(Sprite sprite)
    {
        iconImage.sprite = sprite;
        ShowIcon();
    }

    public void RemoveItem(int idx)
    {
        HideIcon();
        HideText();
    }

    public void HideAmountText(int idx)
    {
        HideText();
    }
}