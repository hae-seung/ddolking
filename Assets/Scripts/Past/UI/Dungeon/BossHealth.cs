using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI healthTxt;
    
    
    [SerializeField] private Image debuffIcon;
    [SerializeField] private Slider slider;


    private BossAI boss;

    public void OpenSlider(LivingEntity monsterBoss)
    {
        boss = monsterBoss as BossAI;

        levelTxt.text = boss.level;
        nameTxt.text = boss.name;

        
        boss.SetUI(slider, debuffIcon);
        UpdateHealthText(1);
        

        boss.onDamage += UpdateHealthText;
        boss.onDead += Dead;
        
        gameObject.SetActive(true);
    }

    private void Dead()
    {
        boss.onDamage -= UpdateHealthText;
        boss.onDead -= Dead;
        gameObject.SetActive(false);
    }

    private void UpdateHealthText(float damage)
    {
        healthTxt.text = $"{(int)boss.Hp} / {(int)slider.maxValue}";
    }

    
    
    
}
