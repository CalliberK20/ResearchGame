using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponStats))]
public class WeaponStatsEditor : Editor
{
    SerializedProperty weaponSprite;
    SerializedProperty weaponAudioClip;
    SerializedProperty weaponPrice;

    SerializedProperty weaponAnimatorType;

    SerializedProperty isMelee;
    SerializedProperty holdAttack;

    SerializedProperty damage;

    SerializedProperty ammo;
    SerializedProperty atkRate;
    SerializedProperty bulletSpeed;
    SerializedProperty bulletDestroyTime;

    bool rangeWeapon = false;

    private void OnEnable()
    {
        weaponSprite = serializedObject.FindProperty("weaponSprite");
        weaponAudioClip = serializedObject.FindProperty("weaponAudioClip");
        weaponPrice = serializedObject.FindProperty("weaponPrice");

        weaponAnimatorType = serializedObject.FindProperty("weaponAnimatorType");

        isMelee = serializedObject.FindProperty("isMelee");
        holdAttack = serializedObject.FindProperty("holdAttack");

        damage = serializedObject.FindProperty("damage");

        ammo = serializedObject.FindProperty("ammo");
        atkRate = serializedObject.FindProperty("atkRate");
        bulletSpeed = serializedObject.FindProperty("bulletSpeed");
        bulletDestroyTime = serializedObject.FindProperty("bulletDestroyTime");
    }

    public override void OnInspectorGUI()
    {
        WeaponStats _weaponStats = (WeaponStats)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(weaponSprite);
        EditorGUILayout.PropertyField(weaponAudioClip);
        EditorGUILayout.PropertyField(weaponPrice);
        EditorGUILayout.Space(5f);
        EditorGUILayout.PropertyField(weaponAnimatorType);
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(isMelee);
        EditorGUILayout.PropertyField(holdAttack);
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(damage);
        EditorGUILayout.PropertyField(atkRate);

        EditorGUILayout.Space(10);

        rangeWeapon = EditorGUILayout.BeginFoldoutHeaderGroup(rangeWeapon, "Range Weapon Stats:");

        if (_weaponStats.isMelee)
        {
            GUI.enabled = false;
        }

        if (rangeWeapon)
        {
            EditorGUILayout.PropertyField(ammo);
            EditorGUILayout.PropertyField(bulletSpeed);
            EditorGUILayout.PropertyField(bulletDestroyTime);
        }
        GUI.enabled = true;

        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
