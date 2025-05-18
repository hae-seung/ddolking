using System;
using System.Collections.Generic;


public class StatusEvents
{
    /// <summary>
    /// 외부에서 스탯 변화를 일으키고 싶을때 호출 -> 스탯 매니저에서 값 갱신
    /// </summary>
    public event Action<Stat, float> onAddStat;
    public void AddStat(Stat stat, float value)
    {
        onAddStat?.Invoke(stat, value);
    }

    /// <summary>
    /// 스텟 매니저에서 스탯 변화가 일어난 경우 호출 -> UI같은 애들이 한 번에 갱신
    /// </summary>
    public event Action<Stat, float> onStatChanged;
    public void StatChanged(Stat goalStat, float value)
    {
        onStatChanged?.Invoke(goalStat, value);
    }

    /// <summary>
    /// 레벨업을 통한 포인트로 스탯 찍음 -> 스탯 매니저에서 스탯 상승
    /// </summary>
    public event Func<Stat, StatData> onStatLevelUpBtnClicked;
    public StatData StatLevelUpBtnClicked(Stat targetStat)
    {
        return onStatLevelUpBtnClicked?.Invoke(targetStat);
    }
    
    /// <summary>
    /// StatusUI에서의 초기화를 위한 호출 -> 스탯 매니저에서 값 리턴
    /// </summary>
    public event Func<Stat, StatData> onGetStatData;
    public StatData GetStatData(Stat targetStat)
    {
        return onGetStatData?.Invoke(targetStat);
    }

    public event Func<Stat, float> onGetStatValue;

    public float GetStatValue(Stat targetStat)
    {
        return (float)onGetStatValue?.Invoke(targetStat);
    }

}
