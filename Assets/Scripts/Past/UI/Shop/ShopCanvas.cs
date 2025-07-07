using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCanvas : MonoBehaviour
{
    [SerializeField] private PastShop pastShop;
    [SerializeField] private ReinforceShop reinforceShop;
    [SerializeField] private ShopPopup _popup;
    [SerializeField] private WarnPopup warnPopup;
    
    
    //todo: 미래

    private void Awake()
    {
        pastShop.gameObject.SetActive(false);
        _popup.gameObject.SetActive(false);
        warnPopup.gameObject.SetActive(false);
    }


    public void OpenShop(ShopType type)
    {
        GameEventsManager.Instance.inputEvents.DisableInput();
        GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
        
        switch (type)
        {
            case ShopType.past:
                pastShop.OpenShop();
                break;
            case ShopType.future:
                break;
            case ShopType.reinforce:
                reinforceShop.OpenShop();
                break;
        }
    }

    public void CloseShop()
    {
        GameEventsManager.Instance.inputEvents.EnableInput();
        GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
    }
    
    public void Warn(int amount)
    {
        warnPopup.Warn(amount);
    }
    
}
