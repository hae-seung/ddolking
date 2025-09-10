using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnchantShop : MonoBehaviour
{
    [SerializeField] private AmuletItemData emptyAmulet;
    [SerializeField] private AmuletItemData enchantAmulet;
    
    [SerializeField] private GameObject enchant;
    [SerializeField] private GameObject explain;

    [SerializeField] private EnchantSlot enchantSlot;
    [SerializeField] private int price;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private RectTransform contentParent;
    [SerializeField] private EnchantListSlot listSlotPrefab;

    [SerializeField] private Button exitBtn;
    [SerializeField] private Button questionBtn;
    [SerializeField] private Button enchantBtn;


    private List<EnchantListSlot> slots = new List<EnchantListSlot>();
    private Item unEnchantAmulet;

    private bool isButtonInit = false;
    private bool isOpenExplain = false;
    
    [ContextMenu("OpenSHop")]
    public void OpenShop()
    {
        if (!isButtonInit)
            InitButtons();

        enchantBtn.interactable = false;
        isOpenExplain = false;
        
        enchant.SetActive(true);
        explain.SetActive(false);
        gameObject.SetActive(true);

        enchantSlot.Init();
        InitSlots();
    }

    private void InitButtons()
    {
        exitBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.CloseShop();
            gameObject.SetActive(false);
        });
        
        questionBtn.onClick.AddListener(() =>
        {
            explain.SetActive(!isOpenExplain);
            isOpenExplain = !isOpenExplain;
        });

        enchantBtn.onClick.AddListener(Enchant);
        

        unEnchantAmulet = emptyAmulet.CreateItem();
        priceText.text = $"x {price}";
        
        isButtonInit = true;
    }

    private void InitSlots()
    {
        int itemCnt = Inventory.Instance.GetItemTotalAmount(emptyAmulet);

        // 슬롯 개수가 모자란 경우 → 새 슬롯 생성
        for (int i = slots.Count; i < itemCnt; i++)
        {
            EnchantListSlot slot = Instantiate(listSlotPrefab, contentParent);
            slots.Add(slot);
            slots[i].Init(emptyAmulet ,UpToEnchantSlot);
        }

        // 슬롯 활성/비활성 처리
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(i < itemCnt);
        }
    }

    
    //인챈트 슬롯으로 아이템 옮기기
    private void UpToEnchantSlot()
    {
        enchantSlot.SwapTo(unEnchantAmulet);
        InitSlots();
        CheckMoney();
    }

    private void CheckMoney()
    {
        enchantBtn.interactable = PlayerWallet.Instance.PastMoney >= price;
    }

    private void Enchant()
    {
        //돈 빠져나가기, 빈 아뮬렛 인벤에서 제거
        PlayerWallet.Instance.SpendMoney(MoneyType.past, price);
        Inventory.Instance.RemoveItem(emptyAmulet, 1);
        
        //강화된 아뮬렛 만들어서 인벤에 넣기
        AmuletItem amulet = enchantAmulet.CreateItem() as AmuletItem;
        List<StatModifier> stats = MakeNewStat();
        amulet.OverrideStatModifier(stats);
        Inventory.Instance.Add(amulet, 1);
        
        //enchantSlot에 강화된 아뮬렛 정보 넣어주기
        enchantSlot.SetEnchantAmulet(amulet);
        
        //InitSlots실행
        InitSlots();
        CheckMoney();
    }

    private List<StatModifier> MakeNewStat()
    {
        List<StatModifier> s = new List<StatModifier>();
        //강화목록에 따라서 랜덤으로 인챈트 능력 부여.
        //하드코딩 필요
        
        return s;
    }
}
