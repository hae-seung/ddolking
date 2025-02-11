using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AmuletEffect : ScriptableObject
{
    public abstract void ApplyEffect();
    public abstract void RemoveEffect();
}
