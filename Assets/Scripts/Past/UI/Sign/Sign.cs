using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] private GameObject levelObject;
    [SerializeField] private GameObject moneyObject;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private SignResult signResult;
    
    
    private DOTweenAnimation _doTweenAnimation;
    private bool isInit = false;
    private int requireLevel;
    private int requireMoney;
    
    public void OpenSignTab(int needLevel, int needMoney, Action UnlockSign)
    {
        if (!isInit)
        {
            Init();
        }

        _doTweenAnimation.DORestartById("show");
        
        requireLevel = needLevel;
        requireMoney = needMoney;
        signResult.SetEvent(UnlockSign);
        
        SetSign();
    }

    public void CloseSignTab()
    {
        _doTweenAnimation.DORestartById("hide");
    }
    
    private void SetSign()
    {
        if (requireLevel > 0)
        {
            levelText.text = $"<u>축적된 경험</u>이 <color=red>{requireLevel}</color> 이상인 자";
            levelObject.SetActive(true);
        }
        else
        {
            levelObject.SetActive(false);
        }

        if (requireMoney > 0)
        {
            moneyText.text = $"<color=red>{requireMoney}개</color> 지불 가능한 자";
            moneyObject.SetActive(true);
        }
        else
        {
            moneyObject.SetActive(false);
        }

        
        
        if (CheckClearable())
        {
            signResult.SuccessQuest();
        }
        else
        {
            signResult.FailQuest();
        }
    }

    private bool CheckClearable()
    {
        if (PlayerWallet.Instance.PastMoney >= requireMoney &&
            GameEventsManager.Instance.playerEvents.GetLevel() >= requireLevel)
        {
            return true;
        }

        return false;
    }

    private void Init()
    {
        _doTweenAnimation = GetComponent<DOTweenAnimation>();
        signResult.Init();
        
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);
        
        isInit = true;
    }
}
