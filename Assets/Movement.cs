using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 2;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (move.magnitude > 0.1f)
            Flip(move.x);

        rigid.position += move * speed * Time.fixedDeltaTime;
    }

    void Flip(float flipMove)
    {
        if (flipMove < 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }
}
