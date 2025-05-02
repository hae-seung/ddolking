using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatorManager : MonoBehaviour
{
    [SerializeField] private StatusManager _statusManager;


    private void Awake()
    {
        GameEventsManager.Instance.calculatorEvents.onCalculateMineDamage += CalculateMineDamage;
        GameEventsManager.Instance.calculatorEvents.onCalculateDamage += CalculateDamage;
    }

    private float CalculateDamage(float damage)
    {
        float defense = _statusManager.GetStatValue(Stat.Defense);
        float realDamage = damage * (1 - defense);
        return realDamage;
    }


    private float CalculateMineDamage(Stat targetStat)
    {
        return _statusManager.GetStatValue(targetStat);
    }
}
