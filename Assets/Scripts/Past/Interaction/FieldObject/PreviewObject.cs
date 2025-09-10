using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PreviewObject : MonoBehaviour
{
    public bool CanEstablish { get; private set; }

    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;
    private readonly Collider2D[] _colliders = new Collider2D[10];
    private ContactFilter2D _contactFilter2D;
    
    
    
    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        
        //트리거는 무시, 콜리젼만 감지
        _contactFilter2D = new ContactFilter2D
        {
            useLayerMask = true,
            useTriggers = false
        };
    }

    private void Update()
    {
        CheckForCollisions();
    }

    private void CheckForCollisions()
    {
        Vector2 boxCenter = (Vector2)transform.position + _boxCollider2D.offset;
        Vector2 boxSize = _boxCollider2D.size;

        int numCollisions = Physics2D.OverlapBox(
            boxCenter,
            boxSize,
            0f,
            _contactFilter2D,
            _colliders
        );

        CanEstablish = true;

        
        
        if (numCollisions > 0)
            CanEstablish = false;
        
        Debug.Log(CanEstablish);
    }

    public void SetPlacementValidity(bool isValid)
    {
        _spriteRenderer.color = isValid ? Color.white : Color.red;
    }

}