using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Door parentDoor;
    [Space]
    public float radius = 1.5f;
    public float entryPass = 30f;
    [Space]
    public TextMeshPro textPrice;
    public GameObject doorSpriteObj;
    public GameObject barrierCollider;
    [Space]
    public LayerMask layersToOpen;
    [HideInInspector]
    public List<Collider2D> collider = new List<Collider2D>();
    private Collider2D player;
    private bool isBought = false;

    private void Start()
    {
        isBought = false;
        if (parentDoor != null)
        {
            textPrice.gameObject.SetActive(false);
            barrierCollider.SetActive(false);
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        collider = Physics2D.OverlapCircleAll(transform.position, radius, layersToOpen).ToList();

        if (collider.Count > 0)
        {
            if(parentDoor != null)
            {
                doorSpriteObj.SetActive(false);
            }
            else if (!collider.Contains(player) || isBought)
                doorSpriteObj.SetActive(false);
            else if (!isBought)
            {
                textPrice.gameObject.SetActive(true);
                textPrice.text = "Unlock: " + entryPass;
            }
        }
        else
        {
            textPrice.gameObject.SetActive(false);
            doorSpriteObj.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (!isBought && CashManager.instance.cash >= entryPass)
            {
                isBought = true;
                textPrice.gameObject.SetActive(false);
                barrierCollider.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
