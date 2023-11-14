using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float health = 50;
    public float speed = 2;
    [Space]
    public Animator legAnim;
    [ShowOnly] public bool isDead = false;

    //--------------PRIVATE VARIABLE----------------------------
    private Rigidbody2D rigid;
    private SpriteRenderer[] spriteRenderers;
    private GunManager gunManager;
    [HideInInspector] public bool isFlip;

    private float regHealth;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        gunManager = GetComponent<GunManager>();

        int count = transform.childCount;
        spriteRenderers = new SpriteRenderer[count];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
        }

        regHealth = health;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isDead)
        {
            Vector2 move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (move.magnitude > 0.1f)
            {
                legAnim.SetBool("Run", true);
            }
            else
                legAnim.SetBool("Run", false);

            rigid.position += move * speed * Time.fixedDeltaTime;

            if (Input.GetButton("Horizontal"))
                Flip(move.x);
        }    
    }

    public void Damage(float damage)
    {
        health -= damage;

        Image healthBar = UIManager.Instance.GetHealthBar();
        healthBar.fillAmount -= damage / regHealth;

        if(!isDead && health <= 0)
        {
            legAnim.SetTrigger("Die");
            gunManager.enabled = false;
            gunManager.torsoAnim.gameObject.SetActive(false);
            isDead = true;
        }
    }

    void Flip(float flipMove)
    {
        isFlip = false;

        if (flipMove < 0)
            isFlip = true;
        else
            isFlip = false;

        foreach (SpriteRenderer spriteRender in spriteRenderers)
        {
            spriteRender.flipX = isFlip;
        }
    }
}
