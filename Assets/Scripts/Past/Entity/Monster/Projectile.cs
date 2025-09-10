using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;


    [Header("있을때만 넣기")] 
    [SerializeField] private bool hasExplode;
    [SerializeField] private int explosionId;
    [SerializeField] private GameObject explodePrefab;
    
    private float hitDamage;
    private Player target;
    private int id;

    private Coroutine releaseCo;
    
    private bool isActive = false;

    private void Awake()
    {
        if (hasExplode && !ObjectPoolManager.Instance.IsPoolRegistered(explosionId))
        {
            ObjectPoolManager.Instance.RegisterPrefab(explosionId, explodePrefab);
        }
    }


    private void OnEnable()
    {
        isActive = true;
        
        if(releaseCo != null)
            StopCoroutine(releaseCo);

        releaseCo = StartCoroutine(AutoRelease());
    }

    private IEnumerator AutoRelease()
    {
        if (!isActive)
        {
            //이미 플레이어와 충돌하여 사라진 경우
            yield break;
        }

        yield return new WaitForSeconds(lifeTime);

        if(hasExplode)
            ObjectPoolManager.Instance.ReleaseObject(id, gameObject);
    }
    
    
    public void SetUp(float damage, Player target, int id)
    {
        hitDamage = damage;
        this.target = target;
        this.id = id;

        Vector3 dir = (target.transform.position - transform.position).normalized;

        // 2D용 회전 (z축만 사용)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);  // Z축만 회전

        rb.velocity = dir * speed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target.OnDamage(hitDamage);
            isActive = false;
            releaseCo = null;

            SpawnExplosionEffect(other.transform);
            
            ObjectPoolManager.Instance.ReleaseObject(id, gameObject);
        }
    }

    private void SpawnExplosionEffect(Transform pos)
    {
        if(!hasExplode)
            return;
        
        ObjectPoolManager.Instance.SpawnObject(
            explosionId,
            pos.position,
            Quaternion.identity);
    }
}
