using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialNpcBehaviour : InteractionBehaviour
{
    private bool isFirstTalk;
    private bool isQuesting;
    private bool hasClearQuest;

    
    [SerializeField] private ItemData questItem;
    [SerializeField] private ItemData rewardItem;


    [Header("경고 대화")]
    [SerializeField] private TalkSessionData warnTalk;
    
    [Header("처음 대화")] //isFirstTalk = false
    [SerializeField] private TalkSessionData firstTalk;
    [Header("퀘스트 진행중이며 완료불가능 대화")] //isQuesting = true && Player doesnt have QuestItem;
    [SerializeField] private TalkSessionData questingTalk;
    [Header("퀘스트 진행중이며 완료가능 대화")] //isQuesting = true && Player Has QuestItem;
    [SerializeField] private TalkSessionData clearableTalk;
    [Header("퀘스트 클리어 후 대화")] //hasClearQuest = true;
    [SerializeField] private TalkSessionData clearNextTalk;


    private void Awake()
    {
        isFirstTalk = false;
        isQuesting = false;
        hasClearQuest = false;
    }

    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        List<TalkData> datas;

        if (!isFirstTalk)
        {
            datas = firstTalk.talkDatas;
            isFirstTalk = true;
            isQuesting = true;
        }
        else if (isQuesting && Inventory.Instance.GetItemTotalAmount(questItem) <= 0)
        {
            datas = questingTalk.talkDatas;
        }
        else if (isQuesting && Inventory.Instance.GetItemTotalAmount(questItem) > 0)
        {
            if (Inventory.Instance.GetEmptySlotAmount() <= 0)
            {
                datas = warnTalk.talkDatas;
                TalkManager.Instance.StartTalk(datas);
                return;
            }
            
            datas = clearableTalk.talkDatas;
            isQuesting = false;
            hasClearQuest = true;
            Inventory.Instance.Add(rewardItem.CreateItem(), 1);
        }
        else if (hasClearQuest)
        {
            datas = clearNextTalk.talkDatas;
        }
        else
        {
            datas = null;
        }
        
        if(datas != null)
            TalkManager.Instance.StartTalk(datas);
    }
}
