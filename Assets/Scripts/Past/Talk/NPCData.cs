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
    public int portraitId;
    public Sprite portrait;
}