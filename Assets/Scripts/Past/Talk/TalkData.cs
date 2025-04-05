using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TalkData
{
    public int npcId;
    [TextArea]
    public string context;
    [Tooltip("0:평볌, 1:웃음, 2:화남, 3:우울, 4:놀람")]
    public int portaritId;
    
    //해당 npcId에 접근해서 해당 npcId의 데이터에 일치하는 portriatId로 초상화 가져오기
}
