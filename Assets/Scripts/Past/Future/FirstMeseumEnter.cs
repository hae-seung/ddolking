using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMeseumEnter : MonoBehaviour
{
    [SerializeField] private TalkSessionData talkSessionData;
    
    private bool hasFirstTalk = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasFirstTalk)
            return;
        
        if (other.CompareTag("Player"))
        {
            List<TalkData> data = talkSessionData.talkDatas;
            TalkManager.Instance.StartTalk(data);
            hasFirstTalk = true;
        }
    }
}
