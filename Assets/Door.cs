using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float radius = 1.5f;
    public LayerMask layersToOpen;
    public Collider2D[] collider;
    private GameObject doorSpriteObj;
    // Start is called before the first frame update
    void Start()
    {
        doorSpriteObj = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        collider = Physics2D.OverlapCircleAll(transform.position, radius, layersToOpen);

        if(collider.Length > 0)
            doorSpriteObj.SetActive(false);
        else doorSpriteObj.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
