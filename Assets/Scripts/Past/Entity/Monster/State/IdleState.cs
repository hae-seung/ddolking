using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    //가만히 있거나 돌아다니거나
    private float stopTime;
    private float moveTime;
    
    private bool isMoving = false;
    
    private float timer;
    
    public IdleState(MonsterController controller) : base(controller){}
  
    
    public override void EnterState()
    {
        int idleTypeIndex = Random.Range(1, 3);
        if (idleTypeIndex == 1)//50% 멈춤
        {
           SetStop();
        }
        else//50% 배회
        {
            SetMove();
        }

        timer = 0f;
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
        
        if (isMoving)
        {
            if (timer >= moveTime)
            {
                SetStop();
            }
            else
            {
                controller.MoveRandom();//움직이는 시간이 끝나기도 전에 목적지에 도착했는지 확인
            }
        }
        else
        {
            if(timer >= stopTime)
                SetMove();
        }
    }

    public override void ExitState()
    {
        isMoving = false;
        timer = 0f;
        controller.StopMove();
    }


    private void SetStop()
    {
        //정지
        isMoving = false;
        stopTime = Random.Range(3f, 5f);
        timer = 0f;
        controller.StopMove();
    }

    private void SetMove()
    {
        //배회
        isMoving = true;
        moveTime = Random.Range(5f, 10f);
        timer = 0f;
        controller.MoveRandom();
    }
    
}
