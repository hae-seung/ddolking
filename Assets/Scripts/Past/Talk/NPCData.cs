using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NPC", menuName = "SO/Talk/NPCData")]
public class NPCData : ScriptableObject
{
    public int npcId;
    public string npcName;
    public string npcJob;
    public List<NPCPortraitInfo> npcPortrait;
}


[Serializable]
public class NPCPortraitInfo
{
    [Tooltip("0:평볌, 1:웃음, 2:화남, 3:우울, 4:놀람")]
    public int portraitId;
    public Sprite portrait;
}