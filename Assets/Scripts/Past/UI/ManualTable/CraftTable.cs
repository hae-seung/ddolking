using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TypeSprite : SerializableDictionary<CraftManualType, Sprite>{}

[System.Serializable]
public class TypeManual : SerializableDictionary<CraftManualType, CraftManualSO>{}

[System.Serializable]
public class TypeColor : SerializableDictionary<CraftManualType, Color>{}

[System.Serializable]
public class TypeString : SerializableDictionary<CraftManualType, string>{}



public class CraftTable : MonoBehaviour
{
    [Header("로그 배경이미지")] 
    [SerializeField] private TypeSprite craftLogBackgroundSprite;

    [Header("아이템 메뉴얼")] 
    [SerializeField] private TypeManual craftManualSo;
    
    [Header("버튼색상(배경이랑 비슷하게)")]
    [SerializeField] private TypeColor menuColor;

    [Header("테이블 이름 설정")] 
    [SerializeField] private TypeString tableName;

    [Header("테이블 로그")] 
    [SerializeField] private CraftTableLog _craftTableLog;

    [Header("아이템 리스트")] 
    [SerializeField] private GameObject contentParent;
    [SerializeField] private GameObject craftManualItemBtnPrefab;

    private Image image;
    private CraftManualSO manual;
    private DOTweenAnimation _doTweenAnimation;
    [SerializeField] private TextMeshProUGUI nameText;
    
    private List<CraftManualItemBtn> _btns = new();
    

    public void Init()
    {
        image = GetComponent<Image>();
        _doTweenAnimation = GetComponent<DOTweenAnimation>();
    }

    public void OpenTable(CraftManualType type)
    {
        image.sprite = craftLogBackgroundSprite[type];
        manual = craftManualSo[type];
        _craftTableLog.SetImage(craftLogBackgroundSprite[type]);
        nameText.text = tableName[type];

        
        //버튼 갯수 부족하면 버튼 추가
        if (_btns.Count < manual.CraftItems.Count)
        {
            int diff = manual.CraftItems.Count - _btns.Count;
            for (int i = 0; i < diff; i++)
            {
                CraftManualItemBtn btn = Instantiate(craftManualItemBtnPrefab, contentParent.transform)
                    .GetComponent<CraftManualItemBtn>();

                btn.Init(_craftTableLog);
                _btns.Add(btn);
            }
        }

        
        
        for (int i = 0; i < _btns.Count; i++)
        {
            if (i < manual.CraftItems.Count)
            {
                _btns[i].SetBtn(manual.CraftItems[i], menuColor[type]);
                _btns[i].gameObject.SetActive(true);
            }
            else
            {
                _btns[i].gameObject.SetActive(false);
            }
        }

        
        gameObject.SetActive(true);
        _doTweenAnimation.DORestartById("show");
    }
    

    public void onCloseBtnClicked()
    {
        UIManager.Instance.CloseCraftTab();
    }
}
