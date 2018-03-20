using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemContainer))]

public class LootInventory : Inventory
{
    [SerializeField]
    private List<ItemContainer> itemContainers;

    void Start()
    {
        UpdateContent();

        for (int i = 0; i < itemContainers.Count; i ++)
        {
            AddNewItem(itemContainers[i].GetVirtualItem(), itemContainers[i]);
        }
    }

	void UpdateContent()
    {
        itemContainers = new List<ItemContainer>(GetComponents<ItemContainer>());
        maxInventorySlots = itemContainers.Count;
    }
}
