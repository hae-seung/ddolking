
using System;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    private int needExperienceToNextLevel = 5;
    private int curExperience = 0;
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

    private void GainExperience(int experience)
    {
        curExperience += experience;
        // check if we're ready to level up
        while (curExperience >= needExperienceToNextLevel)
        {
            curExperience -= needExperienceToNextLevel;
            level++;
            needExperienceToNextLevel *= 2; //다음 목표치 경험치는 2배로 임시 설정 todo: 구글시트로 레벨 매니저 시트 만들기
            Debug.Log("이벤트호출");
            GameEventsManager.Instance.playerEvents.ChangedLevel(level, needExperienceToNextLevel);
        }
        GameEventsManager.Instance.playerEvents.ChangedExperience(curExperience);
    }
}
