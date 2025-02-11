using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    [Header("아이템버리기 팝업")] 
    [SerializeField] private GameObject popup;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button plusBtn;
    [SerializeField] private Button minusBtn;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button cancelBtn;

    private int maxAmount;
    
    
    
    

    public void ShowPanel() => gameObject.SetActive(true);
    public void HidePanel() => gameObject.SetActive(false);

    private void ShowTrashPopup() => popup.SetActive(true);
    private void HideTrashPopup() => popup.SetActive(false);

    public event Action<int> onConfirmationOK;
    
    
    public void FirstAwake()
    {
        InitEvents();
        HidePanel();
        HideTrashPopup();
    }

    private void InitEvents()
    {
        confirmBtn.onClick.AddListener(HidePanel);
        confirmBtn.onClick.AddListener(HideTrashPopup);
        confirmBtn.onClick.AddListener(() => onConfirmationOK?.Invoke(int.Parse(inputField.text)));
        
        cancelBtn.onClick.AddListener(HidePanel);
        cancelBtn.onClick.AddListener(HideTrashPopup);
        
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

    public void OpenTrashPopup(string itemName, int count, Action<int> onConfirm)
    {
        onConfirmationOK = onConfirm;
        this.itemName.text = itemName;
        
        maxAmount = count;
        inputField.text = "1";
        
        ShowPanel();
        ShowTrashPopup();
    }
    
}
