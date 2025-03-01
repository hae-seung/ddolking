using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CraftTable : MonoBehaviour
{
    [Header("테이블 타입")] 
    [SerializeField] private CraftManualType type;
    
    [Header("로그 배경이미지")]
    [SerializeField] private Sprite craftLogBackgroundSprite;

    [Header("아이템 메뉴얼")] 
    [SerializeField] private CraftManualSO craftManualSo;

    [Header("테이블 로그")] 
    [SerializeField] private CraftTableLog _craftTableLog;

    [Header("아이템 리스트")] 
    [SerializeField] private GameObject contentParent;
    [SerializeField] private GameObject craftManualItemBtnPrefab;
    
    [Header("버튼색상(배경이랑 비슷하게)")]
    [SerializeField] private Color menuColor;
    

    private void Awake()
    {
        for (int i = 0; i < craftManualSo.CraftItems.Count; i++)
        {
            CraftManualItemBtn btn = Instantiate(craftManualItemBtnPrefab, contentParent.transform)
                .GetComponent<CraftManualItemBtn>();

            btn.SetBtn(craftManualSo.CraftItems[i], _craftTableLog, menuColor);
        }
    }

    private void OnEnable()
    {
        _craftTableLog.SetImage(craftLogBackgroundSprite);
    }

    public void onCloseBtnClicked()
    {
        UIManager.Instance.ToggleCraftTab(type);
    }
}
