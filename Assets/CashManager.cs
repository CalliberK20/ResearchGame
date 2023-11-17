using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public static CashManager instance;
    private TextMeshProUGUI cashAmount;

    public float cash = 100;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cashAmount = UIManager.Instance.cashAmount;
        ShowAmount();
    }

    public void GiveMoney(float amount)
    {
        cash += amount;
        ShowAmount();
    }

    public void LoseMoney(float amount)
    {
        cash -= amount;
        ShowAmount();
    }

    public bool SuffiecientAmount(float amount)
    {
        if (cash >= amount)
            return true;
        Debug.Log("Un-suffiecient amount");
        return false;
    }

    private void ShowAmount()
    {
        cashAmount.text = cash.ToString("0");
    }
}
