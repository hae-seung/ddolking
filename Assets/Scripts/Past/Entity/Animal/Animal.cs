using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : LivingEntity
{
    [SerializeField] private EntityData animalData;

    private Animator animator;
    [SerializeField] private SpriteRenderer sr;
    private bool facingRight;
    private WaitForSeconds damageTime = new WaitForSeconds(0.2f);
    private WaitForSeconds dieTime = new WaitForSeconds(0.45f);


    private string walk = "isWalk";
    private string dead = "isDead";

    
    [SerializeField] private GrowAnimal growAnimal;
    [SerializeField] private FeedingInteraction feedingInteraction;
    
    protected override void Awake()
    {
        data = animalData;
        
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.enabled = true;

        onDamage += StartDamage;
        onDead += StartDead;
        base.Awake();
        
        SetUp();
    }

    private void StartDead()
    {
        animator.SetTrigger(dead);
        StartCoroutine(DeadAnim());
    }

    private IEnumerator DeadAnim()
    {
        yield return dieTime;
        DisableObject();
    }

    private void StartDamage(float damage)
    {
        StartCoroutine(DamageBlink());
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        agent.enabled = true;

        facingRight = true;
        sr.flipX = facingRight;
        
        SetUp();

        if (growAnimal)
            growAnimal.Enable();
        
        if(feedingInteraction)
            feedingInteraction.Enable();
        
        sr.color= Color.white;
    }
    
    private void SetUp()
    {
        hp = data.Hp;
        toolWear = data.ToolWear;
        defense = data.Defense;
    }

    private IEnumerator DamageBlink()
    {
        sr.color = Color.red;
        yield return damageTime;
        sr.color = Color.white;
    }

    public void SetMove()
    {
        if (!IsStopped())
        {
            return;
        }
        
        MoveRandom();
    }

    public override void MoveRandom()
    {
        //위치잡고 flip 생각해서 이동시키기
        if (overrideSpeed) return;
        
        animator.SetBool(walk, true);
        
        agent.enabled = true;
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        float moveDistance = 10f;
        Vector2 randomDestination = (Vector2)transform.position + randomDirection * moveDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDestination, out hit, 10f, NavMesh.AllAreas))
        {
            CheckTargetPos(hit.position);
            agent.speed = data.MoveSpeed;
            agent.SetDestination(hit.position);
        }
        else
        {
            StopMove();
        }
    }

    public override void StopMove()
    {
        base.StopMove();
        animator.SetBool(walk, false);
    }
    
    private void CheckTargetPos(Vector3 targetTransform)
    {
        if (targetTransform.x - transform.position.x >= 0)
        {
            facingRight = true;
        }
        else
        {
            facingRight = false;
        }
        sr.flipX = facingRight;
    }
    
}
