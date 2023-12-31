using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Image healthBar;
    [Space]
    public TextMeshProUGUI cashAmount;
    [Space]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI waveText;

    private void Awake()
    {
        Instance = this;
    }
}
