using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatorEvents
{
    public event Func<Stat, float> onCalculateMineDamage;
    public float CalculateMineDamage(Stat stat)
    {
        float mineDamage = onCalculateMineDamage.Invoke(stat);
        return mineDamage;
    }

    public event Func<float, float> onCalculateDamage;

    public float CalculateDamage(float damage)
    {
        float takeDamage = onCalculateDamage(damage);
        return takeDamage;
    }

}
