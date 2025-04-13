using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TalkSessionData", menuName = "SO/Talk/TalkSession")]
public class TalkSessionData : ScriptableObject
{
    public int sessionId;
    public List<TalkData> talkDatas;
}
