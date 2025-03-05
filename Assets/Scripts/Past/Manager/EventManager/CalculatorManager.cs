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
    }


    private float CalculateMineDamage(Stat targetStat)
    {
        return _statusManager.GetStatValue(targetStat);
    }
}
