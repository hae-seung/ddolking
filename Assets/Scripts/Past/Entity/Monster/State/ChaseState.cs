using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState
{
    //플레이어를 쫓을때 달리거나 걷거나
    
    private float walkTime;
    private float runTime;

    private float timer;
    
    private bool isRunning = false;
    
    public ChaseState(MonsterController controller) : base(controller) {}

    public override void EnterState()
    {
        int chaseTypeIndex = Random.Range(1, 11);
        if (chaseTypeIndex == 1)//10% run
        {
            SetRun();
        }
        else//90% walk
        {
            SetWalk();
        }
        
        controller.ChasePlayer(isRunning);
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
        
        if (isRunning && timer >= runTime)
        {
            SetWalk();
        }
        else if(!isRunning && timer >= walkTime)
        {
            SetRun();
        }
        
        controller.ChasePlayer(isRunning);
    }

    public override void ExitState()
    {
        isRunning = false;
        timer = 0f;
        controller.StopMove();
    }


    private void SetWalk()
    {
        timer = 0f;
        isRunning = false;
        walkTime = Random.Range(3f, 5f);
    }

    private void SetRun()
    {
        timer = 0f;
        isRunning = true;
        runTime = Random.Range(2f, 4f);
    }
}