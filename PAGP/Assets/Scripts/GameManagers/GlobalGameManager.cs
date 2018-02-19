using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour {

    public GameObject[] initialShelterCharacters;

    public  int totalDays;

    public  GameObject missionCharacter;
    public  GameObject[] shelterCharacters;

    public  Inventory missionLoadOut;
    public  Inventory shelterInventory;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
		
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
        //save totalGames
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
}
