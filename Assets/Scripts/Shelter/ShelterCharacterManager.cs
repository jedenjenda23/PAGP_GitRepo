using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterCharacterManager : MonoBehaviour
{
    public static ShelterCharacterManager instance;
    public Transform[] spawnPoints;

    private void Awake()
    {
        if (ShelterCharacterManager.instance) Destroy(gameObject);
        else ShelterCharacterManager.instance = this;

        SpawnCharactersInShelter();
    }

    public void SpawnCharactersInShelter()
    {
        if (GlobalGameManager.instance)
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
    }
}
