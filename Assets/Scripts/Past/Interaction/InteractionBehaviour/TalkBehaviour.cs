using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
public class TalkBehaviour : InteractionBehaviour
{
    //일반적인 조건없이 그냥 말하는 npc
    
    [SerializeField] private List<TalkSessionData> talkSessionDatas;

    private int currentIndex = 0;
    protected override void Interact(Interactor interactor)
    {
        currentIndex %= talkSessionDatas.Count;
        
        List<TalkData> datas = talkSessionDatas[currentIndex].talkDatas;
        TalkManager.Instance.StartTalk(datas);
        currentIndex++;
    }
}
