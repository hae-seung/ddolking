using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TalkManager : Singleton<TalkManager>
{
    private Dictionary<int, NPCData> npcInfo;
    [SerializeField] private TalkUI talkUI; 
    private DOTweenAnimation talkPanel;
    
    
    public bool isTalk { get; private set; } //튜토리얼용
    
    protected override void Awake()
    {
        base.Awake();
        talkPanel = talkUI.GetComponent<DOTweenAnimation>();
        talkPanel.gameObject.SetActive(false);
        LoadDictionary();
    }

    private void LoadDictionary()
    {
        npcInfo = new Dictionary<int, NPCData>();
        NPCData[] npcAll = Resources.LoadAll<NPCData>("NPCs");

        foreach (var npc in npcAll)
        {
            if(!npcInfo.ContainsKey(npc.npcId))
                npcInfo.Add(npc.npcId, npc);
        }
    }

    public void StartShortTalk(TalkData talkData)//튜토리얼용
    {
        if (!isTalk)
        {
            if(!talkPanel.gameObject.activeSelf)
            {
                talkPanel.gameObject.SetActive(true);
                talkPanel.DORestartById("show");
            }
            NPCData npc = npcInfo[talkData.npcId];
            talkUI.SetTalk(talkData, npc);
            isTalk = true;
            StartCoroutine(WaitForTalkEnd());
        }
        else
        {
            Debug.Log("대화가 이미 진행중");
        }
    }

    private IEnumerator WaitForTalkEnd()
    {
        yield return new WaitUntil(() => talkUI.isFinish);
        isTalk = false;
    }

    public void HidePanel()
    {
        isTalk = false;
        talkPanel.DORestartById("hide");
    }
}
