using System;
using UnityEngine;

public class PlayerEvents
{
    public event Action onDisablePlayerMovement;
    public void DisablePlayerMovement()
    {
        onDisablePlayerMovement?.Invoke();
    }

    public event Action onEnablePlayerMovement;
    public void EnablePlayerMovement()
    {
        onEnablePlayerMovement?.Invoke();
    }

    public event Action<string, bool> onPlayAnimation;
    public void PlayAnimation(string animationName, bool state)
    {
        onPlayAnimation?.Invoke(animationName, state);
    }

    public event Action<int, int> onAcquireItem;//아이템 획득 또는 이미 있는지
    public void AcquireItem(int amount, int itemId)
    {
        onAcquireItem?.Invoke(amount, itemId);
    }

    public event Action<int, int> onDisposeItem; //아이템이 인벤토리에서 사용되거나 나갈때
    public void DisposeItem(int amount, int itemId)
    {
        onDisposeItem?.Invoke(amount, itemId);
    }

    /// <summary>
    /// 외부에서 경험치 획득 -> 레벨 매니저에서 계산
    /// </summary>
    public event Action<int> onGainExperience;
    public void GainExperience(int amount)
    {
        onGainExperience?.Invoke(amount);
    }

    /// <summary>
    /// 레벨 매니저에서 경험치 변화 -> UI 및 레벨업 포인트 지급
    /// </summary>
    public event Action<int> onChangedExperience;
    public void ChangedExperience(int value)
    {
        onChangedExperience?.Invoke(value);
    }

    /// <summary>
    /// 레벨매니저에서 레벨 변화 -> UI에서 업데이트
    /// </summary>
    public event Action<int, int> onChangedLevel;
    public void ChangedLevel(int curLevel, int needExperienceToNextLevel)
    {
        onChangedLevel?.Invoke(curLevel, needExperienceToNextLevel);
    }

    public event Func<int> onGetLevel;
    public int GetLevel()
    {
        if (onGetLevel != null)
            return (int)onGetLevel?.Invoke();

        return -1;
    }

    public event Action onDead;
    public void Dead()
    {
        onDead?.Invoke();
    }


    public event Action onMineEnter;
    public void MineEnter()
    {
        onMineEnter?.Invoke();
    }

    public event Action onMineExit;
    public void ExitMine()
    {
        onMineExit?.Invoke();
    }

}
