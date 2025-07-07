using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    [Header("일반버프")]
    [SerializeField] private GameObject buffIconPrefab;
    [SerializeField] private Transform buffParent;

    [Space(20)] 
    [SerializeField] private SchoolBuffIcon schoolBuffIcon;

    [Space(20)] 
    [SerializeField] private BuffToolTip buffToolTip;
    
    
    private List<BuffIcon> buffList = new List<BuffIcon>();
    private HashSet<int> currentBuffs = new HashSet<int>();
    
    private bool hasSchoolBuff;
    private List<StatModifier> schoolBuffList;

    public void Awake()
    {
        schoolBuffIcon.gameObject.SetActive(false);
        buffToolTip.gameObject.SetActive(false);
        schoolBuffIcon.Init(OpenSchoolToolTip, CloseSchoolToolTip);
        
        GameEventsManager.Instance.playerEvents.onApplyPortionBuff += ApplyPortionBuff;
        GameEventsManager.Instance.playerEvents.onApplySchoolBuff += ApplySchoolBuff;
        //서당버프 이벤트 0시에 초기화
        GameEventsManager.Instance.dayEvents.onChangeDay += EndSchoolBuff;
    }


    private bool ApplySchoolBuff(List<StatModifier> schoolBuffs)
    {
        if (hasSchoolBuff)
            return false;
        
        
        //버프 적용
        hasSchoolBuff = true;
        schoolBuffList = schoolBuffs;

        for (int i = 0; i < schoolBuffList.Count; i++)
        {
            GameEventsManager.Instance.statusEvents.AddStat(
                schoolBuffList[i].stat,
                schoolBuffList[i].increaseAmount);
        }
        
        schoolBuffIcon.gameObject.SetActive(true);
        
        return true;
    }
    

    private bool ApplyPortionBuff(BuffItem buffItem)
    {
        //이미 같은 종류의 버프가 있다면 적용 불가
        if (currentBuffs.Contains(buffItem.BuffData.BuffId))
            return false;


        bool hasApplyBuff = false;
        
        for (int i = 0; i < buffList.Count; i++)
        {
            if (!buffList[i].IsUsing)
            {
                buffList[i].gameObject.SetActive(true);
                buffList[i].SetBuffData(buffItem.BuffData, i, EndBuff, this);
                hasApplyBuff = true;
                break;
            }
        }

        if (!hasApplyBuff)
        {
            BuffIcon buffIcon = Instantiate(buffIconPrefab, buffParent).
                GetComponent<BuffIcon>();
            
            buffList.Add(buffIcon);
            buffIcon.SetBuffData(buffItem.BuffData, buffList.Count - 1, EndBuff, this);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(buffParent as RectTransform);
        }
        
        
        currentBuffs.Add(buffItem.BuffData.BuffId);
        
        //플레이어에게 스텟 적용
        for (int i = 0; i < buffItem.BuffData.GetStatModifier().Count; i++)
        {
            GameEventsManager.Instance.statusEvents.AddStat(
                buffItem.BuffData.GetStatModifier()[i].stat,
                buffItem.BuffData.GetStatModifier()[i].increaseAmount);
        }
        
        
        return true;
    }


    private void EndBuff(int index, BuffItemData data)
    {
        currentBuffs.Remove(data.BuffId);
        buffList[index].gameObject.SetActive(false);
        
        //플레이어에게서 스텟 감소
        for (int i = 0; i < data.GetStatModifier().Count; i++)
        {
            GameEventsManager.Instance.statusEvents.AddStat(
                data.GetStatModifier()[i].stat,
                -data.GetStatModifier()[i].increaseAmount);
        }
        
        CloseToolTip();
    }

    private void EndSchoolBuff(int day)
    {
        if (!hasSchoolBuff)
            return;
        
        //서당 버프 해제
        for (int i = 0; i < schoolBuffList.Count; i++)
        {
            GameEventsManager.Instance.statusEvents.AddStat(
                schoolBuffList[i].stat,
                -schoolBuffList[i].increaseAmount);
        }

        hasSchoolBuff = false;
        schoolBuffList = null;
        
        schoolBuffIcon.gameObject.SetActive(false);
        CloseSchoolToolTip();
    }


    public void OpenToolTip(RectTransform rt, BuffItemData buffData)
    {
        buffToolTip.SetToolTip(rt, buffData);
    }

    public void CloseToolTip()
    {
        buffToolTip.gameObject.SetActive(false);
    }

    private void OpenSchoolToolTip(RectTransform rt)
    {
        buffToolTip.SetSchoolData(rt, schoolBuffList);
    }

    private void CloseSchoolToolTip()
    {
        buffToolTip.gameObject.SetActive(false);
    }
    
}
