using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineUI : MonoBehaviour
{
    [Header("구성요소")]
    [SerializeField] private TextMeshProUGUI mineName;
    [SerializeField] private TextMeshProUGUI spawnList;
    [SerializeField] private TextMeshProUGUI remainTime;
    [SerializeField] private Button enterBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button exitBtn;

    private DOTweenAnimation _doTweenAnimation;

    private Transform enterTransform;
    private Transform exitTransform;
    
    
    private Action ExitMine;
    private Action EnterMine;


    private void Awake()
    {
        _doTweenAnimation = GetComponentInChildren<DOTweenAnimation>();

        enterBtn.onClick.AddListener(() =>
        {
            CloseMineUI();
            EnterMine?.Invoke();
            exitBtn.gameObject.SetActive(true);
            GameEventsManager.Instance.playerEvents.MineEnter();
            UIManager.Instance.StartTransition();
        });

        closeBtn.onClick.AddListener(CloseMineUI);

        exitBtn.onClick.AddListener(() =>
        {
            GameEventsManager.Instance.playerEvents.ExitMine();
            UIManager.Instance.StartTransition();
            ExitMine?.Invoke();
            exitBtn.gameObject.SetActive(false);
        });

        exitBtn.gameObject.SetActive(false);
    }


    public void OpenMineUI(string name, string list,int remainTime ,Action EnterMine, Action ExitMine)
    {
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
        GameEventsManager.Instance.inputEvents.DisableInput();


        this.EnterMine = EnterMine;
        this.ExitMine = ExitMine;

        mineName.text = name;
        spawnList.text = list;

        if (remainTime > 0)
        {
            this.remainTime.text = $"남은시간 : {remainTime}시간";
            enterBtn.gameObject.SetActive(false);
        }
        else
        {
            this.remainTime.text = "입장가능";
            enterBtn.gameObject.SetActive(true);
        }

        
        exitBtn.gameObject.SetActive(false);//나가기 버튼은 광산 입장했을때 활성화
        
        _doTweenAnimation.DORestartById("show");
    }

    private void CloseMineUI()
    {
        GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
        GameEventsManager.Instance.inputEvents.EnableInput();
        
        _doTweenAnimation.DORestartById("hide");
        
        exitBtn.gameObject.SetActive(false);
    }
}
