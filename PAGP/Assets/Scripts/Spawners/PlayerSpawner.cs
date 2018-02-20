using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : Spawner
{
  
    public GameObject cameraPrefab;

    GameObject spawnedPlayer;
    GameObject spawnedCamera;


    public override void Spawn()
    {
        spawnedPlayer = Instantiate(GlobalGameManager.instance.missionCharacter, transform.position, Quaternion.identity);

        //Spawn main camera
        //Singleton
        GameObject sceneCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (sceneCamera != null)
        {
            Destroy(sceneCamera);
        }

        spawnedCamera = Instantiate(cameraPrefab, transform.position, Quaternion.identity);

        //Set up main camera
        CameraController cam = spawnedCamera.GetComponent<CameraController>();
        cam.cameraTarget = spawnedPlayer.transform;
        cam.transform.rotation = Quaternion.Euler(60, 0, 0);

        //Sort player object

        GameObject charactersHolder = GameObject.Find("_Characters");
        spawnedPlayer.transform.parent = charactersHolder.transform;
        GlobalGameManager.instance.deployedCharacter = spawnedPlayer;

    }
}
