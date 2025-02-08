using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; private set; }
    private Status status;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
        
        status = new Status();
        status.Init();
    }

    public void AddStat(List<StatModifier> statModifiers)
    {
        for (int i = 0; i < statModifiers.Count; i++)
        {
            Stat stat = statModifiers[i].stat;
            int increaseAmount = statModifiers[i].increaseAmount;
            
            status.ApplyStatChange(stat, increaseAmount);
        }
        GameEventsManager.Instance.playerEvents.UpdateStatusUI();
    }

    public void AddStat(Stat stat, int amount)
    {
        status.ApplyStatChange(stat, amount);
        GameEventsManager.Instance.playerEvents.UpdateStatusUI();
    }
    
    
}
