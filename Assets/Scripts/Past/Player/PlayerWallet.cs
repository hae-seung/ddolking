using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoneyType
{
    modern,
    past
}

public class PlayerWallet : MonoBehaviour
{
    public static PlayerWallet Instance;
    
    private int modernMoney;
    private int pastMoney;
    [SerializeField] private PlayerStatUI playerStatUI;

    
    public int ModernMoney => modernMoney;
    public int PastMoney => pastMoney;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        modernMoney = PlayerPrefs.GetInt("modernMoney", 5);
        pastMoney = PlayerPrefs.GetInt("pastMoney", 1);
        playerStatUI.ChangeMoney(modernMoney, pastMoney);
    }

    public void SpendMoney(MoneyType type, int amount)
    {
        
    }
    
    
}
