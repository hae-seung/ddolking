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
    [SerializeField] private TextMeshProUGUI contentTxt;
    [SerializeField] private TextMeshProUGUI durabilityTxt;
    [SerializeField] private GameObject interactableTxt; //우클릭시 상호작용가능을 나타냄
    [SerializeField] private RectTransform parentRect;
    
    private StringBuilder sb = new StringBuilder();
    private RectTransform _rt;
    private CanvasScaler _canvasScaler;

    
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
    
        if (statModifiers != null)//스텟 변화가 일어나는 경우
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
                    Stat.MaxHP     => "최대체력",
                    Stat.HP        => "체력",
                    Stat.MaxEnergy => "최대기력",
                    Stat.Energy    => "기력",
                    Stat.Str       => "근력",
                    Stat.Critical  => "치명타",
                    Stat.Speed     => "이동속도",
                    Stat.MineSpeed => "채굴력",
                    _              => null
                };

                if (statName == null) continue;

                string formattedAmount = (statModifiers[i].stat == Stat.Speed || 
                                          statModifiers[i].stat == Stat.MineSpeed)
                    ? $"{sign}{absAmount:F1}"
                    : $"{sign}{(int)absAmount}";

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
        //1 툴팁의 Pivot을 (0,1)로 설정 → 좌상단 기준 정렬
        _rt.pivot = new Vector2(0f, 1f);

        // 🔹 `parentRect`가 null이 아니면 부모 UI로 사용
        if (parentRect == null)
        {
            return;
        }

        //2 슬롯의 네 모서리 좌표를 가져와 정확한 우하단 위치 계산
        Vector3[] slotCorners = new Vector3[4];
        slotRect.GetWorldCorners(slotCorners);
        Vector3 slotBottomRight = slotCorners[3]; // 네 번째 요소가 우하단 좌표

        // 3 우하단 좌표를 스크린 좌표로 변환
        Vector2 tooltipScreenPos = RectTransformUtility.WorldToScreenPoint(null, slotBottomRight);
        

        // 4 스크린 좌표를 `parentRect` 내부 좌표로 변환
        Vector2 tooltipLocalPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,  // 변환 기준 (툴팁이 속한 부모 UI)
            tooltipScreenPos,  // 변환할 스크린 좌표
            null,  // Overlay 모드일 경우 null
            out tooltipLocalPos
        );
        

        // 5 툴팁의 크기 가져오기
        float width = _rt.rect.width;
        float height = _rt.rect.height;

        // 6 툴팁이 화면 밖으로 나가는지 확인
        bool rightTruncated = tooltipScreenPos.x + width > Screen.width;
        bool bottomTruncated = tooltipScreenPos.y - height < 0f;

        // 7 툴팁 위치 조정 (오른쪽/아래쪽이 잘릴 경우)
        if (rightTruncated) tooltipLocalPos.x -= width;
        if (bottomTruncated) tooltipLocalPos.y += height;
        

        // 8 툴팁 위치 적용
        _rt.anchoredPosition = tooltipLocalPos;
    }



}
