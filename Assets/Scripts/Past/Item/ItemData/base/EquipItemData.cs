using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ItemClass
{
    Normal = 3,
    Epic = 5,
    Unique = 7,
    Legend = 10
}

public abstract class EquipItemData : ItemData
{
    public bool isEnhanceable = false;
    
    public ItemClass itemclass;
    public float maxDurability;
    public int itemLevel;
    [SerializeField] private List<StatModifier> statModifier; //상승시킬 플레이어 능력
    [SerializeField] private ItemEnhancementLogic enhancementLogic;//StatModifier에 없는 능력도 강화로 생기게 가능
    
    public List<StatModifier> GetStatModifier()
    {
        return statModifier.Count == 0 ? null : statModifier;
    }

    public ItemEnhancementLogic GetLogic()
    {
        return enhancementLogic;
    }
    
    
    //만약 강화로 StatModifier에 없는 능력이 새로 생긴다면 객체에서 Add해줘야함.
}


#if UNITY_EDITOR
[CustomEditor(typeof(EquipItemData), true)] // EquipItemData를 위한 커스텀 에디터
public class EquipItemDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty isEnhanceableProp = serializedObject.FindProperty("isEnhanceable");
        SerializedProperty enhancementLogicProp = serializedObject.FindProperty("enhancementLogic");

        // 🔹 enhancementLogic을 제외한 모든 필드 자동 표시 (부모 필드 포함)
        DrawPropertiesExcluding(serializedObject, "enhancementLogic");

        // 🔹 isEnhanceable이 true일 때만 enhancementLogic 표시
        if (isEnhanceableProp.boolValue)
        {
            EditorGUILayout.PropertyField(enhancementLogicProp);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif

