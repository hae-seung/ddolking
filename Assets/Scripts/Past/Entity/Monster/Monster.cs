using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : LivingEntity
{
    public Player target;
    
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private MonsterAttack monsterAttack;
    private MonsterController controller;

    
    [Header("성장스텟")] //Awake에서 초기화, Enable에서 강화
    //부모의 hp
    //보모의 toolWear
    //부모의 defense
    private float attackDamage;
    private float bodyDamage;
    private int level;
    
    [Header("고정스텟")]//Awake에서 초기화
    private float attackRange;
    private float attackRangeY;
    private float sightRange;
    
    
    [Header("공격딜레이")]
    private float lastAttackTime;


    private bool facingRight;

    private Vector3 rightFace = new Vector3(1, 1, 1);
    private Vector3 leftFace = new Vector3(1, 1, -1);

    protected override EntityData EntityData => monsterData;
    public float AttackRange => attackRange * attackRange;
    public float SightRange => sightRange * sightRange;
    public bool FacingRight => facingRight;
    

    protected override void Awake()
    {
        base.Awake();
        
        //몬스터 데이터
        attackRange = monsterData.AttackRange;
        attackRangeY = monsterData.AttackRange / 2;
        sightRange = monsterData.SightRange;
        attackDamage = monsterData.AttackDamage;
        bodyDamage = monsterData.BodyDamage;
        
        //부모
        defense = monsterData.Defense;
        toolWear = monsterData.ToolWear;
        
        lastAttackTime = 0f;
    }
    
    public void Init(NavMeshAgent agent, MonsterController controller)
    {
        //agent랑 controller할당
        this.agent = agent;
        this.controller = controller;
        
        this.controller.SetFacing(rightFace);
        
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    
    private void OnEnable()
    {
        facingRight = true;
        controller.SetFacing(rightFace);
        
        hp = monsterData.Hp;//날짜에 따른 성장 체력
        
        
        
        //마지막 실행
        base.OnEnable();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("몸샷");
            target.OnDamage(bodyDamage);
        }
    }


    public void PerformAttack()
    {
        lastAttackTime = Time.time;
        if (monsterAttack.Attack())
        {
            //플레이어에게 공격로직
            Debug.Log("기술공격");
            target.OnDamage(attackDamage);
        }
    }
    


    #region 몬스터 움직임

    public float GetDistanceToPlayer()
    {
        //제곱된 거리 반환
        Vector2 diff = transform.position - target.transform.position;
        float sqrDistance = diff.sqrMagnitude;
        
        return sqrDistance;
    }
    
    private bool CheckDistancePlayerY()
    {
        if (monsterData.IsAdCarry)//원거리는 플레이어와의 Y축 신경X
            return true;
        
        //근거리는 Y축 범위가 X축보다 절반
        float diff = transform.position.y - target.transform.position.y;
        if (Math.Abs(diff) <= attackRangeY)
            return true;

        return false;
    }

    
    //스포너가 설정
    public void SetTarget(Player player)
    {
        target = player;
    }
    
    public override void MoveRandom()
    {
        agent.isStopped = false;

        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        float moveDistance = 10f;
        Vector2 randomDestination = (Vector2)transform.position + randomDirection * moveDistance;

        NavMeshHit hit;
        float sampleRadius = 10f;
        if (NavMesh.SamplePosition(randomDestination, out hit, sampleRadius, NavMesh.AllAreas))
        {
            CheckTargetPos(hit.position);
            
            agent.speed = monsterData.MoveSpeed;
            agent.SetDestination(hit.position);
        }
        else//랜점 목적지를 잡지 못한 경우
        {
            controller.StopMove();
        }
    }

    public bool IsStopped()
    {
        //움직이는 지속시간이 끝나기도 전에 목적지에 도착하거나 랜덤목적지를 잡지 못한경우
        if (agent.isStopped || !agent.hasPath)
            return true;
        
        return false;
    }
    
    public bool CanAttack()
    {
        if (Time.time >= lastAttackTime + monsterData.AttackDelay
            && CheckDistancePlayerY())
        {
            return true;
        }

        return false;
    }


    public void ChasePlayer(bool isRun)
    {
        agent.isStopped = false;

        if (isRun)
        {
            agent.speed = monsterData.RunSpeed;
            agent.acceleration = monsterData.Acc;
        }
        else
        {
            agent.speed = monsterData.MoveSpeed;
            agent.acceleration = 9999f;
        }
        
        CheckTargetPos(target.transform.position);
        agent.SetDestination(target.transform.position);
    }
    

    private void CheckTargetPos(Vector3 targetTransform)
    {
        if (targetTransform.x - transform.position.x >= 0)//right facing
        {
            facingRight = true;
            controller.SetFacing(rightFace);
        }
        else
        {
            facingRight = false;
            controller.SetFacing(leftFace);
        }
    }

    #endregion
    
    private void OnDrawGizmosSelected()
    {
        if (monsterData == null)
            return;
        
        Gizmos.color = Color.red; // 기즈모 색상 설정
        Gizmos.DrawWireSphere(transform.position, monsterData.AttackRange); // 원 그리기
    }
}
