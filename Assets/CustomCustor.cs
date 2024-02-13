using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomCustor : MonoBehaviour
{
    public static CustomCustor instance;

    public Sprite cursorSprite;
    public GameObject cursorObj;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetActive(true);
    }

    private void Update()
    {
        if (!Cursor.visible && UIManager.Instance.cursor.gameObject.activeInHierarchy)
        {
            UIManager.Instance.cursor.transform.position = Input.mousePosition;
        }
    }

    public void SetActive(bool active)
    {
        UIManager.Instance.cursor.gameObject.SetActive(active);
        Cursor.visible = !active;
        UIManager.Instance.cursor.sprite = cursorSprite;
    }
}
