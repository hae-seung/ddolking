using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopup : MonoBehaviour
{
    [Header("레이캐스터 블락")] 
    [SerializeField] private GameObject raycastBlocker;
    
    [Header("팝업")] 
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI popupTypeTxt;
    
    
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button plusBtn;
    [SerializeField] private Button minusBtn;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button cancelBtn;


    private string buyText = "구매";
    private string sellText = "판매";
    private int maxAmount;
    public event Action<int> onConfirmationOK;
    
    
    public void Init()
    {
        InitEvents();
        HidePanel();
    }

    private void InitEvents()
    {
        confirmBtn.onClick.AddListener(HidePanel);
        confirmBtn.onClick.AddListener(() => onConfirmationOK?.Invoke(int.Parse(inputField.text)));
        
        cancelBtn.onClick.AddListener(HidePanel);
        
        //plus btn
        plusBtn.onClick.AddListener(() =>
        {
            int.TryParse(inputField.text, out int amount);
            if (amount < maxAmount)
            {
                //Shift 누르면 10씩 증가
                int nextAmount = Input.GetKey(KeyCode.LeftShift) ? amount + 10 : amount + 1;
                if (nextAmount > maxAmount)
                    nextAmount = maxAmount;
                inputField.text = nextAmount.ToString();
            }
        });
        
        //minus btn
        minusBtn.onClick.AddListener(() =>
        {
            int.TryParse(inputField.text, out int amount);
            if (amount > 1)
            {
                int nextAmount = Input.GetKey(KeyCode.LeftShift) ? amount - 10 : amount - 1;
                if (nextAmount <= 1)
                    nextAmount = 1;
                inputField.text = nextAmount.ToString();
            }
        });
        
        //inputField limit
        inputField.onValueChanged.AddListener(str =>
        {
            int.TryParse(str, out int amount);
            bool flag = false;

            if (amount < 1)
            {
                flag = true;
                amount = 1;
            }
            else if (amount > maxAmount)
            {
                flag = true;
                amount = maxAmount;
            }

            if (flag)
                inputField.text = amount.ToString();
        });
    }

    
    public void OpenPopup(ItemData item, int count, ShopMode shopMode,Action<int> onConfirm)
    {
        onConfirmationOK = onConfirm;
        itemName.text = item.Name;
        itemImage.sprite = item.IconImage;

        if (shopMode == ShopMode.buy)
        {
            popupTypeTxt.text = buyText;
        }
        else
        {
            popupTypeTxt.text = sellText;
        }
        
        maxAmount = count;
        inputField.text = "1";
        
        ShowPanel();
    }
    
    
    private void ShowPanel()
    {
        gameObject.SetActive(true);
        raycastBlocker.SetActive(true);
    } 
    private void HidePanel()
    {
        gameObject.SetActive(false);
        raycastBlocker.SetActive(false);
    }
    
}
