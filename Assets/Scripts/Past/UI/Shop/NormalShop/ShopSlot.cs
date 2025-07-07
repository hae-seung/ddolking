using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ShopMode
{
    buy,
    sell,
}

public class ShopSlot : MonoBehaviour
{
    [Header("슬롯")]
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amountTxt;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI priceText;
    
    
    private ShopPopup _popupUI;
    private ShopMode shopMode;
    private ItemData itemData;

    private string infinite = "\u221e";
    private bool isInfinite = false;//구매용(플레이어가 상점으로부터)
    private int remainAmount;//구매용(상점의 남은 수량)
    private bool hasFreeSlot;//구매용(최소 인벤 2칸은 보장되었음)
    private ShopBuyItem _shopBuyItem;
    private  Action<ShopBuyItem, int> buyItem;


    private int currentHasAmount;//판매용(플레이어가 상점에게)
    private  Action<ItemData, int> sellItem;
    
    public void Init(ShopPopup popupUI)
    {
        _popupUI = popupUI;
        
        button.onClick.AddListener(() =>
        {
            if (!hasFreeSlot)
            {
                UIManager.Instance.ShopWarn(-1); //인벤칸 모자름}
                return;
            }

            if (shopMode == ShopMode.buy)//구매
            {
                if (isInfinite)//무한대로 구매가능일때
                {
                    if (itemData is CountableItemData citemData)
                    {
                        _popupUI.OpenPopup(citemData, citemData.MaxAmount, shopMode ,BuyItem);
                    }
                    else
                    {
                        _popupUI.OpenPopup(itemData, 1, shopMode, BuyItem);
                    }
                }
                else//구매가능 수량이 한정된 경우
                {
                    if (itemData is CountableItemData citemData)
                    {
                        if (citemData.MaxAmount > remainAmount)
                        {
                            _popupUI.OpenPopup(itemData, remainAmount, shopMode ,BuyItem);
                        }
                        else
                        {
                            _popupUI.OpenPopup(itemData, citemData.MaxAmount, shopMode ,BuyItem);
                        }
                    }
                    else
                    {
                        _popupUI.OpenPopup(itemData, 1, shopMode ,BuyItem);
                    }
                   
                }
            }
            else//판매
            {
                _popupUI.OpenPopup(itemData, currentHasAmount, shopMode, SellItem);
            }
        });
    }

    public void SetBuy(ShopBuyItem item, bool hasFreeSlot, Action<ShopBuyItem, int> BuyItem)
    {
        shopMode = ShopMode.buy;
        this.hasFreeSlot = hasFreeSlot;
        buyItem = BuyItem;
        itemData = item.itemData;
        _shopBuyItem = item;
        
        
        nameTxt.text = item.itemData.Name;
        itemImage.sprite = item.itemData.IconImage;
        priceText.text = item.price.ToString();
        
        
        
        remainAmount = item.remainAmount;
        button.interactable = remainAmount != 0;//남은수량이 0일땐 비활성화
        
        
        if (remainAmount < 0) //무한의미
        {
            isInfinite = true;
            amountTxt.text = infinite;
        }
        else
        {
            isInfinite = false;
            amountTxt.text = remainAmount.ToString();
        }
    }

    public void SetSell(Item item, int price,  Action<ItemData, int> SellItem)
    {
        hasFreeSlot = true;
        sellItem = SellItem;
        shopMode = ShopMode.sell;
        
        
        isInfinite = false;
        itemData = item.itemData;
        
        
        
        nameTxt.text = item.itemData.Name;
        itemImage.sprite = item.itemData.IconImage;
        priceText.text = price.ToString();
        
        
        
        if(item is CountableItem citem)
        {
            currentHasAmount = citem.Amount;
            amountTxt.text = $"x{citem.Amount}";
        }
        else
        {
            currentHasAmount = 1;
            amountTxt.text = "x1";
        }
    }


    private void BuyItem(int amount)
    {
        buyItem?.Invoke(_shopBuyItem, amount);
    }

    private void SellItem(int amount)
    {
        sellItem?.Invoke(itemData, amount);
    }
    
}
