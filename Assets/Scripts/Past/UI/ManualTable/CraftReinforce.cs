using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CraftReinforce : MonoBehaviour
{
    [SerializeField] private List<GameObject> stars;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image toolImage;
    [SerializeField] private Button confirmBtn;
    [SerializeField] private GameObject rayBlocker;

    private UnityAction updateAction;
    private ItemData itemData;
    private ReinforceStructureItem ritem;
    
    
    private void Awake()
    {
        confirmBtn.onClick.AddListener(() =>
        {
            ritem.LevelUp();
            updateAction?.Invoke();
            Inventory.Instance.RemoveItem(itemData);
            Close();
        });
    }

    public void Close()
    {
        gameObject.SetActive(false);
        rayBlocker.SetActive(false);
    }


    public void OpenUI(ReinforceStructureItem ritem, UnityAction UpdateState)
    {
        this.ritem = ritem;
        
        rayBlocker.SetActive(true);
        gameObject.SetActive(true);

        int nextLevel = ritem.GetLevel() + 1;
        
        //강화매니저로부터 id를 통해 현재 레벨과 관련된 정보들을 가져와서 할당
        for (int i = 0; i < stars.Count; i++)
            stars[i].SetActive(i < nextLevel);

        float effi = ritem.GetEfficient(nextLevel) * 100;
        description.text = $"생산속도 {(int)effi}% 감소";
        toolImage.sprite = ritem.GetSprite(nextLevel);
        
        
        updateAction = UpdateState;
        
        //강화에 필요한 재료 있는지 확인
        itemData = ritem.GetNextLevelNeedItem(nextLevel);
        int amount = 
            Inventory.Instance.GetItemTotalAmount(itemData);
        
        confirmBtn.interactable = amount >= 1;
    }

   
    
}
