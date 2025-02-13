using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [Header("아뮬렛 슬롯")]
    [SerializeField] private AmuletSlot amuletSlot;

    [Header("스텟 수치 텍스트")] 
    [SerializeField] private TextMeshProUGUI strTxt;
    [SerializeField] private TextMeshProUGUI lukTxt;
    [SerializeField] private TextMeshProUGUI maxHPTxt;
    [SerializeField] private TextMeshProUGUI maxPowerTxt;
    [SerializeField] private TextMeshProUGUI speedTxt;
    [SerializeField] private TextMeshProUGUI mineSpeedTxt;

    [Header("다음강화 수치 텍스트")] 
    [SerializeField] private List<TextMeshProUGUI> _upgradeAmountLists;
    
    [Header("버튼")] 
    [SerializeField] private Button[] btns;
    [SerializeField] private Stat[] btnsStat;

    [Header("포인트")] 
    [SerializeField] private TextMeshProUGUI levelPointTxt;
    
    private int levelPoint = 0;

    
    
    private void Awake()
    {
        if (levelPoint >= 1) 
            Show();
        else 
            Hide();
        
        Init();
    }

    private void Start()
    {
        for(int i = 0; i<btnsStat.Length; i++)
        {
            ChangeUpgradeAmountText(btnsStat[i], i);
        }
    }

    private void Init()
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i;  // 버튼 인덱스 저장
            btns[i].onClick.AddListener(() => onLevelUpBtnClick(btnsStat[index], index));  // 버튼 클릭 시 인덱스와 스탯 전달
        }
        
        GameEventsManager.Instance.playerEvents.onChangedLevel += ChangedLevel;
        GameEventsManager.Instance.statusEvents.onStatChanged += StatChanged;
    }
    

    private void Hide()
    {
        for (int i = 0; i < btns.Length; i++) //버튼의 갯수와 텍스트의 갯수는 동일할거임
        {
            btns[i].gameObject.SetActive(false);
            _upgradeAmountLists[i].gameObject.SetActive(false);
        }

    }
    
    private void Show()
    {
        for (int i = 0; i < btns.Length; i++)
        {
            if(btns[i].interactable)//강화 가능할때만 텍스트도 보이기
            {
                btns[i].gameObject.SetActive(true);
                _upgradeAmountLists[i].gameObject.SetActive(true);
            }
        }
    }

    
    

    private void StatChanged(Stat stat, object value)
    {
        switch (stat)
        {
            case Stat.MaxHP:
                maxHPTxt.text = value.ToString();
                break;
            case Stat.MaxEnergy:
                maxPowerTxt.text = value.ToString();
                break;
            case Stat.Str:
                strTxt.text = value.ToString();
                break;
            case Stat.Luk:
                lukTxt.text = value.ToString();
                break;
            case Stat.Speed:
                speedTxt.text = ((float)value).ToString("F1");
                break;
            case Stat.MineSpeed:
                mineSpeedTxt.text = ((float)value).ToString("F1");
                break;
        }
    }

    private void ChangedLevel(int curLevel, int needExperienceToNextLevel)
    {
        Debug.Log("호출됨");
        if (curLevel <= 1)//게임 시작 초기화를 위해서 호출햇던거임
        {
            levelPointTxt.text = "포인트 : 0";
            return;
        }
        
        levelPoint += 1;
        levelPointTxt.text = $"포인트 : {levelPoint}";
        Show();
    }

    public void UpdateAmuletSlot(Item amuletItem)
    {
        amuletSlot.UpdateAmulet(amuletItem);
    }

    private void onLevelUpBtnClick(Stat targetStat, int index)
    {
        StatData data = GameEventsManager.Instance.statusEvents.StatLevelUpBtnClicked(targetStat);

        _upgradeAmountLists[index].text = $"+{data.increaseAmount}";
        if (data.curStatLevel == data.maxLevel)
        {
            btns[index].interactable = false;
            btns[index].gameObject.SetActive(false);
            _upgradeAmountLists[index].gameObject.SetActive(false);
        }

        levelPoint--;
        levelPointTxt.text = $"포인트 : {levelPoint}";
        if(levelPoint == 0)
        {
            Hide();
        }
    }

    private void ChangeUpgradeAmountText(Stat targetStat, int index)
    {
        StatData data = GameEventsManager.Instance.statusEvents.GetStatData(targetStat);

        _upgradeAmountLists[index].text = $"+{data.increaseAmount}";
    }
}
