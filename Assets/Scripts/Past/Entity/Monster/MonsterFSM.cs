using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : MonoBehaviour
{
    private Monster monster;
    private MonsterController controller;
    

    private Dictionary<StateType, BaseState> monsterState = new ();
    private StateType currentState;

    private bool isActive = false;

    private float timer = 0f;
    private float detectTime = 0.05f;
    
    public void Init(Monster monster, MonsterController controller)//Awake실행
    {
        isActive = false;
        
        this.monster = monster;
        this.controller = controller;

        
        monster.onDamage += OnDamaged;
        monster.onDead += OnDead;
        
        monsterState.Add(StateType.Idle, new IdleState(controller));
        monsterState.Add(StateType.Chase, new ChaseState(controller));
        monsterState.Add(StateType.Attack, new AttackState(controller));
        monsterState.Add(StateType.Injure, new InjureState(controller));
        monsterState.Add(StateType.Dead, new DeadState(controller));
    }

    
    

    private void OnEnable()
    {
        currentState = StateType.Idle;
        monsterState[currentState].EnterState();

        timer = 0f;
    }
    


    //StateType에 따라 업데이트
    private void Update()
    {
        if (currentState == StateType.Dead)
            return;

        DetectObstacleAndUpdateZ();
        
        monsterState[currentState].UpdateState();
        
        TryTransition();
    }
    


    private void TryTransition()
    {
        var state = monsterState[currentState];
        
        
        //1. 현재상태가 전이 가능한지 확인
        if (!state.CanTransition)
        {
            return;
        }
        
        
        //2.자유 상황일때 외부상황 체크
        float distance = monster.GetDistanceToPlayer();
        if (distance <= monster.AttackRange && monster.CanAttack())
        {
            ChangeState(StateType.Attack);
        }
        else if (distance <= monster.SightRange)
        {
            Debug.Log("쫓을 수 있어!");
            ChangeState(StateType.Chase);
        }
        else
        {
            ChangeState(StateType.Idle);
        }
    }


    private void ChangeState(StateType nextState)
    {
        if (currentState == nextState)
            return;
        
        monsterState[currentState].ExitState();
        currentState = nextState;
        monsterState[currentState].EnterState();
    }

    private void OnDamaged(float damage)
    {
        //현재 공격중이거나 피해를 입고 있는 경우 실행x
        if(currentState == StateType.Injure)
            return;
        
        ChangeState(StateType.Injure);
    }

    private void OnDead()
    {
        ChangeState(StateType.Dead);//무슨상황에서든 무조건 죽는 모션 실행
        isActive = false;
    }
    
    public void OnAnimationEnd()
    {
        if (currentState == StateType.Attack || currentState == StateType.Injure)
        {
            ChangeState(StateType.Idle);
        }
    }
    
    
    private void DetectObstacleAndUpdateZ()
    {
        timer += Time.deltaTime;

        if (timer >= detectTime)
        {
            timer = 0f;
            monster.UpdateSortingOrder();
        }
    }
    
}
