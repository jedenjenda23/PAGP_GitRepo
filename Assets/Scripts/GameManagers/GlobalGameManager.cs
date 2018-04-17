using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager instance;           //instance of GGM
    public MissionGameManager currentMissionManager;    //save mission manager (potentialy usefull one day)

    public int totalDays;
    [HideInInspector]
    public int lastDay = -1;
    public GameObject[] initialShelterCharacters;

    public GameObject missionCharacter;
    public GameObject[] shelterCharacters;

    public List<VirtualItem> missionItems;
    public List<VirtualItem> shelterInventoryItems;

    [HideInInspector]
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

    public void SetMissionLoadoutItems(List<VirtualItem> newItems)
    {
        missionItems = newItems;
    }

    public void LoadItemsToInventory(List<VirtualItem> itemsToAdd,Inventory shelterInventory)
    {
        //add items to inventory - called by the inventory not by the manager
        foreach(VirtualItem item in itemsToAdd)
        {
            Debug.Log("________" + item.GetItemName() + "(*)" + item.GetItemAmount());

            if(item.GetItemType() != types.Null)shelterInventory.AddNewItemFromList(item);
        }
    }

    public void GGM_LoadScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public GameObject[] GGM_GetShelterCharactersArray()
    {
        return shelterCharacters;
    }

    public void GGM_EndMissionProcedures()
    {
        //takes deployedCharacter's iventory a and overrides missionLoadOut
        missionItems = deployedCharacter.GetComponent<Inventory>().GetInventoryItems();

        //save game
        GGM_SaveGame();
    }

    public void GGM_SaveGame()
    {
        lastDay = totalDays;
        totalDays++;

        //save totald
        //save missionCharacter
        //save shelterCharacters
        //save shelterInventory
    }

    public void GGM_StartNewGame()
    {
        lastDay = -1;
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
        currentMissionManager = missionManager;
    }
}
