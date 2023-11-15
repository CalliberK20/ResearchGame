using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponStats))]
public class WeaponStatsEditor : Editor
{
    SerializedProperty weaponSprite;

    SerializedProperty isMelee;

    SerializedProperty damage;

    SerializedProperty ammo;
    SerializedProperty atkDelay;
    SerializedProperty bulletSpeed;
    SerializedProperty bulletDestroyTime;

    bool rangeWeapon = false;

    private void OnEnable()
    {
        weaponSprite = serializedObject.FindProperty("weaponSprite");

        isMelee = serializedObject.FindProperty("isMelee");

        damage = serializedObject.FindProperty("damage");

        ammo = serializedObject.FindProperty("ammo");
        atkDelay = serializedObject.FindProperty("atkDelay");
        bulletSpeed = serializedObject.FindProperty("bulletSpeed");
        bulletDestroyTime = serializedObject.FindProperty("bulletDestroyTime");
    }

    public override void OnInspectorGUI()
    {
        WeaponStats _weaponStats = (WeaponStats)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(weaponSprite);

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
            EditorGUILayout.PropertyField(ammo);
            EditorGUILayout.PropertyField(atkDelay);
            EditorGUILayout.PropertyField(bulletSpeed);
            EditorGUILayout.PropertyField(bulletDestroyTime);
        }
        GUI.enabled = true;

        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
