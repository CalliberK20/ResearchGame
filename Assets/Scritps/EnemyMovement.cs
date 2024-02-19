using System.Collections;
using System.Collections.Generic;
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
    private bool canLatch = false;
    private GridCreateManager gridCreate;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private bool isAttacking = false;
    private float reward;
    private float ranX;
    private float ranY;

    private AudioSource audioSource;
    private bool nowLatching = false;
    private Movement targetMovement;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetEnemyStats(EnemyStats newStats)
    {
        nowLatching = false;
        canLatch = newStats.canLatch;
        speed = newStats.speed;
        health = newStats.health;
        atkDamage = newStats.atkDamage;
        atkSpeed = newStats.atkSpeed;
        reward = newStats.reward;
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = newStats.enemyAnimatorController;
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetMovement = target.GetComponent<Movement>();
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridCreate = FindObjectOfType<GridCreateManager>();
        path = new List<NodeGrid>();
        Sound sound = AudioManager.instance.GetAudio("Zombie");
        audioSource.clip = sound.clip;
        audioSource.volume = sound.volume;
        audioSource.loop = sound.loop;
        audioSource.outputAudioMixerGroup = sound.mixer;
        audioSource.Play();
    }

    private void FixedUpdate()
    {
        if (nowLatching)
        {
            transform.position = target.position;
            if (!isAttacking)
                StartCoroutine(Attack());
            return;
        }

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
                Vector3 movePos = new Vector2(path[0].GridX + ranX, path[0].GridY + ranY);

                if (transform.position == movePos)
                {
                    path = new List<NodeGrid>();
                    Flip();
                    anim.SetBool("Run", false);
                    ranX = Random.Range(0f, 1f);
                    ranY = Random.Range(0f, 1f);
                }
                else
                {

                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(movePos.x, movePos.y), speed * Time.deltaTime);
                    anim.SetBool("Run", true);
                    if (!FollowCamera.OnView(transform))
                    {
                        ReSpawnOnPoint();
                        path.Clear();
                    }
                }
            }
            else
            {
                path = gridCreate.FindTarget(transform.position, target.position);
            }
        }
    }

    void ReSpawnOnPoint()
    {
        EnemySpawner spawner = EnemySpawner.Instance;
        int ran = Random.Range(0, spawner.enemyEntry.Count);
        transform.position = spawner.enemyEntry[ran].GetComponent<SpawnPoint>().Spawn();
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
        nowLatching = canLatch;
        anim.SetTrigger("Attack");
        targetMovement.Damage(atkDamage);
        if (canLatch)
            StartCoroutine(targetMovement.GiveSlowness(0.7f));
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
            WaveManager.instance.numOfZombiesInGame--;
            audioSource.Stop();
            StartCoroutine(DesipateDelay());
            Sound sound = AudioManager.instance.GetAudio("ZombieDead");
            audioSource.volume = sound.volume;
            audioSource.loop = sound.loop;
            audioSource.Play();
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

