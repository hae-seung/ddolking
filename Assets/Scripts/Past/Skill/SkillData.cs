using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "SkillData", menuName = "SO/PlayerSkill")]
public class SkillData : ScriptableObject
{
    [Header("스킬 아이콘")] 
    [SerializeField] private Sprite icon;
    
    [Header("스킬상수")] 
    [SerializeField] private float skillConst;

    [Header("타격횟수 / 몇타")] 
    [SerializeField] private int attackCount;
    
    [Header("마릿수")]
    [SerializeField] private int hitCount;

    [Header("실제 스킬 프리팹(여기서 구현)")] 
    [SerializeField] private GameObject skillPrefab;

    [Header("스킬 쿨타임")]
    [SerializeField] private float coolDown;

    [Header("마나")]
    [SerializeField] private float mana;



    public Sprite Icon => icon;
    public float SkillConst => skillConst;
    public int AttackCount => attackCount;
    public int HitCount => hitCount;
    public float CoolDown => coolDown;
    public float Mana => mana;
    public GameObject SkillPrefab => skillPrefab;

}
