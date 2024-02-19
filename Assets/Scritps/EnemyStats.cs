using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType")]
public class EnemyStats : ScriptableObject
{
    public EnemyStats()
    {

    }

    public EnemyStats(RuntimeAnimatorController enemyAnimatorController, Sprite zombieSprite, float health, float speed, float reward, float atkDamage, float atkSpeed, bool canLatch)
    {
        this.enemyAnimatorController = enemyAnimatorController;
        this.zombieSprite = zombieSprite;
        this.health = health;
        this.speed = speed;
        this.reward = reward;
        this.atkDamage = atkDamage;
        this.atkSpeed = atkSpeed;
        this.canLatch = canLatch;
    }


    public RuntimeAnimatorController enemyAnimatorController;
    public Sprite zombieSprite;
    public string zombieName;
    [TextArea(2, 10)]
    public string zombieDescrip;
    [Space]
    public float health = 4f;
    public float speed = 2;
    [Space]
    public float reward = 20;
    [Space]
    public float atkDamage = 2;
    public float atkSpeed = 2f;
    [Space]
    public bool canLatch = false;
}
