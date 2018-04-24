using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_PlayerUI : MonoBehaviour
{
    [SerializeField]
    public static UI_PlayerUI instance;

    public static bool inGameMenuToggle;
    public GameObject inGameMenu;
    public Text playerHp;
    public RectTransform playerHealthbar;
    Vector2 initBarScale;


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

        initBarScale = playerHealthbar.localScale;

        Time.timeScale = 1;
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

    public void UpdateHealthbar(float hp, float maxHP)
    {
        float percHp = (hp / maxHP);
        percHp = Mathf.Clamp(percHp, 0, 100);

        Vector2 newScale = initBarScale * percHp;
        newScale.y = initBarScale.x;

        playerHealthbar.localScale = newScale; 
    }
}