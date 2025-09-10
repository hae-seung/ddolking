using System;
using System.Collections;
using UnityEngine;

public class SwordFall : MonoBehaviour
{
    [SerializeField] private Vector3 destination;
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private SpriteRenderer warnSr;
    [SerializeField] private float waitTime = 1f;

    [Header("Warn Alpha 범위")]
    [SerializeField] private float minAlpha = 0.1f;
    [SerializeField] private float maxAlpha = 0.3f;


    private Player target;
    private float damage;
    
    private void OnEnable()
    {
        warnSr.gameObject.SetActive(true);
        StartCoroutine(WarningSword());
    }

    
    
    private IEnumerator WarningSword()
    {
        float elapsed = 0f;
        Color baseColor = warnSr.color;

        while (elapsed < waitTime)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.PingPong(elapsed * 2.5f, 1f);
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, t) * baseColor.a;
            warnSr.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

            yield return null;
        }

        DropSword();
    }

    private void DropSword()
    {
        warnSr.gameObject.SetActive(false);

        Vector3 targetPos = transform.position + destination;
        
        StartCoroutine(FallToDestination(targetPos));
    }

    private IEnumerator FallToDestination(Vector3 targetPos)
    {
        while (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                fallSpeed * Time.deltaTime
            );
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target.OnDamage(damage);
        }
    }

    public void SetTarget(Player player, float damage)
    {
        target = player;
        this.damage = damage;
    }
    
}