using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class DropObject : Interactable
{
    protected IObjectPool<DropObject> _ManagedPool;
    protected Collider2D _collider; // 충돌 감지를 위한 Collider2D
    [SerializeField] protected float disableCollisionTime = 0.5f; // 충돌 비활성화 시간

    protected void Awake()
    {
        _collider = GetComponent<Collider2D>(); // Collider2D 컴포넌트 가져오기
    }

    // 풀을 설정하는 메서드
    public void SetManagedPool(IObjectPool<DropObject> pool)
    {
        _ManagedPool = pool;
    }

    // 풀로 반환하는 메서드
    public void DestroyDropObject()
    {
        _ManagedPool.Release(this);
    }

    // 드랍되었을 때 콜라이더를 일정 시간 비활성화
    protected void OnEnable()
    {
        StartCoroutine(DisableCollisionTemporarily());
    }

    // 일정 시간 동안 충돌을 비활성화하는 코루틴
    protected IEnumerator DisableCollisionTemporarily()
    {
        _collider.enabled = false; // 콜라이더 비활성화

        // 지정된 시간 동안 대기
        yield return new WaitForSeconds(disableCollisionTime);

        // 콜라이더 활성화
        _collider.enabled = true;
    }

    // 플레이어와 충돌 시 호출되는 메서드
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //인벤토리 저장구현 필요
            DestroyDropObject(); // 풀로 반환
            
        }
    }
}