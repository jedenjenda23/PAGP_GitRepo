 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PAGP/Don't use/VirtualItem")]

public class VirtualItem : InventoryItem
{
    int itemAmount;
    bool itemWeaponAimed;
    public VirtualItem(types newItemType, string newItemName, Sprite newItemIcon, Mesh newItemMesh, Material newItemMaterial, int newItemMaxStack, int newItemAmount, ItemAbility newItemAbility, GameObject newItemPrefabHands, GameObject newItemPrefabGround, animTypes newItemAnimationType)
    {
        itemType = newItemType;
        itemName = newItemName;
        itemIcon = newItemIcon;
        itemMesh = newItemMesh;
        itemMaterial = newItemMaterial;
        itemMaxStack = newItemMaxStack;
        itemAmount = newItemAmount;
        itemAbility = newItemAbility;
        itemPrefabHands = newItemPrefabHands;
        itemPrefabGround = newItemPrefabGround;
        itemAnimationType = newItemAnimationType;
    }
    public void SetAllVariables(InventoryItem inventoryItem, int newItemAmount)
    {
        itemType = inventoryItem.itemType;
        itemName = inventoryItem.itemName;
        itemIcon = inventoryItem.itemIcon;
        itemMesh = inventoryItem.itemMesh;
        itemMaterial = inventoryItem.itemMaterial;
        itemMaxStack = inventoryItem.itemMaxStack;
        itemAmount = newItemAmount;
        itemAbility = inventoryItem.itemAbility;
        itemPrefabHands = inventoryItem.itemPrefabHands;
        itemPrefabGround = inventoryItem.itemPrefabGround;
        itemAnimationType = inventoryItem.itemAnimationType;
    }

    public void MakeItemNull()
    {
        itemType = types.Null;
        itemName = "";
        itemIcon = null;
        itemMesh = null;
        itemMaterial = null;
        itemMaxStack = 1;
        itemAmount = 1;
        itemAbility = null;
        itemPrefabHands = null;
        itemPrefabGround = null;
        itemAnimationType = animTypes.Barehand;
    }

    public int GetItemAmount()
    {
        return itemAmount;
    }

    public bool GetItemWeaponAimed()
    {
        return itemWeaponAimed;
    }

    public void SetItemAmount(int newAmount)
    {
        itemAmount = newAmount;
    }

    public void SetItemWeaponAimed(bool aimed)
    {
        itemWeaponAimed = aimed;
    }
}