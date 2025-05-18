using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PreviewObject : MonoBehaviour
{
    public bool CanEstablish { get; private set; }

    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;
    private readonly Collider2D[] _colliders = new Collider2D[10];
    private ContactFilter2D _contactFilter2D;

    [Header("Land는 무조건 체크 / 설치할 곳 레이어만 체크")]
    [Tooltip("Land는 항상 감지되기때문")]
    [SerializeField] private LayerMask _blockedLayerMask;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        
        //트리거는 무시, 콜라이더만 감지
        _contactFilter2D = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = ~_blockedLayerMask,
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

        
        //충돌된 콜리전이 있다면 설치불가
        for (int i = 0; i < numCollisions; i++)
        {
            GameObject hitObject = _colliders[i].gameObject;

            // 자기 자신 무시
            if (hitObject == gameObject)
                continue;
            CanEstablish = false;
            break;
        }
    }

    public void SetPlacementValidity(bool isValid)
    {
        _spriteRenderer.color = isValid ? Color.white : Color.red;
    }

}