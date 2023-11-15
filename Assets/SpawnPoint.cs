using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Vector2 spawnPos;

    public Vector3 Spawn()
    {
        return transform.position + new Vector3(spawnPos.x, spawnPos.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(spawnPos.x, spawnPos.y), 0.5f);
    }
}
