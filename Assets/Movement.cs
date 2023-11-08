using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 2;
    [Space]
    public Animator legAnim;


    //--------------PRIVATE VARIABLE----------------------------
    private Rigidbody2D rigid;
    private SpriteRenderer[] spriteRenderers;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        int count = transform.childCount;
        spriteRenderers = new SpriteRenderer[count];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (move.magnitude > 0.1f)
        {
            legAnim.SetBool("Run", true);
            Flip(move.x);
        }
        else
            legAnim.SetBool("Run", false);

        rigid.position += move * speed * Time.fixedDeltaTime;
    }

    void Flip(float flipMove)
    {
        bool isFlip = false;

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
