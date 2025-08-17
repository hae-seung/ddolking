using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ThirdAttack", menuName = "Entity/Boss/ThirdAttack")]
public class ThirdAttack : ScriptableObject
{



    public void Execute(BossAI boss)
    {
        Debug.Log("3타 추가타 발생");
    }
}
