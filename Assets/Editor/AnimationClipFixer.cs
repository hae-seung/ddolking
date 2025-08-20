using UnityEditor;
using UnityEngine;

public class AnimationClipFixer : MonoBehaviour
{
    [MenuItem("Tools/Animation/Fix Rig Weapon Scale")]
    static void FixWeaponScale()
    {
        // Project 뷰에서 AnimationClip 선택해야 함
        AnimationClip clip = Selection.activeObject as AnimationClip;
        if (clip == null)
        {
            Debug.LogError("AnimationClip을 선택하세요!");
            return;
        }

        // 원하는 값 입력
        float targetX = 1.7f;
        float targetY = 1f;
        float targetZ = 2.5f;

        var bindings = AnimationUtility.GetCurveBindings(clip);
        foreach (var binding in bindings)
        {
            // Rig Weapon Scale만 찾기
            if (binding.path.Contains("Rig Weapon") && binding.propertyName.Contains("m_LocalScale"))
            {
                var curve = AnimationUtility.GetEditorCurve(clip, binding);

                for (int i = 0; i < curve.keys.Length; i++)
                {
                    Keyframe kf = curve.keys[i];

                    if (binding.propertyName.EndsWith(".x"))
                        kf.value = targetX;
                    else if (binding.propertyName.EndsWith(".y"))
                        kf.value = targetY;
                    else if (binding.propertyName.EndsWith(".z"))
                        kf.value = targetZ;

                    curve.MoveKey(i, kf);
                }

                AnimationUtility.SetEditorCurve(clip, binding, curve);
                Debug.Log($"{binding.propertyName} → 모두 수정 완료");
            }
        }

        Debug.Log($"클립 '{clip.name}'의 Rig Weapon Scale 키프레임이 전부 수정되었습니다.");
    }
}