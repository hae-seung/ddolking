using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;
    private Vector2 velocity = Vector2.zero;
    private Vector2 dir;

    private bool movementDisabled = false;
    private bool mouseMovementDisabled = false;
    private string moveParameter = "isMove";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameEventsManager.Instance.inputEvents.onMovePressed += MovePressed;
        GameEventsManager.Instance.inputEvents.onMouseMoved += MouseMoved;
        GameEventsManager.Instance.playerEvents.onEnablePlayerMovement += EnablePlayerMovement;
        GameEventsManager.Instance.playerEvents.onDisablePlayerMovement += DisablePlayerMovement;
    }
    


    // private void Update() //애니메이션
    // {
    //     
    // }

    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    private void MovePressed(Vector2 moveDir)
    {
        if (movementDisabled)
        {
            velocity = Vector2.zero;
            animator.SetBool(moveParameter, false);
            return;
        }
        
        if (moveDir != Vector2.zero)
            animator.SetBool(moveParameter, true);
        else
            animator.SetBool(moveParameter, false);
        
        velocity = moveDir.normalized * moveSpeed;
        
    }
    
    private void MouseMoved(Vector3 mousePos)
    {
        if(!mouseMovementDisabled)
        {
            dir = mousePos - transform.position;
            animator.SetFloat("inputX", dir.x);
        }
    }

    private void EnablePlayerMovement()
    {
        movementDisabled = false;
        mouseMovementDisabled = false;
    }

    private void DisablePlayerMovement()
    {
        movementDisabled = true;
        mouseMovementDisabled = true;
        velocity = Vector2.zero;
    }
    
    
}
