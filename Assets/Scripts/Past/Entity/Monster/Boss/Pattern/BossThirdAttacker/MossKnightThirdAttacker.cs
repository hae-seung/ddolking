using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossKnightThirdAttacker : BossThirdAttacker
{
    private float fallSpeed;
    
    public override void StartRoutine()
    {
        StartCoroutine(FallRoutine());
    }
    
    private IEnumerator FallRoutine()
    {
        yield return new WaitForSeconds(waitTime);
        
        while (Vector3.Distance(transform.position, des) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, des, 
                fallSpeed * Time.deltaTime);
            yield return null;
        }
        
        yield return new WaitForSeconds(destroyDelay);
        ObjectPoolManager.Instance.ReleaseObject(id, gameObject);
    }
    
    protected override void SetDes(Vector3 des)
    {
        Vector3 newDes = des + Vector3.down * 25f;
        this.des = newDes;
    }

    public override void SetFallSpeed(float speed)
    {
        fallSpeed = speed;
    }
    
}
