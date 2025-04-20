using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WarnPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI warnText;
    [SerializeField] private DOTweenAnimation _doTweenAnimation;
    
    public void Warn(int amount)
    {
        if (amount < 0)//인벤토리 칸 부족
        {
            warnText.text = "인벤토리 최소 <color=red>2칸</color>은 비워놓아야 합니다.";
        }
        else//돈 부족
        {
            warnText.text = $"<color=red>잔액부족!</color> 최대 {amount}개 구매가능 합니다.";
        }
        
        gameObject.SetActive(true);
        _doTweenAnimation.DORestartById("show");
    }

    public void CloseWarn()//버튼 인스펙터에 할당
    {
        gameObject.SetActive(false);
    }
}
