using System;
using UnityEngine;
using DamageNumbersPro;


[System.Serializable]
public class TypeDamage : SerializableDictionary<DamageType, DamageNumber>{}

[System.Serializable]
public class TypeEffect : SerializableDictionary<DamageType, GameObject>{}

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance;
    

    [SerializeField] private TypeDamage typeDamage = new TypeDamage();
    [SerializeField] private TypeEffect typeEffect = new TypeEffect();
    
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
        //데미지 스킨이 아닌 히트 이펙트 프리팹
        //스킨은 자체 에셋에서 풀링이 됨.
        foreach (DamageType type in Enum.GetValues(typeof(DamageType)))
        {
            if (typeEffect.TryGetValue(type, out GameObject effect))
            {
                ObjectPoolManager.Instance.RegisterPrefab((int)type, effect);
            }
            else
            {
                Debug.LogWarning($"[DamageManager] typeEffect에 '{type}' 이(가) 없습니다.");
            }
        }
    }

    public DamageNumber GetDamageSkin(DamageType type)
    {
        return typeDamage[type];
    }

}
