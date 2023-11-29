using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class JournalManager : MonoBehaviour
{
    public static JournalManager instance;

    public Image spriteIcon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    [Space]
    public Image[] sets;
    [Space]
    public WeaponStats[] weaponStats;
    public EnemyStats[] enemyStats;
    [HideInInspector]
    public int switchNum = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetSwitch();
        spriteIcon.sprite = null;
        nameText.text = "";
        descriptionText.text = "";
    }

    public void SwitchContent()
    {
        if(switchNum == 0)
        {
            switchNum = 1;
        }
        else if(switchNum == 1)
        {
            switchNum = 0;
        }
        SetSwitch();
    }

    void SetSwitch()
    {
        SetDeactivateAll();
        switch(switchNum)
        {
            case 0:
                for (int i = 0; i < weaponStats.Length; i++)
                {
                    sets[i].gameObject.SetActive(true);
                    sets[i].sprite = weaponStats[i].weaponSprite;
                    sets[i].GetComponent<InfoDetail>().SetStats(weaponStats[i]);
                }
                break;
            case 1:
                for (int i = 0; i < enemyStats.Length; i++)
                {
                    sets[i].gameObject.SetActive(true);
                    sets[i].sprite = enemyStats[i].zombieSprite;
                    sets[i].GetComponent<InfoDetail>().SetStats(enemyStats[i]);
                }
                break;
        }
        spriteIcon.sprite = null;
        nameText.text = "";
        descriptionText.text = "";
    }

    void SetDeactivateAll()
    {
        foreach(Image imageSet in sets)
        {
            imageSet.gameObject.SetActive(false);
        }
    }

    public void ShowInfo(Sprite icon, string objName, string objDescription)
    {
        spriteIcon.sprite = icon;
        nameText.text = objName;
        descriptionText.text = objDescription;
    }
}
