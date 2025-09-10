using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrowable
{
    public float GetMaxGrowTime();

    public bool GetTypeCorrect(DamageType type);
}
