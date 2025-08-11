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
    private Coroutine makeRoutine;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void MakeItem(CraftItemSO craftItem, int amount, ReinforceStructureItem ritem, Action onFinish)
    {
        gameObject.SetActive(true);
        
        reminAmountTxt.text = $"{amount}";
        itemImage.sprite = craftItem.CraftItemData.IconImage;
        
        _craftItemSo = craftItem;
        
        makeTime = craftItem.MakingTime;

        int curLevel = ritem.GetLevel();
        makeTime *= (1 - ritem.GetEfficient(curLevel));
        
        totalAmount = amount;
        finishEvent = onFinish;
        
        slider.maxValue = makeTime;
        slider.value = 0f;
        
        makeRoutine = StartCoroutine(MakeItemCoroutine());
    }

    private IEnumerator MakeItemCoroutine()
    {
        for (int i = 1; i <= totalAmount; i++)//만들 아이템 총 갯수 
        {
            for (int time = 1; time <= Mathf.RoundToInt(makeTime); time++)//아이템 1개 제작
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
        makeRoutine = null;
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

    public void StopMaking()
    {
        if(makeRoutine != null)
            StopCoroutine(makeRoutine);
        gameObject.SetActive(false);
    }
}
