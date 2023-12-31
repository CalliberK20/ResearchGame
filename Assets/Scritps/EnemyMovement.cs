using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    [ShowOnly] public float health = 4f;
    [ShowOnly] public float speed = 4;
    [ShowOnly] public float atkDamage = 0;
    [ShowOnly] public float atkSpeed = 0;
    public List<NodeGrid> path = new List<NodeGrid>();
    [Space]

    //-------------PRIVATE--------------------------------------
    private GridCreateManager gridCreate;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private bool isAttacking = false;
    private float reward;

    public void SetEnemyStats(EnemyStats newStats)
    {
        speed = newStats.speed;
        health = newStats.health;
        atkDamage = newStats.atkDamage;
        atkSpeed = newStats.atkSpeed;
        reward = newStats.reward;
        anim = transform.GetChild(0).GetComponent<Animator>();
        anim.runtimeAnimatorController = newStats.enemyAnimatorController;
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        gridCreate = FindObjectOfType<GridCreateManager>();
        path = new List<NodeGrid>();
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, target.position) <= 0.7f)
        {
            Flip();
            anim.SetBool("Run", false);
            if (!isAttacking)
                StartCoroutine(Attack());
        }
        else
        {
            if (path.Count > 0)
            {
                NodeGrid pathGrid = path[0];
                float ranX = Random.Range(0f, 1f);
                float ranY = Random.Range(0f, 1f);
                Vector2 movePos = new Vector2(path[0].GridX + ranX, path[0].GridY + ranY);

                /*                if (Vector2.Distance(transform.position, new Vector2(movePos.x, movePos.y)) > 0.5f)
                                {
                                    transform.position = Vector2.MoveTowards(transform.position, movePos, speed * Time.deltaTime);
                                    anim.SetBool("Run", true);
                                }
                                else if (Vector2.Distance(transform.position, new Vector2(movePos.x, movePos.y)) < 0.5f)
                                {
                                    path = new List<NodeGrid>();
                                    Flip();
                                    anim.SetBool("Run", false);
                                }*/

                if (transform.position.x == pathGrid.GridX + 0.5f && transform.position.y == pathGrid.GridY + 0.5f)
                {
                    path = new List<NodeGrid>();
                    Flip();
                    anim.SetBool("Run", false);
                }
                else
                {

                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(pathGrid.GridX + 0.5f, pathGrid.GridY + 0.5f), speed * Time.deltaTime);
                    anim.SetBool("Run", true);
                }
            }
            else
            {
                path = gridCreate.FindTarget(transform.position, target.position);
            }
        }
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

    private IEnumerator Attack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        target.GetComponent<Movement>().Damage(atkDamage);
        yield return new WaitForSeconds(atkSpeed);
        isAttacking = false;
    }

    //--------------------------------MANAGING HEALTH-----------------------------

    public void Damage(float dmg)
    {
        health -= dmg;

        if(health <= 0)
        {
            anim.SetTrigger("Die");
            enabled = false;
            CashManager.instance.GiveMoney(reward);
            StartCoroutine(DesipateDelay());
        }
    }

    //---------------------------------------------------------------------------


    private IEnumerator DesipateDelay()
    {
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if(GridCreateManager.instance.showGizmos)
        {
            foreach (NodeGrid n in path)
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawCube(new Vector3(n.GridX + 0.5f, n.GridY + 0.5f), new Vector3(0.5f, 0.5f));
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.7f);
        }
    }
}

