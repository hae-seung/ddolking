using System;
using System.Collections.Generic;
using UnityEngine;

public class StatData
{
    public int curStatLevel;
    public float increaseAmount; // float으로 변경
    public int maxLevel;

    public StatData(int level, float amount, int maxLevel)
    {
        curStatLevel = level;
        increaseAmount = amount;
        this.maxLevel = maxLevel;
    }
}

public class StatusManager : MonoBehaviour
{
    private Dictionary<Stat, float> status = new Dictionary<Stat, float>(); // object → float
    private Dictionary<Stat, StatData> statLevelData = new Dictionary<Stat, StatData>()
    {
        //레벨로 성장 가능한 옵션들
        //-1은 제한 맥스레벨 제한 없음
        { Stat.MaxHP, new StatData(0, 50f, -1) },
        { Stat.MaxEnergy, new StatData(0, 5f, 50) },
        { Stat.Str, new StatData(0, 5f, -1) },
        { Stat.Critical, new StatData(0, 1f, 50) },
        { Stat.Speed, new StatData(0, 0.5f, 10) },
        { Stat.MineSpeed, new StatData(0, 0.5f, -1) }
    };
    
    private void Awake()
    {
        GameEventsManager.Instance.statusEvents.onAddStat += AddStat;
        GameEventsManager.Instance.statusEvents.onStatLevelUpBtnClicked += StatLevelUpBtnClicked;
        GameEventsManager.Instance.statusEvents.onGetStatData += GetStatData;
        GameEventsManager.Instance.statusEvents.onGetStatValue += GetStatValue;
    }
    

    private void Start()
    {
        InitStatus();
        InvokeEvent();
    }

    private void InvokeEvent()
    {
        foreach (var stat in status)
        {
            GameEventsManager.Instance.statusEvents.StatChanged(stat.Key, stat.Value);
        }
    }

    private void InitStatus()
    {
        status[Stat.Defense] = 0f; //0~1 : 0.1단위
        status[Stat.MaxHP] = 100f;//1단위
        status[Stat.HP] = 100f;//1단위
        status[Stat.MaxEnergy] = 50f;//1단위
        status[Stat.Energy] = 10f;//1단위
        status[Stat.Str] = 5f;//1단위
        status[Stat.Critical] = 50f; //지금은 50퍼센트 0~100 : 1단위
        status[Stat.CriticalDamage] = 1.5f;//기본 1.5배 1 ~ 무한 : 0.5단위
        status[Stat.Speed] = 2f; //1~7 : 0.5단위
        status[Stat.MineSpeed] = 5f; //0.5단위
        status[Stat.ExperienceGetter] = 1f; //1배 ~ 무한 : 1단위 : 획득경험치 증가 버프 
    }
    
    private StatData GetStatData(Stat targetStat)
    {
        return statLevelData.TryGetValue(targetStat, out StatData statData) ? statData : null;
    }

    private StatData StatLevelUpBtnClicked(Stat targetStat)
    {
        if (statLevelData.TryGetValue(targetStat, out StatData statData))
        {
            statData.curStatLevel++;
            AddStat(targetStat, statData.increaseAmount);

            GameEventsManager.Instance.statusEvents.StatChanged(targetStat, status[targetStat]);
            return statData;
        }
        return null;
    }

    private void AddStat(Stat goalStat, float amount)
    {
        if (!status.TryGetValue(goalStat, out float currentStat))
        {
            Debug.Log("존재하지 않는 스탯입니다.");
            return;
        }

        // HP 및 Energy 처리 (최대치 초과 방지)
        if (goalStat == Stat.HP || goalStat == Stat.Energy)
        {
            float newVal = currentStat + amount;
            float maxVal = GetStatValue(goalStat == Stat.HP ? Stat.MaxHP : Stat.MaxEnergy);

            // HP가 0 이하이면 사망 처리 가능
            if (goalStat == Stat.HP && newVal <= 0)
            {
                // 플레이어 사망 처리 로직 추가
                Dead();
            }
            else if (goalStat == Stat.Energy && newVal <= 0)
            {
                //마나가 0이 되면 모든 능력치 감소
            }
            
            
            status[goalStat] = Mathf.Min(newVal, maxVal);
        }
        else if (goalStat == Stat.MaxHP || goalStat == Stat.MaxEnergy)
        {
            //최소 1f보다는 높게
            float newMax = Mathf.Max(currentStat + amount, 1f);
            status[goalStat] = newMax;
            AdjustCurrentStatIfExceedsMax(goalStat, newMax);
        }
        else if(goalStat == Stat.Speed)
        {
            //이동속도는 1~7 사이로만 고정
            status[goalStat] = Math.Clamp(currentStat + amount, 1f, 7f);
        }
        else if(goalStat == Stat.Defense)
        {
            //방어율은 0~1 사이
            status[goalStat] = Mathf.Clamp(currentStat + amount, 0, 1f);
        }
        else
        {
            //나머지는 최소 1
            status[goalStat] = Mathf.Max(currentStat + amount, 1f);
        }

        GameEventsManager.Instance.statusEvents.StatChanged(goalStat, status[goalStat]);
    }

    /// <summary>
    /// 지정한 스탯의 현재 값을 반환합니다. (없으면 0)
    /// </summary>
    private float GetStatValue(Stat stat)
    {
        return status.TryGetValue(stat, out float value) ? value : 0f;
    }

    /// <summary>
    /// 최대치가 변경되었을 때, 해당하는 현재 HP 또는 Energy가 새 최대치를 초과하면 조정합니다.
    /// </summary>
    private void AdjustCurrentStatIfExceedsMax(Stat changedMaxStat, float newMaxValue)
    {
        Stat currentStat = (changedMaxStat == Stat.MaxHP) ? Stat.HP : Stat.Energy;
        if (status.TryGetValue(currentStat, out float currVal) && currVal > newMaxValue)
        {
            status[currentStat] = newMaxValue;
            GameEventsManager.Instance.statusEvents.StatChanged(currentStat, newMaxValue);
        }
    }
    
    
    private void Dead()
    {
        GameEventsManager.Instance.playerEvents.Dead();
    }
}
