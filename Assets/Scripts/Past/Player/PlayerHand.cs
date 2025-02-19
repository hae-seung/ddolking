using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.XR;

public class PlayerHand : MonoBehaviour
{
    private QuickSlotUI quickSlotUI;
    private int currentQuickSlotNumber;// 1 ~ 5번
    private Item currentGripItem; 
    //아이템과 상호작용은 Inventory의 InteractWithItem으로 호출할거임. 그래서 currentQuickSlotNumber로 호출
    
    [SerializeField] private SpriteRenderer weaponImage;
    [SerializeField] private SpriteRenderer toolImage;
    [SerializeField] private SpriteRenderer elseImage;
    
    private void Awake()
    {
        quickSlotUI = FindObjectOfType<QuickSlotUI>();
        
        currentQuickSlotNumber = 1;
        quickSlotUI.SelectSlot(currentQuickSlotNumber);
        UpdateHand(currentQuickSlotNumber);
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.inputEvents.onNumBtnPressed += NumBtnPressed;
    }

    private void OnDisable()
    {
        //GameEventsManager.Instance.inputEvents.onNumBtnPressed -= NumBtnPressed;
    }

    private void NumBtnPressed(string key)
    {
        int selectedNum = int.Parse(key);

        if (selectedNum.Equals(currentQuickSlotNumber))
            return;
        
        quickSlotUI.SelectSlot(currentQuickSlotNumber, selectedNum);
        currentQuickSlotNumber = selectedNum; //일단 슬롯 번호만 바꿈
        
        UpdateHand(currentQuickSlotNumber);
    }

    public void UpdateHand(int slotNum)
    {
        // 현재 선택된 슬롯인지 확인
        if (currentQuickSlotNumber != slotNum) 
            return;

        Item newItem = Inventory.Instance.GetItem(currentQuickSlotNumber - 1);

        // 현재 아이템과 새로운 아이템이 다를 경우만 업데이트
        if (newItem == currentGripItem) 
            return;

        // 모든 이미지 비활성화
        weaponImage.gameObject.SetActive(false);
        toolImage.gameObject.SetActive(false);
        elseImage.gameObject.SetActive(false);

        // 새로운 아이템이 있을 경우 적절한 이미지 활성화
        SpriteRenderer targetImage = newItem switch
        {
            WeaponItem => weaponImage,
            ToolItem => toolImage,
            _ => elseImage // 기본 아이템 처리
        };

        targetImage.gameObject.SetActive(newItem != null);
        if (newItem != null) 
            targetImage.sprite = newItem.itemData.IconImage;

        // 아이템 변경 반영
        ChangeStat(newItem);
        currentGripItem = newItem;
    }



    private void ChangeStat(Item newItem)
    {
        //현재 쥐고 있는 아이템의 스텟만큼 감소
        if(currentGripItem is EquipItem equipItem && currentGripItem is not AmuletItem)
            ReduceStat(equipItem);
        
        //새로 장착될 무기의 스텟만큼 더하기
        if(newItem is EquipItem eitem && newItem is not AmuletItem)
            AddStat(eitem);
    }

    private void ReduceStat(EquipItem eitem)
    {
        List<StatModifier> statModifiers = eitem.GetStatModifier();
        if (statModifiers != null)
        {
            for (int i = 0; i < statModifiers.Count; i++)
            {
                GameEventsManager.Instance.statusEvents.
                    AddStat(statModifiers[i].stat, -statModifiers[i].increaseAmount);
            }
        }
    }

    private void AddStat(EquipItem eitem)
    {
        List<StatModifier> statModifiers = eitem.GetStatModifier();
        if (statModifiers != null)
        {
            for (int i = 0; i < statModifiers.Count; i++)
            {
                GameEventsManager.Instance.statusEvents.
                    AddStat(statModifiers[i].stat, statModifiers[i].increaseAmount);
            }
        }
    }
}
