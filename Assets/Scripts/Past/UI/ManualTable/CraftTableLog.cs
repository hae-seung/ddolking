using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftTableLog : MonoBehaviour
{
    [Header("테이블 로그")] 
    [SerializeField] private Image craftItemImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI itemNameTxt;
    [SerializeField] private TextMeshProUGUI itemDescriptionTxt;
    [SerializeField] private List<CraftTableNeedItem> needItemLists;

    [Header("갯수 증감 및 확인")] 
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button plusBtn;
    [SerializeField] private Button minusBtn;
    [SerializeField] private TextMeshProUGUI amountTxt;

    private CraftItemSO craftItemData;
    private List<CraftNeedItem> craftNeedItems;
    private int maxAmount = 0;
    private bool isCraftable = true;

    private Action<CraftItemSO, int> MakeItem = null;
    
    private void Awake()
    {
        InitBtns();
    }
    
    //선택된 아이템이 바뀌면 호출
    public void SetCraftItemMenu(CraftItemSO craftItem)
    {
        HideLists();
        
        //데이터 저장
        craftItemData = craftItem;
        craftNeedItems = craftItem.CraftNeedItems;
        isCraftable = true;
        
        //갯수 설정
        if (craftItem.CraftItemData is CountableItemData cData)
            maxAmount = cData.MaxAmount;
        else
            maxAmount = 1;
        amountTxt.text = "1";


        
        //아이템 리스트 설정
        itemDescriptionTxt.text = craftItem.CraftItemData.Description;
        craftItemImage.sprite = craftItem.CraftItemData.IconImage;
        itemNameTxt.text = craftItem.CraftItemData.Name;
        for (int i = 0; i < craftNeedItems.Count; i++)
        {
            needItemLists[i].SetNeedItem(craftNeedItems[i]);
            needItemLists[i].gameObject.SetActive(true);
            needItemLists[i].SetItemAmount(craftNeedItems[i], 1);
            
            if (!needItemLists[i].HasEnoughItems)
                isCraftable = false;
        }

        confirmBtn.interactable = isCraftable;
        
        //테이블로그 Open
        gameObject.SetActive(true);
    }
    
    
    private void HideLists()
    {
        for(int i = 0; i<needItemLists.Count; i++)
            needItemLists[i].gameObject.SetActive(false);
    }

    private void InitBtns()
    {
        plusBtn.onClick.AddListener(() =>
        {
            int.TryParse(amountTxt.text, out int amount);
            
            if (amount < maxAmount)
            {
                //Shift 누르면 10씩 증가
                int nextAmount = Input.GetKey(KeyCode.LeftShift) ? amount + 10 : amount + 1;
                if (nextAmount > maxAmount)
                    nextAmount = maxAmount;
                amountTxt.text = nextAmount.ToString();
                
                SetNeedItemsAmount(nextAmount);
            }
        });
        
        minusBtn.onClick.AddListener(() =>
        {
            int.TryParse(amountTxt.text, out int amount);
            
            if (amount > 1)
            {
                int nextAmount = Input.GetKey(KeyCode.LeftShift) ? amount - 10 : amount - 1;
                if (nextAmount <= 1)
                    nextAmount = 1;
                amountTxt.text = nextAmount.ToString();
                
                SetNeedItemsAmount(nextAmount);
            }
        });
        
        confirmBtn.onClick.AddListener(() =>
        {
            int.TryParse(amountTxt.text, out int amount);
            
            MakeItem?.Invoke(craftItemData, amount);
            MakeItem = null;
            UIManager.Instance.CloseCraftTab();
            
            //아이템 수량 감소
            for (int i = 0; i < craftNeedItems.Count; i++)
            {
                Inventory.Instance.RemoveItem(craftNeedItems[i].itemData, needItemLists[i].ItemTotalAmount);
            }
        });
    }

    private void SetNeedItemsAmount(int multiplier)
    {
        isCraftable = true;
        for (int i = 0; i < craftNeedItems.Count; i++)
        {
            needItemLists[i].SetItemAmount(craftNeedItems[i], multiplier);
            if (!needItemLists[i].HasEnoughItems)
            {
                isCraftable = false;
            }
        }

        confirmBtn.interactable = isCraftable;
    }

    public void SetImage(Sprite backGroundSprite)
    {
        backgroundImage.sprite = backGroundSprite;
    }

    public void SetConfirmEvent(Action<CraftItemSO, int> makeItem)
    {
        MakeItem = makeItem;
    }
}
