
using UnityEngine;
using UnityEngine.UI;

public class ReinforceShop : MonoBehaviour
{
    [SerializeField] private ItemList itemList;
    [SerializeField] private UpgradeItemSet upgradeItemSet;
    [SerializeField] private UpgradeGoods upgradeGoods;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private UpgradeItemExplain explain;


    private EquipItem item;
    private ReinforceSlot curSlot;
    
    private void Start()
    {
        //버튼 초기화 및 강화버튼은 비활성화까지
        exitBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.CloseShop();
            gameObject.SetActive(false);
        });
        
        upgradeBtn.onClick.AddListener(UpgradeStart);
        upgradeBtn.interactable = false;
    }

    private void UpgradeStart()
    {
        //업글 가능한지 재화 확인.
        //업글 가능하다면 재화 소모, 업글 및 상황 업데이트

        upgradeGoods.SpendGoods();
        item.LevelUp();
        curSlot.LevelUp();
        
        if (item.curLevel >= 5)
        {
            upgradeBtn.interactable = false;
        }
        
        upgradeItemSet.UpdateSet(item);
        upgradeGoods.UpdateSet(item);
        explain.UpdateSet(item);
    }
    

    public void OpenShop()
    {
        Init();
    }

    private void Init()
    {
        itemList.InitSlot();
        upgradeItemSet.Init();
        upgradeGoods.Init();
        explain.Init();

        upgradeBtn.interactable = false;
        
        gameObject.SetActive(true);
    }


    
    //강화할 아이템을 골랐을 때
    public void UpdateAll(ReinforceSlot slot)
    {
        curSlot = slot;
        item = slot.GetItem() as EquipItem;
        
        //모든 업데이트 시작
        upgradeItemSet.UpdateSet(item);
        upgradeGoods.UpdateSet(item);
        explain.UpdateSet(item);

        upgradeBtn.interactable = upgradeGoods.CanUpgrade;
    }
    
}
