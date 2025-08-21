using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class BossAI : LivingEntity
{
    public Player target;
    
    [Header("보스 정보")]
    [TextArea] public string level;
    [TextArea] public string name;
     
    [SerializeField] private MonsterData bossData;
    [SerializeField] private MonsterAttack monsterAttack;
    [SerializeField] private Renderer renderer;
    
    [Header("DieFx 존재시에만, Id '0'은 존재x 의미")]
    [SerializeField] private int dieFxId;
    [SerializeField] private GameObject dieFxPrefab;

    [Header("고정스텟")]
    private float attackDamage;
    private float attackRange;
    
    [Header("평타 딜레이")]
    private float lastAttackTime;

    [Header("보스패턴")] 
    [SerializeField] private List<BossPattern> patterns;
    [SerializeField] private ThirdAttack thirdAttackPattern;
    [SerializeField] private float globalPatternDelay;
    private List<BossPatternState> patternStates;
    private float lastPatternEndTime;
    
    
    [SerializeField] private Animator animator;
    
    [Header("캐싱")]
    private string movingBlend = "MovingBlend";
    private string attack = "Attack";
    private WaitForSeconds dieTime = new WaitForSeconds(3.5f);
    
    private bool facingRight;

    private Vector3 rightFace = new Vector3(1, 1, 1);
    private Vector3 leftFace = new Vector3(1, 1, -1);
    
    protected override EntityData EntityData => bossData;
    public float AttackRange => attackRange * attackRange;
    public bool FacingRight => facingRight;


    private int currentAttackCnt;
    private bool isAttack;
    
    private void Init()
    {
        agent = GetComponent<NavMeshAgent>();

        SetFacing(rightFace);

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        renderer.sortingLayerName = "Front";
        renderer.sortingOrder = 2;
    }
    
    
    protected override void Awake()
    {
        Init();

        onDead += StartDead;
        data = bossData;
        
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
        agent.speed = bossData.MoveSpeed;
        
        lastAttackTime = 0f;        
        
        facingRight = true;
        SetFacing(rightFace);
        
        patternStates = new List<BossPatternState>();
        foreach (var p in patterns)
        {
            patternStates.Add(new BossPatternState(p));
        }

        lastPatternEndTime = Time.time;
    }

    private void Update()
    {
        if (target.IsDead || IsDead)
            return;
        
        if(!isAttack)
        {
            ChasePlayer();
        }
        
        CheckAndAttackPlayer();
        
        //보스는 플레이어를 쉬지 않고 추격함.
        //보스는 피격모션이 없음.
        //보스는 3번 평타시 강화 패턴이 존재.
    }

    private BossPatternState GetAvailablePattern() 
    {
        var ready = patternStates.FindAll(p => p.IsReady());
        if (ready.Count > 0) 
        {
            return ready[Random.Range(0, ready.Count)];
        }
        return null;
    }

    private void CheckAndAttackPlayer() 
    {
        if (Time.time - lastPatternEndTime >= globalPatternDelay) 
        {
            BossPatternState available = GetAvailablePattern();
            if (available != null) 
            {
                isAttack = true;
                animator.SetBool(attack, true);
                available.Use(); // 쿨타임 시작
                available.Pattern.UseSkill(this);
                StartCoroutine(EndSkill());
                agent.speed = 0f;
                return;
            }
        }
        
        CheckAndPerformBasicAttack();
    }


    private void CheckAndPerformBasicAttack()
    {
        Vector2 diff = transform.position - target.transform.position;
        float distance = diff.sqrMagnitude;

        if (distance <= AttackRange && Time.time - lastAttackTime >= bossData.AttackDelay)
        {
            isAttack = true;
            currentAttackCnt++;
            animator.SetBool(attack, true);
            agent.speed = 0f;
            lastAttackTime = Time.time;
        }
    }

    public void PerformAttack()
    {
        if (monsterAttack.Attack())
        {
            target.OnDamage(attackDamage);
        }
        
        if (currentAttackCnt >= 3)
        {
            thirdAttackPattern.Execute(this);
            currentAttackCnt = 0;
        }
    }
    
    
    private IEnumerator EndSkill()
    {
        yield return new WaitForSeconds(1f);
        isAttack = false;
        lastPatternEndTime = Time.time;
        animator.SetBool(attack, false);
    }

    public void OnAnimationEnd()
    {
        animator.SetBool(attack,false);
        animator.SetFloat(movingBlend,0.5f);
        agent.speed = bossData.MoveSpeed;
        isAttack = false;
    }
    


    public override void SetTarget(Player player, Transform pos)
    {
        target = player;
        
        agent.Warp(pos.position); // 내비메시 상의 유효한 위치로 강제로 이동
    }
    
    private void ChasePlayer()
    {
        if (overrideSpeed) return;
        
        animator.SetFloat(movingBlend, 0.5f);

        // NavMeshAgent 활성화
        agent.enabled = true;

        // 플레이어 위치 업데이트
        agent.SetDestination(target.transform.position);

        // 몬스터가 플레이어의 위치를 쫓도록 계속 설정
        CheckTargetPos(target.transform.position);
    }
    
    private void CheckTargetPos(Vector3 targetTransform)
    {
        if (targetTransform.x - transform.position.x >= 0)
        {
            facingRight = true;
            SetFacing(rightFace);
        }
        else
        {
            facingRight = false;
            SetFacing(leftFace);
        }
    }
    
    private void SetUp()
    {
        hp = bossData.Hp;
        defense = bossData.Defense;
        toolWear = bossData.ToolWear;
        
        
        attackRange = bossData.AttackRange;
        attackDamage = bossData.AttackDamage;

        lastAttackTime = 0f;
        currentAttackCnt = 0;
        isAttack = false;

        //onEnable에서 잔잔바리 작업 알아서 해줌. 여기서는 설정만 건드리면 됨.
    }
    
    public void PlayDieFx()
    {
        ObjectPoolManager.Instance.SpawnObject(
            dieFxId,
            transform.position,
            Quaternion.identity);
    }
    
    private void SetFacing(Vector3 facingDir)
    {
        animator.transform.localScale = facingDir;
    }
    
    private void StartDead()
    {
        animator.SetBool("IsDead", true);
        StartCoroutine(DeadAnim());
    }
    
    private IEnumerator DeadAnim()
    {
        yield return dieTime;
        DisableObject();
    }


    public void SetUI(Slider slider, Image debuffIcon)
    {
        healthSlider = slider;
        slider.maxValue = data.Hp;
        slider.value = Hp;
        slider.gameObject.SetActive(true);
        
        this.debuffIcon = debuffIcon;
        this.debuffIcon.gameObject.SetActive(false);
    }

    public void SetDefense(float def)
    {
        defense = def;
    }
    
}
