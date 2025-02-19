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
        if (moveDir != Vector2.zero)
            animator.SetBool("isMove", true);
        else
            animator.SetBool("isMove", false);
        
        velocity = moveDir.normalized * moveSpeed;
        
        if (movementDisabled)
        {
            velocity = Vector2.zero;
        }
    }
    
    private void MouseMoved(Vector3 mousePos)
    {
        dir = mousePos - transform.position;
        animator.SetFloat("inputX", dir.x);
    }

    private void EnablePlayerMovement()
    {
        movementDisabled = false;
    }

    private void DisablePlayerMovement()
    {
        movementDisabled = true;
        velocity = Vector2.zero;
    }
    
    
}
