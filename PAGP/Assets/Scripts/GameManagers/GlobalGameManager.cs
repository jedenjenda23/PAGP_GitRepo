using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour {

    public static GlobalGameManager instance;
    public MissionGameManager curentMissionManager;

    public int totalDays;
    public GameObject[] initialShelterCharacters;

    public  GameObject missionCharacter;
    public  GameObject[] shelterCharacters;

    public  Inventory missionLoadOut;
    public List<VirtualItem> shelterInventoryItems;

    public GameObject deployedCharacter;    //only for mission manager and exit trigger

    void Awake()
    {
        if (instance != null) Destroy(gameObject);

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}

    public void GGM_LoadScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public GameObject[] GGM_GetShelterCharactersArray()
    {
        return shelterCharacters;
    }

    public void GGM_SaveGame()
    {
        totalDays++;
        //save totald
        //save missionCharacter
        //save shelterCharacters
        //save shelterInventory
    }

    public void GGM_StartNewGame()
    {
        totalDays = 0;
        shelterCharacters = initialShelterCharacters;
        missionCharacter = shelterCharacters[0];
        GGM_LoadScene("map_newGameStartMap");
    }

    public List<VirtualItem> GetShelterInventoryItems()
    {
        return shelterInventoryItems;
    }

    public void SetShelterInventoryItems(List<VirtualItem> items)
    {
        shelterInventoryItems = items;
    }

    public void SetCurentMissionManager(MissionGameManager missionManager)
    {
        curentMissionManager = missionManager;
    }
}
