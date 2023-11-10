using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType")]
public class EnemyStats : ScriptableObject
{
    public RuntimeAnimatorController enemyAnimatorController;
    public float speed = 2;
}
