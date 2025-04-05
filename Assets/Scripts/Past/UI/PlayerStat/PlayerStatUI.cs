
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStatUI : MonoBehaviour
{
    [Header("스텟")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthTxt;
    [Space(10)]
    [SerializeField] private Slider powerSlider;
    [SerializeField] private TextMeshProUGUI powerTxt;
    
    [Header("레벨")]
    [SerializeField] private Slider experienceSlider;
    [SerializeField] private TextMeshProUGUI levelTxt;


    private void Awake()
    {
        GameEventsManager.Instance.statusEvents.onStatChanged += StatChanged;
        GameEventsManager.Instance.playerEvents.onChangedExperience += ChangedExperience;
        GameEventsManager.Instance.playerEvents.onChangedLevel += ChangedLevel;
    }
    

    private void ChangedLevel(int curLevel, int nextExperienceToNextLevel)
    {
        levelTxt.text = $"레벨 : {curLevel}";
        experienceSlider.maxValue = nextExperienceToNextLevel;
    }

    private void ChangedExperience(int value)
    {
        experienceSlider.value = value;
    }

    private void StatChanged(Stat goalStat, float changedValue)
    {
        switch (goalStat)
        {
            case Stat.MaxHP :
                healthSlider.maxValue = (int)changedValue;
                healthTxt.text = $"{healthSlider.value}/{healthSlider.maxValue}";
                break;
            case Stat.HP:
                healthSlider.value = (int)changedValue;
                healthTxt.text = $"{healthSlider.value}/{healthSlider.maxValue}";
                break;
            case Stat.MaxEnergy :
                powerSlider.maxValue = (int)changedValue;
                powerTxt.text = $"{powerSlider.value}/{powerSlider.maxValue}";
                break;
            case Stat.Energy:
                powerSlider.value = (int)changedValue;
                powerTxt.text = $"{powerSlider.value}/{powerSlider.maxValue}";
                break;
            default:
                return;
        }
    }
}
