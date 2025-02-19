using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    [Header("슬롯 이미지")] 
    [SerializeField] private Image slotImage;
    
    [Header("아이템 아이콘 이미지")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;
    
    private void ShowIcon() => iconImage.gameObject.SetActive(true);
    private void HideIcon() => iconImage.gameObject.SetActive(false);

    private void ShowText() => amountText.gameObject.SetActive(true);
    private void HideText() => amountText.gameObject.SetActive(false);

    public bool IsUsing { get; private set; } = false;


    public void SetSlotImage(Color color)
    {
        slotImage.color = color;
    }
    
    public void UpdateAmount(int amount  = 0)//countableItem만 실행함
    {
        if(amount == 0)
            HideText();
        else
        {
            amountText.text = amount.ToString();
            ShowText();
        }
    }

    public void SetItemIcon(Sprite sprite)
    {
        iconImage.sprite = sprite;
        ShowIcon();
    }

    public void RemoveItem()
    {
        HideIcon();
        HideText();
        iconImage.sprite = null;
    }

    public void HideAmountText()
    {
        HideText();
    }
}
