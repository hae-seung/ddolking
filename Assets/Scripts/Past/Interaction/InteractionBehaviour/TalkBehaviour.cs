using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
public class TalkBehaviour : InteractionBehaviour
{
    //일반적인 조건없이 그냥 말하는 npc
    
    [SerializeField] private List<TalkSessionData> talkSessionDatas;

    private SpriteRenderer sr;
    
    private int currentIndex = 0;
    public bool facingRight;

    private void Awake()
    {
        currentIndex = 0;
        sr = GetComponent<SpriteRenderer>();
    }
    
    
    
    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        //talk세션이 여러개면 순서대로 무한 굴레
        currentIndex %= talkSessionDatas.Count;
        
        List<TalkData> datas = talkSessionDatas[currentIndex].talkDatas;

        if (sr)
        {
            if (interactor.transform.position.x - transform.position.x >= 0)
                facingRight = true;
            else
                facingRight = false;
            
            sr.flipX = facingRight;
        }
        
        TalkManager.Instance.StartTalk(datas);
        currentIndex++;
    }
}
