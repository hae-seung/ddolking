using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonUI : MonoBehaviour
{
    [Header("구성요소")]
    [SerializeField] private GameObject info;
    [SerializeField] private TextMeshProUGUI dungeonName;
    [SerializeField] private TextMeshProUGUI dungeonExplain;
    [SerializeField] private TextMeshProUGUI remainTime;
    [SerializeField] private GameObject ticketSet;
    [SerializeField] private ItemData ticketItem;

    [Header("소탕")]
    [SerializeField] private GameObject sweepSet; //Info의 소탕정보
    [SerializeField] private TextMeshProUGUI sweepLimitTime;
    [SerializeField] private GameObject sweepItems;
    [SerializeField] private Transform sweepContent;
    
    [Header("버튼")]
    [SerializeField] private Button enterBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button sweepBtn;

    [Header("소탕보상")] 
    [SerializeField] private GameObject rewardResultUI;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject slotPrefab;
    
    
    
    
    //소탕보상
    private List<SweepSlot> slots;//슬롯저장용
    private List<SweepReward> rewards; //소탕 보상목록
    
    //소탕결과 창
    private List<SweepSlot> resultSlots = new List<SweepSlot>();
    
    [SerializeField]
    private DOTweenAnimation _doTweenAnimation;
    
    private Action EnterDungeon;
    private Action ExitDungeon;

    private bool CanUseTicket = false;
    private bool CanEnter = false;
    
    
    private void Awake()
    {
        rewardResultUI.SetActive(false);
        info.SetActive(false);
        slots = new List<SweepSlot>();
        
        enterBtn.onClick.AddListener(() =>
        {
            CloseDungeonUI();
            EnterDungeon?.Invoke();
            UseTicket();
            exitBtn.gameObject.SetActive(true);
        });

        closeBtn.onClick.AddListener(CloseDungeonUI);
        
        exitBtn.onClick.AddListener(() =>
        {
            GameEventsManager.Instance.playerEvents.ExitMine();
            UIManager.Instance.StartTransition();
            ExitDungeon?.Invoke();
            exitBtn.gameObject.SetActive(false);
        });
        
        sweepBtn.onClick.AddListener(() =>
        {
            //이미 소탕이 가능하니 눌러짐.
            //소탕 보상 지급
            CloseDungeonUI();
            UseTicket();
            GiveSweepReward();
            ExitDungeon.Invoke(); //던전을 클리어한 셈 치기 위함.
        });
        
        exitBtn.gameObject.SetActive(false);
        sweepItems.SetActive(false);
    }


    public void OpenDungeonUI(string name, string explain, int remainTime, int sweepLimitTime,
        bool isSweepable, bool hasFirstClear,
        Action Enter, Action Exit)
    {
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
        GameEventsManager.Instance.inputEvents.DisableInput();

        EnterDungeon = Enter;
        ExitDungeon = Exit;

        dungeonName.text = name;
        dungeonExplain.text = explain;

        CanUseTicket = false;
        CanEnter = false;
        
        //입장시간까지 남음
        SetRemainTime(remainTime);
        
        sweepSet.SetActive(hasFirstClear && !isSweepable); //최초클리어&&소탕미션 미달성
            
        sweepBtn.gameObject.SetActive(hasFirstClear);//최초클리어시 모습 드러냄
        sweepBtn.interactable = isSweepable && CanEnter; //입장가능시간 또는 티켓을 보유하며 동시에 소탕가능시에만 활성화
        
        
        this.sweepLimitTime.text = $"{sweepLimitTime}분";
        
        exitBtn.gameObject.SetActive(false);//나가기 버튼은 광산 입장했을때 활성화
        
        info.SetActive(true);
        _doTweenAnimation.DORestartById("show");
    }

    public void OpenSweepRewards(bool CanSweep, List<SweepReward> rewards)
    {
        if(!CanSweep)
        {
            sweepItems.SetActive(false);
            return;
        }
        
        //목록 채우기
        this.rewards = rewards; //실제 리워드목록 저장
        
        for(int i = 0; i<slots.Count; i++)
            slots[i].gameObject.SetActive(false);
        
        int currentSlotIndex = 0;

        for (int i = 0; i < rewards.Count; i++)
        {
            if (currentSlotIndex >= slots.Count)
            {
                // 슬롯 부족하면 생성
                SweepSlot slot = Instantiate(slotPrefab, sweepContent).GetComponent<SweepSlot>();
                slots.Add(slot);
            }

            slots[currentSlotIndex].SetData(rewards[i].item, rewards[i].amount);
            slots[currentSlotIndex].gameObject.SetActive(true);
            currentSlotIndex++;
        }
        
        sweepItems.SetActive(true);
    }
    
    
    private void SetRemainTime(int remainTime)
    {
        if (remainTime > 0)
        {
            //티켓이 있다면 입장가능
            if (Inventory.Instance.GetItemTotalAmount(ticketItem) > 0)
            {
                CanUseTicket = true;
                ticketSet.SetActive(true);
                this.remainTime.text = "입장가능";
                enterBtn.gameObject.SetActive(true);
                CanEnter = true;
            }
            else
            {
                ticketSet.SetActive(true);
                this.remainTime.text = $"{remainTime}시간";
                enterBtn.gameObject.SetActive(false);
                CanEnter = false;
            }
        }
        else
        {
            this.remainTime.text = "입장가능";
            ticketSet.SetActive(false);
            enterBtn.gameObject.SetActive(true);
            CanEnter = true;
        }

    }
    
    private void CloseDungeonUI()
    {
        GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
        GameEventsManager.Instance.inputEvents.EnableInput();
        
        _doTweenAnimation.DORestartById("hide");
        
        sweepItems.SetActive(false);
        exitBtn.gameObject.SetActive(false);
    }


    private void UseTicket()
    {
        if (CanUseTicket)
        {
            Inventory.Instance.RemoveItem(ticketItem, 1);
        }
    }

    private void GiveSweepReward()
    {
        //보상지급
        //지급된 보상이 무엇인지 UI 띄워서 확인시켜주기

        List<int> rewardAmount = new List<int>();
        for (int i = 0; i < rewards.Count; i++)
        {
            Item item = rewards[i].item.CreateItem();
            int remainAmount = rewards[i].amount - Inventory.Instance.Add(item, rewards[i].amount);
            rewardAmount.Add(remainAmount);
        }
        
        
        rewardResultUI.SetActive(true);
        
        
        for (int i = 0; i < resultSlots.Count; i++)
        {
            resultSlots[i].gameObject.SetActive(false);
        }
        
        int currentSlotIndex = 0;

        for (int i = 0; i < rewards.Count; i++)
        {
            if (currentSlotIndex >= resultSlots.Count)
            {
                // 슬롯 부족하면 생성
                SweepSlot slot = Instantiate(slotPrefab, content).GetComponent<SweepSlot>();
                resultSlots.Add(slot);
            }

            resultSlots[currentSlotIndex].SetData(rewards[i].item, rewardAmount[i]);
            resultSlots[currentSlotIndex].gameObject.SetActive(true);
            currentSlotIndex++;
        }
    }
    
}
