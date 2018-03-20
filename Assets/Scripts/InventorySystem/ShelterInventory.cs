using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterInventory : Inventory
{
    // Use this for initialization
    public Vector3 shelterInventoryPosition;

    void Start()
    {
        for (int i = 0; i < maxInventorySlots; i++)
        {
            items.Add(guiPrefabHolder.GetNullItem());
        }

        LoadShelterInventory();
    }

    public void LoadShelterInventory()
    {

        if (GlobalGameManager.instance != null)
        {
            //load saved inventory
            items = GlobalGameManager.instance.GetShelterInventoryItems();
            
            //add items aquired on mission
            GlobalGameManager.instance.LoadItemsToInventory(GlobalGameManager.instance.missionItems, this);

            DrawInventoryUpdate();
        }

        DrawInventory(true, shelterInventoryPosition);
    }

    void SaveShelterInventory()
    {
        GlobalGameManager.instance.SetShelterInventoryItems(this.GetInventoryItems());
    }
}
