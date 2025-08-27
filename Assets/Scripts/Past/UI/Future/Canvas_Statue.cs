using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Statue : MonoBehaviour
{
    [SerializeField] private Image statueImage;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI timeZoneTxt;
    [SerializeField] private TextMeshProUGUI explainTxt;
    [SerializeField] private Button buyBtn;

    [SerializeField] private GameObject selectObject;
    [SerializeField] private Button okBtn;
    [SerializeField] private Button noBtn;
    [SerializeField] private Button closeBtn;
    
    [SerializeField] private GameObject statueObject;
    
    [TextArea][SerializeField] private string willBuytext;

    [SerializeField] private int price;
    [SerializeField] private ItemData nameTagItemData;
    
    private Action Buy = null;  //버튼 누르면 Talk발생
    private Action OkBtn = null; //실제 구매 확정시 발생
    
    private void Awake()
    {
        buyBtn.onClick.AddListener(SetBuyScript);
        okBtn.onClick.AddListener(OnClickOkBtn);
        noBtn.onClick.AddListener(OnClickNoBtn);
        closeBtn.onClick.AddListener(Close);
        
        statueObject.SetActive(false);
        buyBtn.gameObject.SetActive(false);
        selectObject.SetActive(false);
    }

    public void OpenStatue(StatueData data, Action BuyStatue = null)
    {
        buyBtn.gameObject.SetActive(false);
        
        statueImage.sprite = data.StatueImage;
        nameTxt.text = data.Name;
        timeZoneTxt.text = data.Day;
        explainTxt.text = data.Explain;

        if(BuyStatue != null)
        {
            OkBtn = BuyStatue;
            buyBtn.gameObject.SetActive(true);
        }
        
        statueObject.SetActive(true);
    }

    private void SetBuyScript()
    {
        explainTxt.text = willBuytext;
        selectObject.SetActive(true);
    }


    private void OnClickOkBtn()
    {
        //돈 있는지 확인
        int curMoney = PlayerWallet.Instance.ModernMoney;
        if (curMoney < price)
        {
            explainTxt.text = "돈이 부족하군요";
            return;
        }

        //인벤토리 공간 있는지 확인
        if (Inventory.Instance.GetEmptySlotAmount() <= 0)
        {
            explainTxt.text = "인벤토리 공간이 부족하군요";
            return;
        }

        Item item = nameTagItemData.CreateItem();
        Inventory.Instance.Add(item, 1);
        PlayerWallet.Instance.SpendMoney(MoneyType.modern, price);
        OkBtn?.Invoke();

        Close();
    }

    private void OnClickNoBtn()
    {
       Close();
    }

    private void Close()
    {
        buyBtn.gameObject.SetActive(false);
        selectObject.SetActive(false);
        statueObject.SetActive(false);
        
        GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
    }
}
