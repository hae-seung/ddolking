using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignResult : MonoBehaviour
{
    [SerializeField] private GameObject successObject;
    [SerializeField] private GameObject failObject;

    [SerializeField] private Button successExitBtn;
    [SerializeField] private Button failExitBtn;
    
    [SerializeField] private Button successUnlockBtn;

    private Action UnlockSign;
    
    public void Init()
    {
        //버튼 초기화
        successExitBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.CloseSignTab();
        });
        
        failExitBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.CloseSignTab();
        });
        
        successUnlockBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.CloseSignTab();
            UnlockSign?.Invoke();
        });
    }

    public void SetEvent(Action UnlockSign)
    {
        this.UnlockSign = UnlockSign;
    }
    
    public void SuccessQuest()
    {
        successObject.SetActive(true);
        failObject.SetActive(false);
    }

    public void FailQuest()
    {
        failObject.SetActive(true);
        successObject.SetActive(false);
    }
}
