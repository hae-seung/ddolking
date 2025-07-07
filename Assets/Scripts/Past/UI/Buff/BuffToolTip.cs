using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buffName;
    [SerializeField] private Image buffIcon;
    [SerializeField] private TextMeshProUGUI statTxt;
    [SerializeField] private GameObject schoolBuffWarn;

    [SerializeField] private Sprite schoolBuffIcon;

    private RectTransform _rt;
    private StringBuilder sb = new StringBuilder();
    

    public void SetToolTip(RectTransform iconRect, BuffItemData data)
    {
        if (_rt == null)
            _rt = GetComponent<RectTransform>();

        _rt.pivot = new Vector2(0f, 1f); // 좌상단 기준

        // 아이콘의 월드 좌표 구하기
        Vector3[] corners = new Vector3[4];
        iconRect.GetWorldCorners(corners);

        // corners[3] = 우하단 (Bottom Right)
        Vector3 worldPos = corners[3];

        // 월드 좌표를 로컬 좌표로 변환 (툴팁의 부모 기준)
        Vector3 localPos = _rt.parent.InverseTransformPoint(worldPos);

        _rt.anchoredPosition = localPos;

        SetData(data);
        
        schoolBuffWarn.SetActive(false);
        gameObject.SetActive(true);
    }

    private void SetData(BuffItemData data)
    {
        buffName.text = data.Name;
        buffIcon.sprite = data.IconImage;
        
        
        List<StatModifier> statModifiers = data.GetStatModifier();

        if (statModifiers != null) //스텟 변화가 일어나는 경우
        {
            sb.Clear();

            for (int i = 0; i < statModifiers.Count; i++)
            {
                float amount = statModifiers[i].increaseAmount;
                if (amount == 0) continue;

                string sign = amount > 0 ? "+" : "-";
                float absAmount = Mathf.Abs(amount);

                string statName = statModifiers[i].stat switch
                {
                    Stat.MaxHP => "최대체력",
                    Stat.HP => "체력",
                    Stat.MaxEnergy => "최대기력",
                    Stat.Energy => "기력",
                    Stat.Str => "근력",
                    Stat.Critical => "치명타",
                    Stat.Speed => "이동속도",
                    Stat.MineSpeed => "채굴력",
                    _ => null
                };

                if (statName == null) continue;

                string formattedAmount = (statModifiers[i].stat == Stat.Speed ||
                                          statModifiers[i].stat == Stat.MineSpeed)
                    ? $"{sign}{absAmount:F1}"
                    : $"{sign}{(int)absAmount}";

                sb.AppendLine($"{statName} : {formattedAmount}");
            }



            statTxt.text = sb.ToString();
        }
    }


    public void SetSchoolData(RectTransform iconRect, List<StatModifier> schoolBuffs)
{
    if (_rt == null)
        _rt = GetComponent<RectTransform>();

    _rt.pivot = new Vector2(0f, 1f); // 좌상단 기준

    // 아이콘의 월드 좌표 구하기
    Vector3[] corners = new Vector3[4];
    iconRect.GetWorldCorners(corners);

    Vector3 worldPos = corners[3]; // 우하단
    Vector3 localPos = _rt.parent.InverseTransformPoint(worldPos);
    _rt.anchoredPosition = localPos;

    // 데이터 세팅
    buffName.text = "서당 버프";
    buffIcon.sprite = schoolBuffIcon;

    sb.Clear();

    foreach (var stat in schoolBuffs)
    {
        if (stat.increaseAmount == 0) continue;

        string sign = stat.increaseAmount > 0 ? "+" : "-";
        float absAmount = Mathf.Abs(stat.increaseAmount);

        string statName = stat.stat switch
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

        string formattedAmount;

        if (stat.stat == Stat.Defense)
        {
            formattedAmount = $"{sign}{(int)(absAmount * 100)}%"; //표기만 % 형식으로
        }
        else if (stat.stat == Stat.Critical)
        {
            formattedAmount = $"{sign}{(int)absAmount}%";
        }
        else if (stat.stat == Stat.CriticalDamage || stat.stat == Stat.ExperienceGetter)
        {
            formattedAmount = $"{sign}{absAmount:F1}배";
        }
        else if (stat.stat == Stat.Speed || stat.stat == Stat.MineSpeed)
        {
            formattedAmount = $"{sign}{absAmount:F1}";
        }
        else 
        {
            formattedAmount = $"{sign}{(int)absAmount}";
        }

        sb.AppendLine($"{statName} : {formattedAmount}");
    }

    statTxt.text = sb.ToString();
    schoolBuffWarn.SetActive(true);
    gameObject.SetActive(true);
}



}
