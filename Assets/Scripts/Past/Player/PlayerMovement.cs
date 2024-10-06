using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rigid;
    public Vector2 inputVec;
    public float speed;
    private void Awake()
    {
        Application.targetFrameRate = 75;
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
       
    }

    private void FixedUpdate()
    {
        Vector2 nextPos = rigid.position + inputVec * speed * Time.deltaTime;
        rigid.MovePosition(nextPos);
    }

    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}
