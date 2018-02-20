using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionGameManager : MonoBehaviour
{
    public static MissionGameManager mgmInstance;
    public GameObject playerSpawnPoint;

    public void Awake()
    {
        mgmInstance = this;
        MGM_StartMission();
    }

    public  void MGM_EndMisson()
    {
        //save loot, save character, return to shelter, 
        GlobalGameManager.instance.GGM_SaveGame();
        GlobalGameManager.instance.GGM_LoadScene("map_shelter");
    }

    public  void MGM_StartMission()
    {
        GlobalGameManager.instance.SetCurentMissionManager(this);
        SpawnNpcsAndPlayer();
    }

    public void SpawnNpcsAndPlayer()
    {
        Spawner[] spawners = GetComponentsInChildren<Spawner>(false);

        foreach(Spawner spawner in spawners)
        {
            spawner.Spawn();
        }
    }

}
