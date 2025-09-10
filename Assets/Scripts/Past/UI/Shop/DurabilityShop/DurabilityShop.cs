
using UnityEngine;
using UnityEngine.UI;

public class DurabilityShop : MonoBehaviour
{
    [SerializeField] private D_ItemList itemList;
    [SerializeField] private D_Slot itemSlot;
    [SerializeField] private D_Cost repairCost;
    [SerializeField] private DurabilityInfo durabilityInfo;

    [SerializeField] private Button repairBtn;
    [SerializeField] private Button exitBtn;

    
    private EquipItem item;
    private ReinforceSlot curSlot;


    private int needMoney = 0;
    private int curMoney = 0;

    private void Start()
    {
        exitBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            UIManager.Instance.CloseShop();
        });
        
        repairBtn.onClick.AddListener(() =>
        {
            PlayerWallet.Instance.SpendMoney(MoneyType.past, needMoney);
            Repair();
            
            itemSlot.UpdateSlot(item);
            repairCost.UpdateSlot(item);
            durabilityInfo.UpdateSlot(item);
        });
    }

    private void Repair()
    {
        //내구도 수리
        item.RepairDurability();
    }


    private void Init()
    {
        //창이 열렸을때만 실행
        itemList.InitSlot();
        itemSlot.Init();
        repairCost.Init();
        durabilityInfo.Init();

        repairBtn.interactable = false;
        
        gameObject.SetActive(true);
    }

    public void OpenShop()
    {
        Init();
    }

    public void UpdateAll(ReinforceSlot slot)
    {
        curSlot = slot;
        item = slot.GetItem() as EquipItem;
        
        //모든 업데이트 시작
        itemSlot.UpdateSlot(item);
        repairCost.UpdateSlot(item);
        durabilityInfo.UpdateSlot(item);


        if (item.CurDurability == item.MaxDurability)
        {
            repairBtn.interactable = false;
            return;
        }

        curMoney = PlayerWallet.Instance.PastMoney;
        
        switch (item.EquipData.itemclass)
        {
            //가격은 바뀔 수 있음
            case ItemClass.Normal:
                needMoney = 5;
                break;
            case ItemClass.Epic:
                needMoney = 10;
                break;
            case ItemClass.Unique:
                needMoney = 15;
                break;
            case ItemClass.Legend:
                needMoney = 20;
                break;
        }

        repairBtn.interactable = curMoney >= needMoney;
    }
}
