using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public abstract class WeaponBuffer
{
    public DebuffBase currentDebuff { get; private set; }
    
    
    private LivingEntity entity;
    public int debuffLevel { get; protected set; }
    private Coroutine _coroutine;

    
    
    public WeaponBuffer(DebuffBase data, int level)
    {
        currentDebuff = data;
        debuffLevel = level;
    }
    
    
    public bool CanApply()
    {
        int ran = Random.Range(1, 101); // 1 이상 100 이하
        return currentDebuff.debuffProbability >= ran;
    }
    
    
    
    public void RemoveDebuff()
    {
        if (_coroutine != null)
        {
            entity.StopCoroutine(_coroutine);
            entity.OnEndDebuff();
        }
    }

    public void ApplyEffect(LivingEntity target)
    {
        entity = target;
        _coroutine = entity.StartCoroutine(OnDebuffStart(entity, RemoveDebuff));
    }

    protected abstract IEnumerator OnDebuffStart(IDamageable entity, UnityAction onEnd);

    public abstract WeaponBuffer CreateBuffer(int level);

    public abstract string GetDebuffDescription();
}

