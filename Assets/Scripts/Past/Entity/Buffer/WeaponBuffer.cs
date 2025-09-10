using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponBuffer
{
    public DebuffBase currentDebuff { get; private set; }
    public int debuffLevel { get; protected set; }

    private IDamageable damageableTarget;
    private IDebuffable debuffableTarget;
    private MonoBehaviour runner;
    private Coroutine _coroutine;

    public WeaponBuffer(DebuffBase data, int level)
    {
        currentDebuff = data;
        debuffLevel = level;
    }

    public void LevelUp()
    {
        debuffLevel++;
    }
    
    public bool CanApply()
    {
        int ran = Random.Range(1, 101);
        return currentDebuff.debuffProbability >= ran;
    }

    public void RemoveDebuff()
    {
        if (_coroutine != null && runner != null)
        {
            runner.StopCoroutine(_coroutine);
            debuffableTarget?.OnEndDebuff();
        }
    }

    public void ApplyEffect(IDamageable target, IDebuffable debuffable, MonoBehaviour coroutineRunner)
    {
        this.damageableTarget = target;
        this.debuffableTarget = debuffable;
        this.runner = coroutineRunner;

        _coroutine = runner.StartCoroutine(OnDebuffStart(damageableTarget, RemoveDebuff));
    }

    protected abstract IEnumerator OnDebuffStart(IDamageable entity, UnityAction onEnd);
    public abstract WeaponBuffer CreateBuffer(int level);
    public abstract string GetDebuffDescription();
    public abstract string GetNextDebuffDescription();
}