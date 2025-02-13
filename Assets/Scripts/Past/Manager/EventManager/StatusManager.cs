using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class StatData
{
    public int curStatLevel;
    public object increaseAmount;
    public int maxLevel;

    public StatData(int level, object amount, int maxLevel)
    {
        curStatLevel = level;
        increaseAmount = amount;
        this.maxLevel = maxLevel;
    }
}


public class StatusManager : MonoBehaviour
{
    private Dictionary<Stat, object> status = new Dictionary<Stat, object>();
    private Dictionary<Stat, StatData> statLevelData = new Dictionary<Stat, StatData>()
    {
        { Stat.MaxHP, new StatData(0, 50, -1) }, // hpMax : X
        { Stat.MaxEnergy, new StatData(0, 20, 50) }, //powerMax : 1000
        { Stat.Str, new StatData(0, 5, -1) },//계산 공식 존재,  strMax : X
        { Stat.Luk, new StatData(0, 1, 100) },//% , 계산공식 존재 , Lukmax : 100
        { Stat.Speed, new StatData(0, 0.5f, 20) },// speedMax : 7f;
        { Stat.MineSpeed, new StatData(0, 2f, 50) }// 나누기 10 => 초단위로 생각, , mineSpeedMax : 100f; == 10초 단축
    };
    
    private void Awake()
    {
        GameEventsManager.Instance.statusEvents.onAddStat += AddStat;
        GameEventsManager.Instance.statusEvents.onStatLevelUpBtnClicked += StatLevelUpBtnClicked;
        GameEventsManager.Instance.statusEvents.onGetStatData += GetStatData;
    }
    
    private void Start()
    {
        InitStatus();
        InvokeEvent();//다른 스크립트가 awake에서 구독을 하고 실행시켜줘야함.
    }

    private void InvokeEvent()
    {
        foreach (var stat in status)
        {
            GameEventsManager.Instance.statusEvents.StatChanged(stat.Key, stat.Value);//초기화된 값으로 UI 초기화
        }
    }

    private void InitStatus()
    {
        status[Stat.MaxHP] = 100;
        status[Stat.HP] = status[Stat.MaxHP];
        status[Stat.MaxEnergy] = 100;
        status[Stat.Energy] = status[Stat.MaxEnergy];
        status[Stat.Str] = 5;
        status[Stat.Luk] = 5;
        status[Stat.Speed] = 2f;
        status[Stat.MineSpeed] = 5f;
        
    }
    

    private StatData GetStatData(Stat targetStat)
    {
        if (statLevelData.TryGetValue(targetStat, out StatData statData))
        {
            return statData;
        }
        return null;
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
    

    private void OnDisable()
    {
        //todo:구독해지
    }

    private void AddStat(Stat goalStat, object amount)
    {
        if (status.TryGetValue(goalStat, out object currentStat))
        {
            if (currentStat is int currentInt && amount is int amountInt)
                status[goalStat] = currentInt + amountInt;
            else if (currentStat is float currentFloat && amount is float amountFloat)
                status[goalStat] = currentFloat + amountFloat;
            else
            {
                Debug.LogWarning("스텟 계산 중 int float 형이 일치 하지 않습니다");
                return;
            }
            
            GameEventsManager.Instance.statusEvents.StatChanged(goalStat, status[goalStat]);
        }
        else
        {
            Debug.Log("그딴 스텟은 존재하지 않습니다");
        }
    }
}
