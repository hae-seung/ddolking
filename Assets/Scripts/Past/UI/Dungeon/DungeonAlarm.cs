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

    private Coroutine alarmRoutine;

    public void AlarmBoss(string message)
    {
        if (message == "")
            return;

        if (alarmRoutine != null)
            return;
        
        bossMessage.text = message;
        bossAlarm.SetActive(true);
        alarmRoutine = StartCoroutine(WaitBossMessage());
    }

    private IEnumerator WaitBossMessage()
    {
        yield return new WaitForSeconds(3f);
        bossAlarm.SetActive(false);
        alarmRoutine = null;
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

        alarmRoutine = null;
    }


    #region 봄마을 전용 웨이브 알람

    public void WaveAlarm(Action Finish)
    {
        waveAlarm.SetActive(true);
        StartCoroutine(WaitThirdTimeAlarm(Finish));
    }
    
    private IEnumerator WaitThirdTimeAlarm(Action Finish)
    {
        yield return new WaitForSeconds(4.5f);
        waveAlarm.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        Finish?.Invoke();
    }

    #endregion
}
