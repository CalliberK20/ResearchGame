using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponStats))]
public class WeaponStatsEditor : Editor
{
    SerializedProperty isMelee;

    SerializedProperty damage;

    SerializedProperty delayShot;
    SerializedProperty bulletSpeed;
    SerializedProperty bulletDestroyTime;

    bool rangeWeapon = false;

    private void OnEnable()
    {
        isMelee = serializedObject.FindProperty("isMelee");

        damage = serializedObject.FindProperty("damage");

        delayShot = serializedObject.FindProperty("delayShot");
        bulletSpeed = serializedObject.FindProperty("bulletSpeed");
        bulletDestroyTime = serializedObject.FindProperty("bulletDestroyTime");
    }

    public override void OnInspectorGUI()
    {
        WeaponStats _weaponStats = (WeaponStats)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(isMelee);

        EditorGUILayout.PropertyField(damage);

        EditorGUILayout.Space(10);

        rangeWeapon = EditorGUILayout.BeginFoldoutHeaderGroup(rangeWeapon, "Range Weapon Stats:");

        if (_weaponStats.isMelee)
        {
            GUI.enabled = false;
        }

        if (rangeWeapon)
        {
            EditorGUILayout.PropertyField(delayShot);
            EditorGUILayout.PropertyField(bulletSpeed);
            EditorGUILayout.PropertyField(bulletDestroyTime);
        }
        GUI.enabled = true;

        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
