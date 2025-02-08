using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 velocity = Vector2.zero;
    //private Animator animator;
    private SpriteRenderer visual;

    private bool movementDisabled = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        visual = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        GameEventsManager.Instance.inputEvents.onMovePressed += MovePressed;
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
        velocity = moveDir.normalized * moveSpeed;

        if (movementDisabled)
        {
            velocity = Vector2.zero;
        }
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
