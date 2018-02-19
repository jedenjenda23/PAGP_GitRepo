using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterInventory : Inventory {
    // Use this for initialization
    GlobalGameManager gGameManager;
    public Vector3 shelterInventoryPosition;

    void Start()
    {
        gGameManager = GameObject.Find("_GlobalGameManager").GetComponent<GlobalGameManager>();
        for (int i = 0; i < maxInventorySlots; i++)
        {
            items.Add(guiPrefabHolder.GetNullItem());
        }

        LoadShelterInventory();
    }

    public void LoadShelterInventory()
    {

        if (gGameManager.shelterInventory != null)
        {
            items = gGameManager.shelterInventory.GetInventoryItems();
            DrawInventoryUpdate();
        }

        else SetShelterInventory();

        DrawInventory(true, shelterInventoryPosition);
    }

    void SetShelterInventory()
    {
        gGameManager.shelterInventory = this;
    }
}
