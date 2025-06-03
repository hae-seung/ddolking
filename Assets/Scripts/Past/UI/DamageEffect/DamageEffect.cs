using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private DamageType damageType;
    
    private void OnEnable()
    {
        transform.localScale = new Vector3(
            Random.Range(0, 2) == 0 ? -1 : 1,
            transform.localScale.y,
            transform.localScale.z);
    }

    
    /// <summary>
    /// 클립에서 Event로 자동 실행
    /// </summary>
    public void ReleaseEffectToPool()
    {
        ObjectPoolManager.Instance.ReleaseObject((int)damageType, gameObject);
    }
}
