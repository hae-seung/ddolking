using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UpgradeItemExplain : MonoBehaviour
{
    [SerializeField] private GameObject scrollViewObject;

    [SerializeField] private TextMeshProUGUI itemClassTxt;
    [SerializeField] private TextMeshProUGUI itemNameTxt;
    [SerializeField] private TextMeshProUGUI statTxt;

    [SerializeField] private GameObject debuffObject;
    [SerializeField] private TextMeshProUGUI currentDebuffTxt;
    [SerializeField] private TextMeshProUGUI nextDebuffTxt;


    private EquipItem equipItem;
    private StringBuilder sb = new StringBuilder();
    
    public void Init()
    {
        scrollViewObject.SetActive(false);
        
        debuffObject.SetActive(false);
        currentDebuffTxt.text = "";
        nextDebuffTxt.text = "";
    }

    
    public void UpdateSet(EquipItem item)
    {
        equipItem = item;
        
        UpdateClass();
        UpdateName();
        UpdateStat();
        UpdateDebuff();
        
        scrollViewObject.SetActive(true);
    }

    private void UpdateDebuff()
    {
        if (equipItem.curLevel != 2 && equipItem.curLevel != 4)
        {
            debuffObject.SetActive(false);
            currentDebuffTxt.text = "";
            nextDebuffTxt.text = "";
            return;
        }

        if (equipItem is WeaponItem weapon)
        {
            string text = weapon.GetDebuffDescription();
            if (text.Length == 0) 
            {
                //디버프 없음
                currentDebuffTxt.text = "";
                nextDebuffTxt.text = "";
                debuffObject.SetActive(false);
            }
            else
            {
                currentDebuffTxt.text = text;
                nextDebuffTxt.text = $"{weapon.GetNextDebuffDescription()}";
                debuffObject.SetActive(true);
            }
        }
        
        
    }

    private void UpdateStat()
    {
        List<StatModifier> currentModifiers = equipItem.GetStatModifier();
        Dictionary<Stat, float> currentStats = new();

        if (currentModifiers != null)
        {
            foreach (var mod in currentModifiers)
            {
                currentStats[mod.stat] = mod.increaseAmount;
            }
        }

        // 다음 레벨 강화 정보 가져오기
        int currentLevel = equipItem.curLevel;
        int nextLevel = currentLevel + 1;

        var enhanceLogics = equipItem.GetLogic?.GetItemEnhanceLogic();
        var nextLogic = enhanceLogics?.Find(logic => logic.nextLevel == nextLevel);

        Dictionary<Stat, float> nextStats = new(currentStats); // 기본적으로 현재 값 복사

        if (nextLogic != null)
        {
            foreach (var enhance in nextLogic.statEnhancements)
            {
                if (nextStats.ContainsKey(enhance.stat))
                nextStats[enhance.stat] += enhance.enhanceValue;
                else
                    nextStats[enhance.stat] = enhance.enhanceValue;
            }
        }

        sb.Clear();

        // 현재와 다음 값 비교해서 표시
        foreach (var kvp in nextStats)
        {
            float curVal = currentStats.ContainsKey(kvp.Key) ? currentStats[kvp.Key] : 0;
            float nextVal = kvp.Value;

            if (curVal == 0 && nextVal == 0) continue; // 둘 다 0이면 생략

            string statName = kvp.Key switch
            {
                Stat.Defense => "경감률",
                Stat.CriticalDamage => "치명타 피해",
                Stat.ExperienceGetter => "경험치 증가",
                Stat.MaxHP => "최대체력",
                Stat.HP => "체력",
                Stat.MaxEnergy => "최대기력",
                Stat.Energy => "기력",
                Stat.Str => "근력",
                Stat.Critical => "치명타 확률",
                Stat.Speed => "이동속도",
                Stat.MineSpeed => "채굴력",
                _ => null
            };

            if (statName == null) continue;

            string formattedCurrent = FormatStatValue(kvp.Key, curVal);
            if (equipItem.curLevel >= 5 || curVal == nextVal)
            {
                sb.AppendLine($"{statName} : {formattedCurrent}");
            }
            else
            {
                string formattedNext = FormatStatValue(kvp.Key, nextVal);
                sb.AppendLine($"{statName} : {formattedCurrent} → {formattedNext}");
            }
        }

        statTxt.text = sb.ToString();
        statTxt.gameObject.SetActive(sb.Length > 0);
    }

    private string FormatStatValue(Stat stat, float value)
    {
        string sign = value > 0 ? "+" : ""; // 음수는 부호 제거
        float abs = Mathf.Abs(value);

        return stat switch
        {
            Stat.Defense => $"{sign}{(int)(abs * 100)}%",
            Stat.Critical => $"{sign}{(int)abs}%",
            Stat.CriticalDamage or Stat.ExperienceGetter => $"{sign}{abs:F1}배",
            Stat.Speed or Stat.MineSpeed => $"{sign}{abs:F1}",
            _ => $"{sign}{(int)abs}"
        };
    }




    private void UpdateName()
    {
        itemNameTxt.text = equipItem.itemData.Name;
    }

    private void UpdateClass()
    {
        switch (equipItem.EquipData.itemclass)
        {
            case ItemClass.Normal :
                itemClassTxt.text = "일반";
                itemClassTxt.color = Color.gray;
                break;
            case ItemClass.Epic :
                itemClassTxt.text = "고급";
                itemClassTxt.color = new Color32(138, 43, 226, 255);
                break;
            case ItemClass.Unique:
                itemClassTxt.text = "희귀";
                itemClassTxt.color = Color.yellow;
                break;
            case ItemClass.Legend :
                itemClassTxt.text = "전설";
                itemClassTxt.color = Color.green;
                break;
        }
    }
}
