using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BadgeMachine))]
public class BadgeMachineEditor : Editor
{
    SerializedProperty script;
    SerializedProperty badgeSprites;
    SerializedProperty pointsPrice;
    SerializedProperty badgeType;

    SerializedProperty canBeInteracted;

    SerializedProperty healthToGive;
    SerializedProperty speedToGive;
    SerializedProperty deductiveReloadSpeed;
    SerializedProperty time;

    SerializedProperty refillTime;

    private void OnEnable()
    {
        script = serializedObject.FindProperty("script");
        pointsPrice = serializedObject.FindProperty("pointsPrice");
        badgeSprites = serializedObject.FindProperty("badgeSprites");
        badgeType = serializedObject.FindProperty("badgeType");

        canBeInteracted = serializedObject.FindProperty("canBeInteracted");

        healthToGive = serializedObject.FindProperty("healthToGive");
        speedToGive = serializedObject.FindProperty("speedToGive");
        deductiveReloadSpeed = serializedObject.FindProperty("deductiveReloadSpeed");
        time = serializedObject.FindProperty("time");

        refillTime = serializedObject.FindProperty("refillTime");
    }

    public override void OnInspectorGUI()
    {
        BadgeMachine _badgeMachine = (BadgeMachine)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(badgeSprites);
        EditorGUILayout.PropertyField(badgeType);
        EditorGUILayout.PropertyField(pointsPrice);
        EditorGUILayout.PropertyField(canBeInteracted);

        if (_badgeMachine.badgeType == BadgeType.green)
            EditorGUILayout.PropertyField(healthToGive);
        else if (_badgeMachine.badgeType == BadgeType.blue)
            EditorGUILayout.PropertyField(speedToGive);
        else if(_badgeMachine.badgeType == BadgeType.yellow)
            EditorGUILayout.PropertyField(deductiveReloadSpeed);

        if(_badgeMachine.badgeType == BadgeType.blue || _badgeMachine.badgeType == BadgeType.yellow)
            EditorGUILayout.PropertyField(time);

        EditorGUILayout.PropertyField(refillTime);


        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
