using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MakingItemUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI reminAmountTxt;


    private CraftItemSO _craftItemSo;
    private int totalAmount;
    private Action finishEvent;

    private float makeTime;
    
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    

    public void MakeItem(CraftItemSO craftItem, int amount, Action onFinish)
    {
        gameObject.SetActive(true);
        
        slider.maxValue = craftItem.MakingTime;
        slider.value = 0f;
        
        reminAmountTxt.text = $"{amount}";
        itemImage.sprite = craftItem.CraftItemData.IconImage;
        
        _craftItemSo = craftItem;
        totalAmount = amount;
        finishEvent = onFinish;
        
        StartCoroutine(nameof(MakeItemCoroutine));
    }

    private IEnumerator MakeItemCoroutine()
    {
        makeTime = _craftItemSo.MakingTime;
        
        for (int i = 1; i <= totalAmount; i++)//만들 아이템 총 갯수 
        {
            for (int time = 1; time <= makeTime; time++)//아이템 1개 제작
            {
                slider.value = time;
                if (slider.value == slider.maxValue)
                {
                    yield return new WaitForSeconds(0.1f);
                    break;
                }
                yield return new WaitForSeconds(1);
            }

            reminAmountTxt.text = $"{totalAmount - i}";
            slider.value = 0;

            InstanceObject();
        }
        
        gameObject.SetActive(false);
        finishEvent?.Invoke();
        finishEvent = null;
    }

    private void InstanceObject()
    {
        if (!ObjectPoolManager.Instance.IsPoolRegistered(_craftItemSo.CraftItemData.ID))
        {
            ObjectPoolManager.Instance.RegisterPrefab(_craftItemSo.CraftItemData.ID,
                _craftItemSo.CraftItemData.DropObjectPrefab);
        }

        Vector3 dropPosition = transform.position +
                               new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.5f, -1.0f), 0);
        GameObject dropObj = ObjectPoolManager.Instance.SpawnObject(
            _craftItemSo.CraftItemData.ID,
            dropPosition,
            Quaternion.Euler(0, 0, 0));

        if (dropObj != null)
        {
            dropObj.transform.DOJump(dropPosition, 1f, 1, 0.8f).SetEase(Ease.OutBounce);
        }
    }
}
