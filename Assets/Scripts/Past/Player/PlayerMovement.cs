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
}
