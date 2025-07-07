
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
        upgradeBtn.enabled = false;
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
            upgradeBtn.enabled = false;
        }
        
        upgradeItemSet.UpdateSet(item);
        upgradeGoods.UpdateSet(item);
        explain.UpdateSet(item);
        
        if (upgradeGoods.CanUpgrade)
            upgradeBtn.enabled = true;
        
        
    }

    public void OnEnable()
    {
        Init();
    }

    public void OpenShop()
    {
        Init();
    }

    private void Init()
    {
        itemList.UpdateSlot();
        upgradeItemSet.Init();
        upgradeGoods.Init();
        explain.Init();
    }


    public void UpdateAll(ReinforceSlot slot)
    {
        curSlot = slot;
        item = slot.GetItem() as EquipItem;
        
        if(item == null)
            Debug.Log("아닌뎅"); 
        
        //모든 업데이트 시작
        upgradeItemSet.UpdateSet(item);
        upgradeGoods.UpdateSet(item);
        explain.UpdateSet(item);

        if (upgradeGoods.CanUpgrade)
            upgradeBtn.enabled = true;
    }
    
}
