using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum VillageType
{
    Home,
    Spring,
    Summer,
    Autumn,
    Winter
}

public class FieldShortCut : MonoBehaviour
{
    [SerializeField] private Transform homePos;
    [SerializeField] private Transform springPos;
    [SerializeField] private Transform summerPos;
    [SerializeField] private Transform autumnPos;
    [SerializeField] private Transform winterPos;

    [SerializeField] private Button homeBtn;
    [SerializeField] private Button springBtn;
    [SerializeField] private Button summerBtn;
    [SerializeField] private Button autumnBtn;
    [SerializeField] private Button winterBtn;
    [SerializeField] private Button exitBtn;


    private Interactor player;
    
    public void Init()
    {
        //버튼 등록
        InitBtns();
    }
    

    public void Open(Interactor player)
    {
        this.player = player;
        gameObject.SetActive(true);
    }

    public void RegisterShortCut(VillageType type)
    {
        switch (type)
        {
            case VillageType.Home:
                homeBtn.interactable = true;
                break;
            case VillageType.Spring:
                springBtn.interactable = true;
                break;
            case VillageType.Summer:
                summerBtn.interactable = true;
                break;
            case VillageType.Autumn:
                autumnBtn.interactable = true;
                break;
            case VillageType.Winter:
                winterBtn.interactable = true;
                break;
        }
    }
    
    private void InitBtns()
    {
        homeBtn.onClick.AddListener(() =>
        {
            CloseUI();
            UIManager.Instance.StartTransition();
            player.transform.position = homePos.position;
        });
        
        springBtn.onClick.AddListener(() =>
        {
            CloseUI();
            UIManager.Instance.StartTransition();
            player.transform.position = springPos.position;
        });
        
        summerBtn.onClick.AddListener(() =>
        {
            CloseUI();
            UIManager.Instance.StartTransition();
            player.transform.position = summerPos.position;
        });
        
        autumnBtn.onClick.AddListener(() =>
        {
            CloseUI();
            UIManager.Instance.StartTransition();
            player.transform.position = autumnPos.position;
        });
        
        winterBtn.onClick.AddListener(() =>
        {
            CloseUI();
            UIManager.Instance.StartTransition();
            player.transform.position = winterPos.position;
        });
        
        exitBtn.onClick.AddListener(CloseUI);
    }

    private void CloseUI()
    {
        gameObject.SetActive(false);
        GameEventsManager.Instance.inputEvents.EnableInput();
        GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
    }
}
