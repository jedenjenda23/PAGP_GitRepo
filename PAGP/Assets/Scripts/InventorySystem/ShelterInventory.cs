using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterInventory : Inventory {
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
            items = GlobalGameManager.instance.GetShelterInventoryItems();
            DrawInventoryUpdate();
        }

        else SetShelterInventory();

        DrawInventory(true, shelterInventoryPosition);
    }

    void SetShelterInventory()
    {
        GlobalGameManager.instance.SetShelterInventoryItems(this.GetInventoryItems());
    }
}
