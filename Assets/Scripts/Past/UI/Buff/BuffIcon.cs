using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI countTxt;

    private RectTransform rt;
    private BuffUI buffUI;
    
    private BuffItemData buffData;
    private int uiIndex;
    private int remainTime;
    
    private WaitForSeconds delay = new WaitForSeconds(1f);

    private Action<int, BuffItemData> onEndBuff;
    private Coroutine buffCoroutine;
    
    public bool IsUsing { get; private set; }


    

    public void SetBuffData(BuffItemData data, int index, Action<int, BuffItemData> EndBuff, BuffUI ui)
    {
        if (rt == null)
            rt = GetComponent<RectTransform>();
        
        IsUsing = true;
        uiIndex = index;
        
        remainTime = 0;

        buffData = data;
        onEndBuff = EndBuff;
        buffUI = ui;
        
        remainTime = buffData.BuffDuration;
        image.sprite = buffData.IconImage;
        countTxt.text = remainTime.ToString();

        buffCoroutine = StartCoroutine(BuffCoroutine());
    }

    private IEnumerator BuffCoroutine()
    {
        while (remainTime >= -1)
        {
            countTxt.text = remainTime.ToString();

            if (remainTime == -1)
            {
                EndBuff();
                yield break;
            }

            yield return delay;
            remainTime--;
        }
    }

    private void EndBuff()
    {
        IsUsing = false;
        StopCoroutine(buffCoroutine);
        onEndBuff?.Invoke(uiIndex, buffData);
        onEndBuff = null;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        buffUI.OpenToolTip(rt, buffData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buffUI.CloseToolTip();
    }
}
