using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 4;
    public List<NodeGrid> path = new List<NodeGrid>();
    [Space]
    private GridCreateManager gridCreate;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        gridCreate = FindObjectOfType<GridCreateManager>();
        gridCreate.FindTarget(transform.position, target.position, this);

        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        NodeGrid pathGrid = path[0];

        if (path.Count > 1)
        {
            if (transform.position.x == pathGrid.GridX + 0.5f && transform.position.y == pathGrid.GridY + 0.5f)
            {
                gridCreate.FindTarget(transform.position, target.position, this);
                Flip();
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(path[0].GridX + 0.5f, path[0].GridY + 0.5f), speed * Time.deltaTime);
            }
        }
        else
            gridCreate.FindTarget(transform.position, target.position, this);
    }

    private void Flip()
    {
        bool flip = false;

        if (target.position.x > transform.position.x)
            flip = true;
        else if (target.position.x < transform.position.x)
            flip = false;

        spriteRenderer.flipX = flip;
    }

    private void OnDrawGizmos()
    {
        foreach(NodeGrid n in path)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawCube(new Vector3(n.GridX + 0.5f, n.GridY + 0.5f), new Vector3(0.5f, 0.5f));
        }
    }
}

