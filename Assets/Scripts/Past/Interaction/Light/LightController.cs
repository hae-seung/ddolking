using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    private Light2D light;
    private bool isFirstObject = true;

    private void Awake()
    {
        isFirstObject = true;
        light = GetComponent<Light2D>();
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.dayEvents.onTurnLight += TurnLight;
        if(!isFirstObject)
            CheckCurrentTime();
    }

    private void Start()
    {
        isFirstObject = false;
        CheckCurrentTime();
    }

    private void OnDisable()
    {
        if(GameEventsManager.Instance != null)
            GameEventsManager.Instance.dayEvents.onTurnLight -= TurnLight;
    }
    
    
    private void TurnLight(bool state)
    {
        light.enabled = state;
    }
    

    private void CheckCurrentTime()
    {
        int currentHour = GameEventsManager.Instance.dayEvents.GetCurrentTime();
        if (currentHour != -1)
        {
            if (currentHour >= 16 || currentHour <= 8)
                light.enabled = true;
            else
                light.enabled = false;
        }
        else
        {
            Debug.Log("dayManager 구독 실패");
        }
    }
}
