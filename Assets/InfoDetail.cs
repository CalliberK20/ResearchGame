using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoDetail : MonoBehaviour
{
    [ShowOnly]
    public WeaponStats weaponStats;
    [ShowOnly]
    public EnemyStats enemyStats;

    public void ClickInfo()
    {
        JournalManager journalManager = JournalManager.instance;

        if (journalManager.switchNum == 0)
            journalManager.ShowInfo(weaponStats.weaponSprite, weaponStats.weaponName, weaponStats.weaponDescrip);
        else if(journalManager.switchNum == 1)
            journalManager.ShowInfo(enemyStats.zombieSprite, enemyStats.zombieName, enemyStats.zombieDescrip);
    }


    public void SetStats(EnemyStats enemyStats)
    {
        this.enemyStats = enemyStats;
    }

    public void SetStats(WeaponStats weaponStats)
    {
        this.weaponStats = weaponStats;
    }
}
