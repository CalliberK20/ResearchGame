using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    public Sprite weaponSprite;
    public bool holdAttack;
    public bool isMelee = false;

    public float damage = 2;

    public int ammo = 5;
    public float delayShot = 1f;
    public float bulletSpeed = 4f;
    public float bulletDestroyTime = 2f;
}
