using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineMachineUI : MonoBehaviour
{
    [SerializeField] private GameObject tab;
    [Header("집중채굴 버튼 슬롯")]
    [SerializeField] private GameObject ctSlot;
    [Header("결과 슬롯")] 
    [SerializeField] private GameObject retrieveSlot;

    [Header("집중채굴 content오브젝트")] 
    [SerializeField] private Transform ctParent;
    [Header("결과 content오브젝트")] 
    [SerializeField] private Transform retrieveParent;

    [Header("집중채굴 기름 갯수")] 
    [SerializeField] private TextMeshProUGUI oilText;
    [Header("결과 현재채굴 중")]
    [SerializeField] private TextMeshProUGUI curMineText;
    [Header("결과 저장고")] 
    [SerializeField] private TextMeshProUGUI storeText;
    
    [Header("버튼들")] 
    [SerializeField] private Button startBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button retrieveBtn;

    [Header("오일데이터")] 
    [SerializeField] private ItemData oilData;

    private Action StartMining;
    private Action RetrieveAction;
    private MineMachine machine;

    private List<CTSlot> ctSlots = new();
    private List<RetrieveSlot> retrieveSlots = new();

    private float basicPb;
    private bool isOpenUI;
    
    private void Awake()
    {
        isOpenUI = false;
        
        startBtn.onClick.AddListener(() =>
        {
            StartMining.Invoke();
            Inventory.Instance.RemoveItem(oilData, machine.GetOilPrice());
            UpdateOilText();
            CloseUI();
        });

        retrieveBtn.onClick.AddListener(() =>
        {
            RetrieveAction.Invoke();
        });
        
        closeBtn.onClick.AddListener(CloseUI);
    }

    private void UpdateOilText()
    {
        int price = machine.GetOilPrice();
        int curOil = Inventory.Instance.GetItemTotalAmount(oilData);

        oilText.text = $"{curOil} / {price}";

        startBtn.interactable = curOil >= price;
    }


    private void Start()
    {
        tab.SetActive(false);
    }


    public void Open(MineMachine machine, Action OperateMachine, Action Retrieve)
    {
        //화면이 열리면 채굴 버튼은 비활성화
        startBtn.interactable = false;

        StartMining = OperateMachine;
        RetrieveAction = Retrieve;
        this.machine = machine;

        //집중모드, 결과창 슬롯들 전부 off 
        for(int i = 0; i<ctSlots.Count; i++)
            ctSlots[i].gameObject.SetActive(false);
        
        for(int i = 0; i<retrieveSlots.Count; i++)
            retrieveSlots[i].gameObject.SetActive(false);


        isOpenUI = true;
        UpdateUI(machine);
        
        tab.SetActive(true);
    }

    private void UpdateUI(MineMachine machine)
    {
        //집중채굴 업데이트
        UpdateConcentrationUI(machine);
        OnClickSlot(ctSlots[machine.CurConcentrationIndex], machine.CurConcentrationIndex);
        
        //결과창 업데이트
        UpdateRetrieveUI(machine);
    }

    //집중채굴
    private void UpdateConcentrationUI(MineMachine mineMachine)
    {
        List<MineMachinePbData> list = mineMachine.GetOreList;
        
        basicPb = 50/(list.Count - 1);
        
        int curSlotCnt = ctSlots.Count;
        for (int i = 0; i < curSlotCnt; i++)
        {
            ctSlots[i].SetSlot(list[i].oreData, basicPb);
            ctSlots[i].gameObject.SetActive(true);
        }

        for (int i = curSlotCnt; i < list.Count; i++)
        {
            CTSlot slot = Instantiate(ctSlot, ctParent).GetComponent<CTSlot>();
            slot.SetSlot(list[i].oreData, basicPb);
            ctSlots.Add(slot);

            Button btn = slot.GetComponent<Button>();
            int index = i;
            btn.onClick.AddListener(() => OnClickSlot(slot, index));
        }

        ctSlots[mineMachine.CurConcentrationIndex].HighLight();
    }
    
     //회수
     public void UpdateRetrieveUI(MineMachine mineMachine)
     {
         //UI가 열린상태에서도 채굴을 하게 되면 지속적으로 UI상태 업데이트를 위함.
         //그러나 UI가 안열려 있으면 채굴해도 굳이 Update X
         if (!isOpenUI)
             return;
         
         List<Item> list = mineMachine.GetMineResult;

         int curSlotCnt = retrieveSlots.Count;
         int itemCnt = list.Count;

        // 기존 슬롯 업데이트 or 끄기
         for (int i = 0; i < curSlotCnt; i++)
         {
             if (i < itemCnt)
             {
                 retrieveSlots[i].SetSlot(list[i]);
                 retrieveSlots[i].gameObject.SetActive(true);
             }
             else
             {
                 retrieveSlots[i].gameObject.SetActive(false); // 남는 슬롯은 끄기
             }
         }

         // 부족한 슬롯 생성
         for (int i = curSlotCnt; i < itemCnt; i++)
         {
             RetrieveSlot slot = Instantiate(retrieveSlot, retrieveParent).GetComponent<RetrieveSlot>();
             slot.SetSlot(list[i]);
             retrieveSlots.Add(slot);
         }
         
         curMineText.text = $"{machine.CurMineAmount} / 400";
         storeText.text = $"{machine.StoreAmount} / 2000";
         retrieveBtn.interactable = machine.StoreAmount > 0;

         isOpenUI = true;
     }

     private void OnClickSlot(CTSlot slot, int index)
     {
         ctSlots[machine.CurConcentrationIndex].OffHighLight(basicPb);
         slot.HighLight();
         machine.ChangeIndex(index);


         int price = machine.GetOilPrice();
         int curOil = Inventory.Instance.GetItemTotalAmount(oilData);

         oilText.text = $"{curOil} / {price}";

         startBtn.interactable = curOil >= price;
     }

     private void CloseUI()
     {
         isOpenUI = false;
         tab.SetActive(false); 
     }
}
