using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PastShop : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] private Button buyBtn;
    [SerializeField] private Button sellBtn;
    [SerializeField] private Button exchangeBtn;
    [SerializeField] private Button specialBtn;
    [SerializeField] private Button exitBtn;

    [Header("탭")] 
    [SerializeField] private ItemView itemView;
    [SerializeField] private ExchangeView exchangeView;

    [Header("버튼선택 색상")]
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectColor;

    
    [Header("팝업")]
    [SerializeField] private ShopPopup _popup;
    
    
    private bool isInit = false;
    private Button currentSelectBtn = null;
    

    public void OpenShop()
    {
        if (!isInit)
            Init();

        //기본 선택 버튼은 Buy
        if (currentSelectBtn != null)
            currentSelectBtn.image.color = normalColor;
        buyBtn.image.color = selectColor;
        currentSelectBtn = buyBtn;
        
        //기본 띄워지는 창은 itemView
        exchangeView.gameObject.SetActive(false);
        itemView.OpenBuyTab();
        
        gameObject.SetActive(true);
    }

    private void Init()
    {
        ButtonInit();
        itemView.Init(_popup);
        _popup.Init();
        
        
        isInit = true;
    }

    private void ButtonInit()
    {
        buyBtn.onClick.AddListener(() =>
        {
            if (currentSelectBtn != null)
            {
                currentSelectBtn.image.color = normalColor;
            }
            buyBtn.image.color = selectColor;
            currentSelectBtn = buyBtn;
            
            exchangeView.gameObject.SetActive(false);
            itemView.OpenBuyTab();
        });
        
        sellBtn.onClick.AddListener(() =>
        {
            if (currentSelectBtn != null)
            {
                currentSelectBtn.image.color = normalColor;
            }
            sellBtn.image.color = selectColor;
            currentSelectBtn = sellBtn;
            
            exchangeView.gameObject.SetActive(false);
            itemView.OpenSellTab();
        });
        
        exchangeBtn.onClick.AddListener(() =>
        {
            if (currentSelectBtn != null)
            {
                currentSelectBtn.image.color = normalColor;
            }
            exchangeBtn.image.color = selectColor;
            currentSelectBtn = exchangeBtn;

            exchangeView.OpenExchangeTab();
            itemView.CloseTab();
        });
        
        specialBtn.onClick.AddListener(() =>
        {
            if (currentSelectBtn != null)
            {
                currentSelectBtn.image.color = normalColor;
            }
            specialBtn.image.color = selectColor;
            currentSelectBtn = specialBtn;
            
            exchangeView.gameObject.SetActive(false);
            itemView.OpenSpecialTab();
        });
        
        exitBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.CloseShop();
            gameObject.SetActive(false);
            _popup.gameObject.SetActive(false);
        });
    }
}
