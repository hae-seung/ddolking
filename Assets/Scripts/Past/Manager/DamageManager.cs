using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;


public enum DamageType
{
    normal,
    critical
}


[System.Serializable]
public class TypeDamage : SerializableDictionary<DamageType, DamageNumber>{}

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private TypeDamage typeDamage = new TypeDamage();

    public DamageNumber GetDamageSkin(DamageType type)
    {
        return typeDamage[type];
    }

}
