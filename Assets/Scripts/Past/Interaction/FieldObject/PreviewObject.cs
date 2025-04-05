using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PreviewObject : MonoBehaviour
{
    public bool CanEstablish { get; private set; }

    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;
    private readonly Collider2D[] _colliders = new Collider2D[5];
    private LayerMask _ignoreLayerMask;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _ignoreLayerMask = LayerMask.GetMask("Land", "Ignore Raycast"); // 무시할 레이어들
    }

    private void Update()
    {
        CheckForCollisions();
    }

    private void CheckForCollisions()
    {
        Vector2 boxCenter = (Vector2)transform.position + _boxCollider2D.offset;
        Vector2 boxSize = _boxCollider2D.size;

        int numCollisions = Physics2D.OverlapBoxNonAlloc( // BoxCollider2D 범위 기반 감지
            boxCenter, 
            boxSize, 
            0f, // 회전 없음 (필요 시 z축 회전값 입력)
            _colliders,
            ~_ignoreLayerMask // Land와 Ignore Raycast는 감지하지 않음
        );

        if (numCollisions == 0)
        {
            CanEstablish = true;
            _spriteRenderer.color = Color.white;
        }
        else
        {
            CanEstablish = false;
            _spriteRenderer.color = Color.red;
        }
    }
}