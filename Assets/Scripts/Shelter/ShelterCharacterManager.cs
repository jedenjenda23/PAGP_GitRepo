using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShelterCharacterManager : MonoBehaviour
{
    public static ShelterCharacterManager instance;
    public Transform[] spawnPoints;
    public RectTransform characterPanel;
    public GameObject characterCardPreset;
    public Text daysCounterText;

    private void Awake()
    {
        if (ShelterCharacterManager.instance) Destroy(gameObject);
        else ShelterCharacterManager.instance = this;

        if (GlobalGameManager.instance)
        {
            //  SpawnCharactersInShelter();
            AddTotalDays();
            SpawnCharacterPanelCards();

        }
    }

    public void SpawnCharactersInShelter()
    {
            if (GlobalGameManager.instance.shelterCharacters.Length > 0 && spawnPoints.Length >= GlobalGameManager.instance.shelterCharacters.Length)
            {
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    GameObject charToSpawn = GlobalGameManager.instance.shelterCharacters[i];

                    GameObject newCharacter = Instantiate(charToSpawn, spawnPoints[i], false);
                    newCharacter.transform.localPosition = Vector3.zero;

                    //deactivate unnecessary sh*ts
                    newCharacter.GetComponent<GC_PlayableHumanoidCharacter>().enabled = false;
                    newCharacter.GetComponent<Inventory>().enabled = false;
                }
            }
    }

    public void AddTotalDays()
    {
        Debug.Log(daysCounterText.gameObject);
        daysCounterText.text = "Total days: " + GlobalGameManager.instance.totalDays;
    }

    public void SpawnCharacterPanelCards()
    {
            if (GlobalGameManager.instance.shelterCharacters.Length >= GlobalGameManager.instance.shelterCharacters.Length)
            {
                for (int i = 0; i < GlobalGameManager.instance.shelterCharacters.Length; i++)
                {
                    GameObject charToSpawn = GlobalGameManager.instance.shelterCharacters[i];
                
                    GameObject newCharacterCard = Instantiate(characterCardPreset, characterPanel);
                    newCharacterCard.transform.localPosition = Vector3.zero;

                    //recalculateCharacter
                    if (GlobalGameManager.instance.totalDays > GlobalGameManager.instance.lastDay)
                    {
                    charToSpawn.GetComponent<CharacterAttributes>().RecalculateStats();
                    }

                    newCharacterCard.GetComponent<CharacterCard>().LoadCharacterGraphics(charToSpawn.GetComponent<CharacterAttributes>());
                }
            }
    }
}
