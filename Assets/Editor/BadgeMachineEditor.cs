using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BadgeMachine))]
public class BadgeMachineEditor : Editor
{
    SerializedProperty script;
    SerializedProperty badgeSprites;
    SerializedProperty badgeType;

    SerializedProperty canBeInteracted;

    SerializedProperty healthToGive;
    SerializedProperty speedToGive;
    SerializedProperty reloadSpdToGive;
    SerializedProperty time;

    SerializedProperty refillTime;

    private void OnEnable()
    {
        script = serializedObject.FindProperty("script");
        badgeSprites = serializedObject.FindProperty("badgeSprites");
        badgeType = serializedObject.FindProperty("badgeType");

        canBeInteracted = serializedObject.FindProperty("canBeInteracted");

        healthToGive = serializedObject.FindProperty("healthToGive");
        speedToGive = serializedObject.FindProperty("speedToGive");
        reloadSpdToGive = serializedObject.FindProperty("reloadSpdToGive");
        time = serializedObject.FindProperty("time");

        refillTime = serializedObject.FindProperty("refillTime");
    }

    public override void OnInspectorGUI()
    {
        BadgeMachine _badgeMachine = (BadgeMachine)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(badgeSprites);
        EditorGUILayout.PropertyField(badgeType);
        EditorGUILayout.PropertyField(canBeInteracted);

        if (_badgeMachine.badgeType == BadgeType.green)
            EditorGUILayout.PropertyField(healthToGive);
        else if (_badgeMachine.badgeType == BadgeType.blue)
            EditorGUILayout.PropertyField(speedToGive);
        else if(_badgeMachine.badgeType == BadgeType.yellow)
            EditorGUILayout.PropertyField(reloadSpdToGive);

        if(_badgeMachine.badgeType == BadgeType.blue || _badgeMachine.badgeType == BadgeType.yellow)
            EditorGUILayout.PropertyField(time);

        EditorGUILayout.PropertyField(refillTime);


        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
