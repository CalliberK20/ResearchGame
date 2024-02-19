using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    public Sprite weaponSprite;
    public string weaponAudioName;
    public string weaponName;
    public string weaponDescrip;

    public float weaponPrice = 20;

    public int weaponAnimatorType = 0;

    public bool isMelee = false;
    public bool holdAttack;
    public float atkRate = 1f; 

    public float damage = 2;

    public int ammo = 5;
    /*  DIDN'T ADD THIS IN THE EDITOR =========>  */ public float delayShot = 1f;
    public float bulletSpeed = 4f;
    public float reloadSpeed = 3f;
    public float bulletDestroyTime = 2f;
}
