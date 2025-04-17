using System;

using UnityEngine;

[Serializable]
public class IdPortrait : SerializableDictionary<int, Sprite>{}

[CreateAssetMenu(fileName = "NPC", menuName = "SO/Talk/NPCData")]
public class NPCData : ScriptableObject
{
    public int npcId;
    public string npcName;
    public string npcJob;
    [Tooltip("0:평볌, 1:웃음, 2:화남, 3:우울, 4:놀람")]
    public IdPortrait npcPortrait;
}
