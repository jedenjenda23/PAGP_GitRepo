using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_PlayerUI : MonoBehaviour
{
    [SerializeField]
    public static UI_PlayerUI instance;

    public static Text UI_PlayerHp;
    public static bool inGameMenuToggle;
    public GameObject inGameMenu;

    public GameObject playerHotbar;
    public GameObject itemSlotPreset;
    public GameObject itemPreset;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else Destroy(gameObject);

        Time.timeScale = 1;
        UI_PlayerHp = GameObject.Find("Text_PlayerHP").GetComponent<Text>();
    }


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInGameMenu();
        }
    }

    public void ToggleInGameMenu()
    {
        inGameMenuToggle = !inGameMenuToggle;
        inGameMenu.SetActive(inGameMenuToggle);

        if (inGameMenuToggle)
        {
            Time.timeScale = 0;
        }
        else Time.timeScale = 1;
    }
}
