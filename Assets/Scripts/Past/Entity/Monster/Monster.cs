using System;
using UnityEngine;
using UnityEngine.AI;

public class Monster : LivingEntity
{
    public Player target;

    [SerializeField] private MonsterData monsterData;
    [SerializeField] private MonsterAttack monsterAttack;
    private MonsterController controller;

    [Header("성장스텟")]
    private float attackDamage;
    private float bodyDamage;
    private int level;

    [Header("고정스텟")]
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

        attackRange = monsterData.AttackRange;
        attackRangeY = monsterData.AttackRange / 2;
        sightRange = monsterData.SightRange;
        attackDamage = monsterData.AttackDamage;
        bodyDamage = monsterData.BodyDamage;

        defense = monsterData.Defense;
        toolWear = monsterData.ToolWear;

        lastAttackTime = 0f;
    }

    public void Init(NavMeshAgent agent, MonsterController controller)
    {
        this.agent = agent;
        this.controller = controller;

        controller.SetFacing(rightFace);

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    protected override void OnEnable()
    {
        facingRight = true;
        controller.SetFacing(rightFace);

        hp = monsterData.Hp;

        base.OnEnable();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
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
            Debug.Log("기술공격");
            target.OnDamage(attackDamage);
        }
    }

    #region 움직임 관련

    public float GetDistanceToPlayer()
    {
        Vector2 diff = transform.position - target.transform.position;
        return diff.sqrMagnitude;
    }

    private bool CheckDistancePlayerY()
    {
        if (monsterData.IsAdCarry)
            return true;

        float diff = transform.position.y - target.transform.position.y;
        return Math.Abs(diff) <= attackRangeY;
    }

    public void SetTarget(Player player)
    {
        target = player;
    }

    public override void MoveRandom()
    {
        if (overrideSpeed) return;

        agent.isStopped = false;

        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        float moveDistance = 10f;
        Vector2 randomDestination = (Vector2)transform.position + randomDirection * moveDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDestination, out hit, 10f, NavMesh.AllAreas))
        {
            CheckTargetPos(hit.position);
            agent.speed = monsterData.MoveSpeed;
            agent.SetDestination(hit.position);
        }
        else
        {
            controller.StopMove();
        }
    }

    public bool IsStopped()
    {
        return agent.isStopped || !agent.hasPath;
    }

    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + monsterData.AttackDelay && CheckDistancePlayerY();
    }

    public void ChasePlayer(bool isRun)
    {
        if (overrideSpeed) return;

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
        if (targetTransform.x - transform.position.x >= 0)
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
        if (monsterData == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, monsterData.AttackRange);
    }
}
