using System;
using UnityEngine;
using UnityEngine.AI;
using Cainos.Common;


public class Monster : LivingEntity
{
    //private로 바꿔야함
    public Player target;
    
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private MonsterAttack monsterAttack;
    private MonsterController controller;
    
    [Header("DieFx 존재시에만, Id '0'은 존재x 의미")]
    [SerializeField] private int dieFxId;
    [SerializeField] private GameObject dieFxPrefab;

    [Header("성장스텟")]
    protected float attackDamage;
    //private float bodyDamage;
    private int level;

    [Header("고정스텟")]
    private float attackRange;
    private float attackRangeY;
    private float sightRange;
    private float levelRatio;

    [Header("공격딜레이")]
    protected float lastAttackTime;

    private bool facingRight;

    private Vector3 rightFace = new Vector3(1, 1, 1);
    private Vector3 leftFace = new Vector3(1, 1, -1);


    [SerializeField] private Renderer renderer;    
    private BoxCollider2D _collider2D;
    private LayerMask objectLayer;
    private readonly Collider2D[] _cachedResults = new Collider2D[5];
    private int resultCount;
    
    
    protected override EntityData EntityData => monsterData;
    public float AttackRange => attackRange * attackRange;
    public float SightRange => sightRange * sightRange;
    public bool FacingRight => facingRight;

    
    
    public void Init(NavMeshAgent agent, MonsterController controller)
    {
        this.agent = agent;
        this.controller = controller;

        controller.SetFacing(rightFace);

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        levelRatio = monsterData.LevelRatio;

        objectLayer = LayerMask.GetMask("Obstacle");
        renderer.sortingLayerName = "Front";
    }
    
    
    
    protected override void Awake()
    {
        data = monsterData;
        _collider2D = GetComponent<BoxCollider2D>();
        
        if (dieFxId != 0 && !ObjectPoolManager.Instance.IsPoolRegistered(dieFxId))
        {
            ObjectPoolManager.Instance.RegisterPrefab(dieFxId, dieFxPrefab);
        }
    
        // 몬스터가 활성화될 때 NavMeshAgent 활성화
        agent.enabled = true;
    
        ObjectPoolManager.Instance.RegisterPrefab(data.EntityId, data.EntityPrefab);
        
        SetUp();
        
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        agent.enabled = true;
        
        lastAttackTime = 0f;        
        
        facingRight = true;
        controller.SetFacing(rightFace);
    }
    
    
    //애니메이션 event등록 호출
    public virtual void PerformAttack()
    {
        lastAttackTime = Time.time;
        if (monsterAttack.Attack())
        {
            //근거리 : 일단 공격을 행했고, 해당 범위내에 플레이어 있으면 데미지 주기
            //원거리 : 그냥 플레이어 방향으로 화살쏘기만 하면 됨.
            Debug.Log("기술공격");
            target.OnDamage(attackDamage);
        }
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("몸샷");
    //     }
    // }


    #region 움직임 관련

    public float GetDistanceToPlayer()
    {
        Vector2 diff = transform.position - target.transform.position;
        return diff.sqrMagnitude;
    }

    private bool CheckDistancePlayerY()
    {
        float diff = transform.position.y - target.transform.position.y;
        return Math.Abs(diff) <= attackRangeY;
    }

    public override void SetTarget(Player player, Transform pos)
    {
        target = player;
        
        agent.Warp(pos.position); // 내비메시 상의 유효한 위치로 강제로 이동
    }

    public override void MoveRandom()
    {
        if (overrideSpeed) return;

        // 비활성화 상태에서 경로 설정하려면, 먼저 활성화해야 함
        agent.enabled = true; // 비활성화 상태에서 이동이 불가능하므로 비활성화

        // 랜덤 방향으로 이동
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
    

    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + monsterData.AttackDelay && CheckDistancePlayerY();
    }

    public void ChasePlayer(bool isRun)
    {
        if (overrideSpeed) return;

        // NavMeshAgent 활성화
        agent.enabled = true;

        // 플레이어 위치 업데이트
        agent.SetDestination(target.transform.position);

        // 속도 설정
        if (isRun)
        {
            agent.speed = monsterData.RunSpeed;
        }
        else
        {
            agent.speed = monsterData.MoveSpeed;
        }

        // 몬스터가 플레이어의 위치를 쫓도록 계속 설정
        CheckTargetPos(target.transform.position);
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


    public override void SetLevel(int level)
    {
        if (level == 1)
        {
            SetUp();//기본베이스로 셋업
            return;
        }
        
        //레벨 2 이상이면 비율따라 증가하도록 셋업
        
        attackDamage = monsterData.AttackDamage * level * levelRatio;

        defense = monsterData.Defense * level * levelRatio;
        toolWear = monsterData.ToolWear * level * levelRatio;
        
        hp = monsterData.Hp * level * levelRatio;
        healthSlider.maxValue = hp;
        healthSlider.value = hp;
        
        lastAttackTime = 0f;
        
        //onEnable에서 잔잔바리 작업 알아서 해줌. 여기서는 설정만 건드리면 됨.
    }

    private void SetUp()
    {
        hp = monsterData.Hp;
        defense = monsterData.Defense;
        toolWear = monsterData.ToolWear;
        
        
        attackRange = monsterData.AttackRange;
        attackRangeY = monsterData.AttackRange * 0.5f;
        sightRange = monsterData.SightRange;
        attackDamage = monsterData.AttackDamage;
        

        lastAttackTime = 0f;
        
        //onEnable에서 잔잔바리 작업 알아서 해줌. 여기서는 설정만 건드리면 됨.
    }
    
    private void OnDrawGizmosSelected()
    {
        if (monsterData == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, monsterData.AttackRange);
    }


    public void PlayDieFx()
    {
        ObjectPoolManager.Instance.SpawnObject(
            dieFxId,
            transform.position,
            Quaternion.identity);
    }

    

    public void UpdateSortingOrder()
    {
        resultCount = Physics2D.OverlapBoxNonAlloc(
            _collider2D.bounds.center,
            _collider2D.bounds.size,
            0f,
            _cachedResults,
            objectLayer
        );

        float selfY = transform.position.y;

        for (int i = 0; i < resultCount; i++)
        {
            if (_cachedResults[i] == null) continue;

            float otherY = _cachedResults[i].transform.position.y;

            if (otherY < selfY)
            {
                SetSortingOrder(-2); // 뒤로
                return;
            }
        }

        SetSortingOrder(1); // 앞으로
    }

    private void SetSortingOrder(int order)
    {
        renderer.sortingOrder = order;
    }


}
