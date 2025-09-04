
using System;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    private float needExperienceToNextLevel = 5;
    private float curExperience = 0;
    private int level = 1;


    
    
    private void Start()
    {
        InvokeEvents();
    }

    private void InvokeEvents()
    {
        GameEventsManager.Instance.playerEvents.ChangedExperience(curExperience);
        GameEventsManager.Instance.playerEvents.ChangedLevel(level, needExperienceToNextLevel);
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.playerEvents.onGainExperience += GainExperience;
        GameEventsManager.Instance.playerEvents.onGetLevel += GetLevel;
    }

    private int GetLevel()
    {
        return level;
    }

    private void GainExperience(float experience)
    {
        float experienceGetter = GameEventsManager.Instance.statusEvents.GetStatValue(Stat.ExperienceGetter);
        //경험치 증가 버프 생각

        experience *= experienceGetter;
        
        curExperience += experience;
        // check if we're ready to level up
        while (curExperience >= needExperienceToNextLevel)
        {
            curExperience -= needExperienceToNextLevel;
            level++;
            needExperienceToNextLevel *= 1.25f; //다음 목표치 경험치는 1.25배
            GameEventsManager.Instance.playerEvents.ChangedLevel(level, needExperienceToNextLevel);
        }
        GameEventsManager.Instance.playerEvents.ChangedExperience(curExperience);
    }
}
