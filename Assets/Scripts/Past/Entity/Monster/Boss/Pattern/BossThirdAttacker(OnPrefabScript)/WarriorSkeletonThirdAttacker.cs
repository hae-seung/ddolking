using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;




//SwordBullet 발사대
public class WarriorSkeletonThirdAttacker : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    
    private int id;//자기 자신 id
    private float damage;
    private float rotationTime;
    private float speed;
    private BossAI boss;

    //소환시킬 회전칼날 id
    private int swordId;

    private Coroutine rotationRoutine;
    private List<SkeletonSwordBullet> swordList = new();
    
    public void Init(int id, float attackDamage, float waitTime, float flySpeed, BossAI boss, int spawnSowrdId)
    {
        this.id = id;
        damage = attackDamage;
        rotationTime = waitTime;
        speed = flySpeed;
        this.boss = boss;
        swordId = spawnSowrdId;
        
        swordList.Clear();
        if(rotationRoutine != null)
            StopCoroutine(rotationRoutine);
    }

    public void StartRoutine()
    {
        int spawnCnt = Random.Range(3, 9);

        for (int i = 0; i < spawnCnt; i++)
        {
            SkeletonSwordBullet bullet = ObjectPoolManager.Instance.SpawnObject(
                swordId,
                transform.position,
                Quaternion.identity).GetComponent<SkeletonSwordBullet>();
            
            bullet.transform.parent = transform;
            Vector3 rot = Vector3.forward * 360 * i / spawnCnt;
            bullet.transform.Rotate(rot);

            bullet.transform.Translate(bullet.transform.up * 1f, Space.World);
            bullet.Init(swordId, speed, damage);
            
            swordList.Add(bullet);
        }

        rotationRoutine = StartCoroutine(RotationSword());
    }

    private IEnumerator RotationSword()
    {
        float timer = 0f;
        while(timer < rotationTime)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        
        
        for (int i = 0; i < swordList.Count; i++)
        {
            swordList[i].CanAttack();
            swordList[i].transform.SetParent(null, true);
            swordList[i].Launch();
        }
        
        ObjectPoolManager.Instance.ReleaseObject(id, gameObject);
    }
}
