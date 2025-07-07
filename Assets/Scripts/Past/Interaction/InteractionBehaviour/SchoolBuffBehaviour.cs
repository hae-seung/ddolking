using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolBuffBehaviour : InteractionBehaviour
{
    [SerializeField] private SchoolBuffData schoolBuffData;

    private Animator animator;
    private bool isRefill;
    private List<StatModifier> statModifiers = new();

    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        GameEventsManager.Instance.dayEvents.onChangeDay += ChangeDay;
    }

    private void ChangeDay(int day)
    {
        isRefill = true;
        animator.SetBool("isBuff", true);
    }


    protected override void Interact(Interactor interactor, Item currentGripItem = null)
    {
        GetBuff();
    }

    private void GetBuff()
    {
        if (!isRefill) return;

        statModifiers.Clear();
        MakeNextRandomBuffs();

        // 버프 적용
        if (GameEventsManager.Instance.playerEvents.ApplySchoolBuff(statModifiers))
        {
            isRefill = false;
            animator.SetBool("isBuff", false);
        }
    }

    private void MakeNextRandomBuffs()
    {
        int buffCount = schoolBuffData.GetRandomBuffCount(); // 여기로 변경됨

        List<SchoolBuffProbability> available = new(schoolBuffData.SchoolBuffProbabilities);
        Shuffle(available);

        for (int i = 0; i < buffCount && i < available.Count; i++)
        {
            var buff = available[i];
            float randValue = Random.Range(buff.minValue, buff.maxValue);
            randValue = Mathf.Round(randValue * 2f) / 2f;
            
            StatModifier modifier = new StatModifier(buff.stat, randValue);
            statModifiers.Add(modifier);
        }
    }

   
    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}

