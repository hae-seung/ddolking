using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    [Header("상점 메뉴얼")]
    [SerializeField] private BuyMenu buyMenu;
    [SerializeField] private SellMenu sellMenu;
    [SerializeField] private BuyMenu specialMenu;
    
    [Header("슬롯 부모")]
    [SerializeField] private GameObject contentParent;

    [Header("슬롯 프리팹")]
    [SerializeField] private GameObject slotPrefab;
    
    
    
    private ShopPopup popup;
    private bool hasFreeSlot = false;
    
    private List<ShopSlot> slots = new List<ShopSlot>();
    private List<ShopBuyItem> buyItemInstance = new List<ShopBuyItem>();//구매 가능 물품들의 실제 객체(남은양 갱신가능)
    private List<ShopBuyItem> specialItemInstance = new List<ShopBuyItem>();
    
    public void Init(ShopPopup popup)
    {
        this.popup = popup;

        hasFreeSlot = false;
        
        //buy메뉴 슬롯 갯수로 일단 초기화.
        for (int i = 0; i < buyMenu.GetItems.Count; i++)
        {
            ShopSlot newSlot = Instantiate(slotPrefab,
                contentParent.transform).GetComponent<ShopSlot>();

            newSlot.Init(this.popup);
            slots.Add(newSlot);


            //일반구매 아이템
            ShopBuyItem shopBuyItem = new ShopBuyItem(buyMenu.GetItems[i]);
            buyItemInstance.Add(shopBuyItem);
        }

        for (int i = 0; i < specialMenu.GetItems.Count; i++)
        {
            //스페셜 구매 아이템
            ShopBuyItem shopBuyItem = new ShopBuyItem(specialMenu.GetItems[i]);
            specialItemInstance.Add(shopBuyItem);
        }
    }
    
    public void OpenBuyTab()
    {
        //플레이어 인벤토리에 최소 두 칸이라도 남는 공간 있는지 확인하기
        CheckPlayerInventory();
        
        
        //슬롯 모자라면 추가
        if (buyMenu.GetItems.Count > slots.Count)
        {
            int addAmount = buyMenu.GetItems.Count - slots.Count;
            for (int i = 0; i < addAmount; i++)
            {
                ShopSlot newSlot = Instantiate(slotPrefab,
                    contentParent.transform).GetComponent<ShopSlot>();

                newSlot.Init(popup);
                slots.Add(newSlot);
            }
        }
        
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetBuy(buyItemInstance[i], hasFreeSlot, BuyItem);
            slots[i].gameObject.SetActive(i < buyMenu.GetItems.Count);
        }
        
        gameObject.SetActive(true);
    }

    private void CheckPlayerInventory()
    {
        IReadOnlyList<Item> items = Inventory.Instance.Items;
        int freeSlotCnt = 0;
        for (int i = 0; i < Inventory.Instance.SlotCnt; i++)
        {
            if (items[i] == null)
                freeSlotCnt++;
        }

        if (freeSlotCnt >= 2)
            hasFreeSlot = true;
        else
            hasFreeSlot = false;

    }

    public void OpenSellTab()
    {
        IReadOnlyList<Item> items = Inventory.Instance.Items;

        int slotIndex = 0;
        for (int i = 0; i < Inventory.Instance.SlotCnt; i++)
        {
            if (items[i] == null) continue;
            
            // 판매 가능한 아이템만 필터링
            if (sellMenu.GetItems.TryGetValue(items[i].itemData, out int price))
            {
                // 슬롯이 부족하면 생성
                if (slotIndex >= slots.Count)
                {
                    ShopSlot newSlot = Instantiate(slotPrefab, 
                        contentParent.transform).GetComponent<ShopSlot>();
                    newSlot.Init(popup);
                    slots.Add(newSlot);
                }

                // 슬롯 설정 및 활성화
                slots[slotIndex].SetSell(items[i], price, SellItem);
                slots[slotIndex].gameObject.SetActive(true);
                slotIndex++;
            }
        }

        // 남는 슬롯 비활성화
        for (int i = slotIndex; i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
        
        gameObject.SetActive(true);
    }


    public void OpenSpecialTab()//구매기능만 존재 buy랑 같음
    {
        CheckPlayerInventory();
        
        //슬롯 모자라면 추가
        if (specialMenu.GetItems.Count > slots.Count)
        {
            int addAmount = specialMenu.GetItems.Count - slots.Count;
            for (int i = 0; i < addAmount; i++)
            {
                ShopSlot newSlot = Instantiate(slotPrefab,
                    contentParent.transform).GetComponent<ShopSlot>();

                newSlot.Init(popup);
                slots.Add(newSlot);
            }
        }
        
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetBuy(specialItemInstance[i], hasFreeSlot, BuyItem);
            slots[i].gameObject.SetActive(i < specialMenu.GetItems.Count);
        }
        
        gameObject.SetActive(true);
    }
    
    public void CloseTab()
    {
        gameObject.SetActive(false);
    }


    
    
    private void BuyItem(ShopBuyItem item, int amount)
    {
        //실제 구매로직
        //remainAmount를 줄이고 wallet으로부터 돈이 빠져나감.
        //돈 딸리면 최대 몇개 구매 가능한지 경고문 띄우기
        //인벤토리에 아이템 추가해주기

        int totalPrice = item.price * amount;
        if (PlayerWallet.Instance.PastMoney < totalPrice)//돈부족
        {
            int buyableAmount = PlayerWallet.Instance.PastMoney / item.price;
            UIManager.Instance.ShopWarn(buyableAmount);
            return;
        }
        
        PlayerWallet.Instance.SpendMoney(MoneyType.past, totalPrice);
        Item newItem = item.itemData.CreateItem();
        Inventory.Instance.Add(newItem, amount);
        
        item.BuyItem(amount);
        OpenBuyTab();
    }

    private void SellItem(ItemData data, int amount)
    {
        //실제 판맨로직
        //인벤토리에서 아이템 빼기
        //wallet에 돈 채워넣기
        //OpenSellTab호출

        int totalPrice = sellMenu.GetItems[data] * amount;
        
        Inventory.Instance.RemoveItem(data, amount);
        PlayerWallet.Instance.GetMoney(MoneyType.past, totalPrice);
        OpenSellTab();
    }
    
}
