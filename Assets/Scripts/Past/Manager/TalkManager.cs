using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class TalkManager : Singleton<TalkManager>
{
    private Dictionary<int, NPCData> npcInfo;
    [SerializeField] private TalkUI talkUI;
    private DOTweenAnimation talkPanel;
    
    
    public bool isTalk { get; private set; } 
    
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

    private void HideOtherUI()
    {
        
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

    
    /// <summary>
    /// RealTalk
    /// </summary>
    
    public void HidePanel()
    {
        isTalk = false;
        talkPanel.DORestartById("hide");
        VirtualCameraManager.Instance.GetCamera(CameraType.talk).SetActive(false);
        
        GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
    }

    public void StartTalk(List<TalkData> talks)
    {
        if (isTalk)
            return;
        
        if(!talkPanel.gameObject.activeSelf)
            talkPanel.gameObject.SetActive(true);
        
        talkPanel.DORestartById("show");
        VirtualCameraManager.Instance.GetCamera(CameraType.talk).SetActive(true);
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
        
        
        StartCoroutine(AutoTalk(talks));
    }

    private IEnumerator AutoTalk(List<TalkData> talks)
    {
        isTalk = true;
        NPCData npc;
        for (int i = 0; i < talks.Count; i++)
        {
            npc = npcInfo[talks[i].npcId];
            talkUI.SetTalk(talks[i], npc);
            yield return new WaitUntil(() => talkUI.isFinish);
        }
        HidePanel();
    }
}
