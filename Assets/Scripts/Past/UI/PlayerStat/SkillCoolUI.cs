using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI coolText;


    private WaitForSeconds second = new WaitForSeconds(1f);
    private Coroutine coolCoroutine;
    
    
    public void UseSkill(SkillData data)
    {
        gameObject.SetActive(true);
        
        if(coolCoroutine != null)
            StopCoroutine(coolCoroutine);

        coolCoroutine = StartCoroutine(SkillCoolDown(data));
    }

    private IEnumerator SkillCoolDown(SkillData data)
    {
        int time = (int)data.CoolDown;
        image.sprite = data.Icon;
        coolText.text = time.ToString();
        
        while (time > 0)
        {
            yield return second;
            time--;
            coolText.text = time.ToString();
        }
        
        gameObject.SetActive(false);
    }
}
