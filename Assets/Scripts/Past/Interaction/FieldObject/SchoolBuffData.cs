using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SchoolBuff", menuName = "SO/Buff/SchoolBuffData")]
public class SchoolBuffData : ScriptableObject
{
    [Header("서당 버프 종류 및 수치 범위")]
    [SerializeField] private List<SchoolBuffProbability> schoolBuffProbabilities;

    public List<SchoolBuffProbability> SchoolBuffProbabilities => schoolBuffProbabilities;

    /// <summary>
    /// 사용할 랜덤 버프 개수 반환 (1 이상, 종류 수 이하)
    /// </summary>
    public int GetRandomBuffCount()
    {
        return Random.Range(1, schoolBuffProbabilities.Count + 1); // 범위: [1, count]
    }
}

[System.Serializable]
public class SchoolBuffProbability
{
    public Stat stat;
    public float minValue;
    public float maxValue;
}


[System.Serializable]
public class WeightedInt
{
    public int value;      // 버프 개수
    public float weight;   // 해당 개수의 확률 (%)
}


