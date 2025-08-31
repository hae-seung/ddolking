using System;
using System.Collections.Generic;
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

    
    public event Action onPlayerNoHurt;
    /// <summary>
    /// 플레이어 무적 활성화
    /// </summary>
    public void PlayerNoHurt()
    {
        onPlayerNoHurt?.Invoke();
    }
    
    public event Action onPlayerYesHurt;
    /// <summary>
    /// 플레이어 무적 종료
    /// </summary>
    public void PlayerYesHurt()
    {
        onPlayerYesHurt?.Invoke();
    }
    
    
    
    
    public event Action<string, bool> onPlayAnimation;

    public void PlayAnimation(string animationName, bool state)
    {
        onPlayAnimation?.Invoke(animationName, state);
    }

    public event Action<int> onAcquireItem; //아이템 획득 또는 이미 있는지
    public void AcquireItem(int index)
    {
        onAcquireItem?.Invoke(index);
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


    public event Action<Color> onMineEnter;

    public void MineEnter(Color color)
    {
        onMineEnter?.Invoke(color);
    }

    public event Action onMineExit;

    public void ExitMine()
    {
        onMineExit?.Invoke();
    }


    /// <summary>
    /// 버프
    /// </summary>
    public event Func<BuffItem, bool> onApplyPortionBuff;
    public bool ApplyPortionBuff(BuffItem buffItem)
    {
        if (onApplyPortionBuff != null)
            return onApplyPortionBuff.Invoke(buffItem);

        return false;
    }


    public event Func<List<StatModifier> ,bool> onApplySchoolBuff;
    public bool ApplySchoolBuff(List<StatModifier> schoolBuffs)
    {
        if (onApplySchoolBuff != null)
            return onApplySchoolBuff.Invoke(schoolBuffs);

        return false;
    }


    public event Func<Item> onGetHandItem;
    public Item GetHandItem()
    {
        if (onGetHandItem != null)
            return onGetHandItem.Invoke();
        return null;
    }

}
