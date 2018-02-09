using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : Spawner
{
    //public GameObject playerUIPrefab;
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public bool spawnOnAwake = true;

    GameObject spawnedPlayer;
    GameObject spawnedCamera;

    private void Awake()
    {
        if (spawnOnAwake)
        {
            SpawnPlayer();
        }   
    }

    public void SpawnPlayer()
    {
        //Spawn Player UI
        /*
        GameObject oldPlayerUI = GameObject.Find("PlayerUI");

        if (oldPlayerUI != null)
        {
            Destroy(oldPlayerUI);
            Debug.Log("Deleted player UI. New one will be spawned");
        }

        Instantiate(playerUIPrefab, transform.position, Quaternion.identity);
        */

        //spawnedCamera = Instantiate(cameraPrefab, transform.position, Quaternion.identity);
        //Spawn player object
        //Singleton
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            foreach (GameObject obj in players)
            {
                Destroy(obj);
            }
        }

        spawnedPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);


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

    }
}
