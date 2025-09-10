using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(BoxCollider2D))]
public class MonsterAttack : MonoBehaviour
{
    [Header("공격 판정 설정")]
    [SerializeField] private Transform attackBone;     // Rig Weapon 본 (Collider 붙어있는 Transform)
    [SerializeField] private BoxCollider2D boxCollider; // 실제 공격용 Collider
    [SerializeField] private LayerMask targetLayer;     // Player 레이어
    [SerializeField] private GameObject animatorObject; // flip 여부 확인할 애니메이터 오브젝트
    

    public bool Attack()
    {
        if (attackBone == null || boxCollider == null)
            return false;

        // Collider 기준 월드 좌표 변환
        Vector2 center = (Vector2)attackBone.TransformPoint(boxCollider.offset);
        Vector2 size   = Vector2.Scale(boxCollider.size, attackBone.lossyScale);
        float angle    = attackBone.eulerAngles.z;

        //애니메이터 오브젝트의 flip 상태에 따라 angle 보정
        if (animatorObject != null && animatorObject.transform.localScale.z < 0f)
        {
            angle = -angle;
        }

        // 박스 오버랩 체크
        Collider2D hit = Physics2D.OverlapBox(center, size, angle, targetLayer);
        return hit != null;
    }

    // Scene 뷰에서 공격 범위를 시각화
    private void OnDrawGizmos()
    {
        if (attackBone == null || boxCollider == null) return;

        Gizmos.color = Color.red;

        Vector2 center = (Vector2)attackBone.TransformPoint(boxCollider.offset);
        Vector2 size   = Vector2.Scale(boxCollider.size, attackBone.lossyScale);
        float angle    = attackBone.eulerAngles.z;

        if (animatorObject != null && animatorObject.transform.localScale.z < 0f)
        {
            angle = -angle;
        }

        // 회전 적용된 박스 그리기
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}