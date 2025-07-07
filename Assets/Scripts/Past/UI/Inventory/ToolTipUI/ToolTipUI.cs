using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ToolTipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI equipItemClassTxt;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI statTxt;
    [SerializeField] private TextMeshProUGUI debuffText;
    [SerializeField] private TextMeshProUGUI contentTxt;
    [SerializeField] private TextMeshProUGUI durabilityTxt;
    [SerializeField] private GameObject interactableTxt; //우클릭시 상호작용가능을 나타냄
    [SerializeField] private RectTransform parentRect;
    
    private StringBuilder sb = new StringBuilder();
    private RectTransform _rt;
    private CanvasScaler _canvasScaler;


    private Color textColor = Color.black;
    
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);
    
    private void Awake()
    {
        Init();
        Hide();
    }

    private void Init()
    {
        TryGetComponent(out _rt);
        _rt.pivot = new Vector2(0f, 1f);//좌상단
        _canvasScaler = GetComponentInParent<CanvasScaler>();
    }

    public void SetItemInfo(Item item)
    {
        itemName.text = item.itemData.Name;
        contentTxt.text = $"{item.itemData.Description}";
        debuffText.text = "";
        
        statTxt.gameObject.SetActive(false);
        interactableTxt.SetActive(false);
        equipItemClassTxt.gameObject.SetActive(false);
        durabilityTxt.gameObject.SetActive(false);

        if (item is EquipItem equipItem)
            SetEquipItemText(equipItem);
        
        if (item is IUseable || item is AmuletItem || item is EstablishItem)
            interactableTxt.SetActive(true);
        
        if (item is IStatModifier statItem)
            SetStatText(statItem);

        if (item is WeaponItem weaponItem)
        {
            Debug.Log(weaponItem.GetDebuffDescription());
        }
    }

    private void SetEquipItemText(EquipItem equipItem)
    {
        if (equipItem.curLevel > 0)
            itemName.text += $" +{equipItem.curLevel}";

        if (equipItem is WeaponItem weapon)
        {
            string text = weapon.GetDebuffDescription();
            if (text.Length == 0) 
            {
                //디버프 없음
                debuffText.text = "";
            }
            else
            {
                debuffText.text = text;
                debuffText.color = textColor;
            }
        }
        
        SetClassText(equipItem);
        SetDurabilityText(equipItem);
    }

    private void SetDurabilityText(EquipItem equipItem)
    {
        durabilityTxt.text = $"{equipItem.CurDurability} / {equipItem.EquipData.maxDurability}";
        durabilityTxt.gameObject.SetActive(true);
    }

    private void SetClassText(EquipItem equipItem)
    {
        switch (equipItem.EquipData.itemclass)
        {
            case ItemClass.Normal :
                equipItemClassTxt.text = "일반";
                equipItemClassTxt.color = Color.gray;
                break;
            case ItemClass.Epic :
                equipItemClassTxt.text = "고급";
                equipItemClassTxt.color = new Color32(138, 43, 226, 255);
                break;
            case ItemClass.Unique:
                equipItemClassTxt.text = "희귀";
                equipItemClassTxt.color = Color.yellow;
                break;
            case ItemClass.Legend :
                equipItemClassTxt.text = "전설";
                equipItemClassTxt.color = Color.green;
                break;
        }
        equipItemClassTxt.gameObject.SetActive(true);
    }

    private void SetStatText(IStatModifier statItem)
{
    List<StatModifier> statModifiers = statItem.GetStatModifier();

    if (statModifiers != null)
    {
        sb.Clear();

        foreach (var stat in statModifiers)
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
                formattedAmount = $"{sign}{(int)(absAmount * 100)}%"; // 퍼센트 표기
            }
            else if (stat.stat == Stat.Critical)
            {
                formattedAmount = $"{sign}{(int)absAmount}%"; // 정수 % 표기
            }
            else if (stat.stat == Stat.CriticalDamage || stat.stat == Stat.ExperienceGetter)
            {
                formattedAmount = $"{sign}{absAmount:F1}배"; // 배수 표기
            }
            else if (stat.stat == Stat.Speed || stat.stat == Stat.MineSpeed)
            {
                formattedAmount = $"{sign}{absAmount:F1}"; // 소수 1자리
            }
            else
            {
                formattedAmount = $"{sign}{(int)absAmount}"; // 기본 정수 표기
            }

            sb.AppendLine($"{statName} : {formattedAmount}");
        }

        statTxt.text = sb.ToString();
        statTxt.gameObject.SetActive(true);
    }
    else
    {
        statTxt.gameObject.SetActive(false);
    }
}

    
    public void SetRectPosition(RectTransform slotRect)
    {
        if (parentRect == null || _rt == null)
            return;

        // 툴팁 크기 강제 갱신
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rt);

        // 기본적으로 슬롯의 우하단에 표시
        Vector3[] slotCorners = new Vector3[4];
        slotRect.GetWorldCorners(slotCorners);
        Vector3 targetWorldPos = slotCorners[3]; // 우하단

        // 월드 좌표 → 스크린 좌표 변환
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, targetWorldPos);

        // 툴팁의 크기
        float width = _rt.rect.width;
        float height = _rt.rect.height;

        // 기본 위치 (pivot은 0,1 → 좌상단 기준)
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            screenPos,
            null,
            out anchoredPos
        );

        // 화면 경계 고려
        float canvasWidth = parentRect.rect.width;
        float canvasHeight = parentRect.rect.height;

        // anchoredPos는 canvas 중심 기준이므로 보정이 필요
        float halfCanvasWidth = canvasWidth * 0.5f;
        float halfCanvasHeight = canvasHeight * 0.5f;

        // 좌우 경계
        float minX = -halfCanvasWidth;
        float maxX = halfCanvasWidth - width;

        // 위아래 경계 (툴팁 pivot이 좌상단이므로 Y좌표가 높을수록 위쪽임)
        float minY = -halfCanvasHeight + height;
        float maxY = halfCanvasHeight;

        // X 보정
        anchoredPos.x = Mathf.Clamp(anchoredPos.x, minX, maxX);

        // Y 보정
        anchoredPos.y = Mathf.Clamp(anchoredPos.y, minY, maxY);

        _rt.anchoredPosition = anchoredPos;
    }






}
