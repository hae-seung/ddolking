using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DungeonTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    private int currentTime;

    private Coroutine timerCoroutine;
    private WaitForSeconds waitTime = new WaitForSeconds(1f);

    private void Awake()
    {
        gameObject.SetActive(false);
    }


    public void StartTimer(float time)
    {
        gameObject.SetActive(true);
        
        if(timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        currentTime = (int)time * 60;
        
        timerCoroutine = StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        while (currentTime > 0)
        {
            int minutes = currentTime / 60;
            int second = currentTime % 60;
            timeText.text = $"{minutes:00}:{second:00}";

            yield return waitTime;
            --currentTime;
        }
        timeText.text = "00:00";
    }

    public float GetRemainTime()
    {
        return currentTime;
    }

    public void HideTimer()
    {
        gameObject.SetActive(false);
    }

    public void StopTimer()
    {
        if(timerCoroutine != null)
            StopCoroutine(timerCoroutine);
    }
    
}
