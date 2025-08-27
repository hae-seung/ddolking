using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCanvas : MonoBehaviour
{
    [SerializeField] private PastShop pastShop;
    [SerializeField] private FutureShop futureShop;
    [SerializeField] private ReinforceShop reinforceShop;
    [SerializeField] private DurabilityShop durabilityShop;
    [SerializeField] private ShopPopup _popup;
    [SerializeField] private WarnPopup warnPopup;
    
    
    //todo: 미래

    private void Awake()
    {
        pastShop.gameObject.SetActive(false);
        futureShop.gameObject.SetActive(false);
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
                futureShop.OpenShop();
                break;
            case ShopType.reinforce:
                reinforceShop.OpenShop();
                break;
            case ShopType.durability:
                durabilityShop.OpenShop();
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
