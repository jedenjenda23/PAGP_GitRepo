using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionGameManager : LocalGameManager
{
    public GameObject[] npcSpawnPoints;
    public GameObject playerSpawnPoint;



    public  void MGM_EndMisson()
    {
        
    }

    public  void MGM_StartMission()
    {
        SpawnNpcsAndPlayer();
    }

    public void SpawnNpcsAndPlayer()
    {
        foreach (GameObject spawnObj in npcSpawnPoints)
        {
            //initiate npcSpawners
            //spawnobj.GetComponent<Spawner>().Spawn();
        }

        //initiate playerSpawners
        //playerSpawnPoint.GetComponent<Spawner>().Spawn();
    }

}
