using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonAlarm : MonoBehaviour
{
    [SerializeField] private GameObject bossAlarm;
    [SerializeField] private TextMeshProUGUI bossMessage;
    
    
    [SerializeField] private GameObject waveAlarm;
    [SerializeField] private GameObject goNextStageAlarm;


    public void AlarmBoss(string message)
    {
        if (message == "")
            return;
        
        bossMessage.text = message;
        bossAlarm.SetActive(true);
        StartCoroutine(WaitBossMessage());
    }

    private IEnumerator WaitBossMessage()
    {
        yield return new WaitForSeconds(3f);
        bossAlarm.SetActive(false);
    }

    public void WaveAlarm(Action Finish)
    {
        waveAlarm.SetActive(true);
        StartCoroutine(WaitThirdTimeAlarm(Finish));
    }

    public void FinishWave()
    {
        goNextStageAlarm.SetActive(true);
    }

    public void Off()
    {
        bossAlarm.SetActive(false);
        waveAlarm.SetActive(false);
        goNextStageAlarm.SetActive(false);
    }
    
    private IEnumerator WaitThirdTimeAlarm(Action Finish)
    {
        yield return new WaitForSeconds(4.5f);
        waveAlarm.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        Finish?.Invoke();
    }
}
