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

        SetItemIcon(other.iconImage.sprite);
        other.SetItemIcon(tempSprite);
    }

    public void SetItemIcon(Sprite otherSprite)
    {
        if (otherSprite != null)
        {
            iconImage.sprite =  otherSprite;
            IsUsing = true;                         //중요
            ShowIcon();
        }
        else//다른 슬롯엔 아이템이 없었음을 의미
        {
            RemoveItem();
        }
    }

    public void RemoveItem()
    {
        HideIcon();
        HideText();
        IsUsing = false;                            //중요
        iconImage.sprite = null;
    }
    
    

    public void HideAmountText()
    {
        HideText();
    }
}