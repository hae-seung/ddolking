using System;
using UnityEngine;
using DamageNumbersPro;


[System.Serializable]
public class TypeDamage : SerializableDictionary<DamageType, DamageNumber>{}

[System.Serializable]
public class TypeEffect : SerializableDictionary<DamageType, GameObject>{}

[System.Serializable]
public class DebuffSprite : SerializableDictionary<DebuffType, Sprite>{}

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance;
    
    [SerializeField] private TypeDamage typeDamage = new TypeDamage();
    [SerializeField] private TypeEffect typeEffect = new TypeEffect();
    [SerializeField] private DebuffSprite debuffSprite = new DebuffSprite();
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (var kvp in typeEffect)
        {
            DamageType type = kvp.Key;
            GameObject effect = kvp.Value;

            if (effect != null)
            {
                ObjectPoolManager.Instance.RegisterPrefab((int)type, effect);
                Debug.Log($"[DamageManager] Registered Effect: {type}");
            }
            else
            {
                Debug.LogWarning($"[DamageManager] Effect for {type} is null. Skipped.");
            }
        }
    }


    public DamageNumber GetDamageSkin(DamageType type)
    {
        return typeDamage[type];
    }

    public Sprite GetDebuffImage(DebuffType type)
    {
        return debuffSprite[type];
    }
    
    
}
