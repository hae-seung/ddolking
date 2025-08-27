using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAndBuytStatueBehaviour : LookStatueBehaviour
{
    [SerializeField] private StatueData sellData;
    [SerializeField] private SpriteRenderer sr;
    public bool isSell;
    
    
    
    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        if(!isSell)//팔리기전
            UIManager.Instance.OpenStatueExplain(statueData, BuyStatue);
        else//팔린 후
            UIManager.Instance.OpenStatueExplain(sellData);
        
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
    }

    
    
    
    private void BuyStatue()
    {
        isSell = true;
        sr.sprite = sellData.StatueImage;
    }
    
    
}
