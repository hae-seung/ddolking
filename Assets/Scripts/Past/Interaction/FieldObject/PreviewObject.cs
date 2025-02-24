using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PreviewObject : MonoBehaviour
{
    public bool CanEstablish { get; private set; }

    private CircleCollider2D _circleCollider2D;
    private SpriteRenderer spriteRenderer;
    private readonly Collider2D[] colliders = new Collider2D[5];
    private LayerMask ignoreLayerMask;

    private void Awake()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ignoreLayerMask = LayerMask.GetMask("Land", "Ignore Raycast"); //무시할 레이어들
    }

    private void Update()
    {
        CheckForCollisions();
    }

    private void CheckForCollisions()
    {
        int numCollisions = Physics2D.OverlapCircleNonAlloc(//Overlap은 ignoreRaycast레이어도 감지해서 또 설정해줌.
            (Vector2)transform.position + _circleCollider2D.offset,
            _circleCollider2D.radius,
            colliders,
            ~ignoreLayerMask //Land와 Ignore Raycast는 감지하지 않음
        );

        if (numCollisions == 0)
        {
            CanEstablish = true;
            spriteRenderer.color = Color.white;
        }
        else
        {
            CanEstablish = false;
            spriteRenderer.color = Color.red;
        }
    }


}