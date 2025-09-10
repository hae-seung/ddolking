using System;
using System.Collections;
using UnityEngine;

public class AutoRelease : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private int id;

    private void OnEnable()
    {
        StartCoroutine(ReleaseAuto());
    }
    
    
    private IEnumerator ReleaseAuto()
    {
        yield return new WaitForSeconds(lifeTime);
        
        ObjectPoolManager.Instance.ReleaseObject(id, gameObject);
        
    }
    
}
