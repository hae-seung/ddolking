using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pastAmountTxt;
    [SerializeField] private TextMeshProUGUI nextUpdateTxt;
    [SerializeField] private TextMeshProUGUI probabilityTxt;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button confirmBtn;


    private bool isInit = false;
    private int remainDay = 3;//다음 갱신일까지

    private int maxAmount;//현재 가진 500원 갯수
    [SerializeField] private int exchangeAmount = 5; //교환될 은자 갯수
    private int probability;//다음 갱신때 상승 하락 확률(성공기준 확률)
    
    
    public void OpenExchangeTab()
    {
        if (!isInit)
            Init();

        maxAmount = PlayerWallet.Instance.ModernMoney;
        inputField.text = PlayerWallet.Instance.ModernMoney >= 1 ? "1" : "0";
        
        gameObject.SetActive(true);
    }

    private void Init()
    {
        probability = 5;//50퍼 확률로 시작
        
        inputField.onValueChanged.AddListener(str =>
        {
            int.TryParse(str, out int amount);
            bool flag = false;
            
            if (maxAmount <= 0)
            {
                flag = true;
                amount = 0;
            }
            else if (amount < 1)
            {
                flag = true;
                amount = 1;
            }
            else if (amount > maxAmount)
            {
                flag = true;
                amount = maxAmount;
            }

            if (flag)
                inputField.text = amount.ToString();
        });
        
        
        confirmBtn.onClick.AddListener(() =>
        {
            if (maxAmount <= 0)
                return;

            int amount = int.Parse(inputField.text);
            PlayerWallet.Instance.SpendMoney(MoneyType.modern, amount);
            PlayerWallet.Instance.GetMoney(MoneyType.past, exchangeAmount * amount);
            maxAmount -= amount;
            if (maxAmount <= 0)
                inputField.text = "0";
        });


        SetProbabilityText();
        SetExchangeAmountText();
        nextUpdateTxt.text = $"다음 갱신일까지 {remainDay}일";
        
        GameEventsManager.Instance.dayEvents.onChangeDay += ChangeDay;//날짜 변경 구독
        
        isInit = true;
    }
    

    private void SetExchangeAmountText()
    {
        pastAmountTxt.text = $"x{exchangeAmount}";
    }

    private void SetProbabilityText()
    {
        probabilityTxt.text = $"<color=red>하락 : {100 - (probability) * 10}%</color> \n" +
                              $"<color=blue>상승 : {probability * 10}%";
    }

    private void ChangeDay(int day)
    {
        remainDay -= 1;
        SetRemainText();
    }

    private void SetRemainText()
    {
        if (remainDay == 0)
        {
            UpdateExchange();
            remainDay = 3;
        }

        nextUpdateTxt.text = $"다음 갱신일까지 {remainDay}일";
    }

    private void UpdateExchange()
    {
        int ran = Random.Range(0, 100);  // 0~99
        int successRate = probability * 10;

        bool isSuccess = ran < successRate;

        if (isSuccess)//상승
        {
            probability = Mathf.Max(0, probability - 1);
            int delta = GetRandomDelta();  // +1~+5
            exchangeAmount += delta;
        }
        else//실패
        {
            probability = Mathf.Min(10, probability + 1);
            int delta = GetRandomDelta();  // +1~+5, 나중에 - 부호 붙이기
            exchangeAmount = Mathf.Max(1, exchangeAmount - delta);
        }

        SetExchangeAmountText();
        SetProbabilityText();
    }
    
    private int GetRandomDelta()
    {
        int rand = Random.Range(0, 100);

        if (rand < 40) return 1;  // 40%
        if (rand < 70) return 2;  // 30%
        if (rand < 90) return 3;  // 20%
        if (rand < 95) return 4;  // 5%
        return 5;                 // 5%
    }

}
