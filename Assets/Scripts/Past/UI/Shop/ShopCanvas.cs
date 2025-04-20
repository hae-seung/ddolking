using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCanvas : MonoBehaviour
{
    [SerializeField] private PastShop pastShop;
    [SerializeField] private ShopPopup _popup;
    [SerializeField] private WarnPopup warnPopup;
    
    //todo: 미래

    private void Awake()
    {
        pastShop.gameObject.SetActive(false);
        _popup.gameObject.SetActive(false);
        warnPopup.gameObject.SetActive(false);
    }


    public void OpenShop(string type)
    {
        switch (type)
        {
            case "past":
                pastShop.OpenShop();
                break;
            case "future":
                break;
        }
    }

    public void Warn(int amount)
    {
        warnPopup.Warn(amount);
    }
    
}
