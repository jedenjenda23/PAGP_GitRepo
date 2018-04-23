using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum animTypes { Barehand, OneHandMelee, TwoHandMelee, Pistol, Rifle};
public enum types { Weapon, Ammo, Medicine, Nutrition, Null };
[CreateAssetMenu(menuName = "PAGP/InventoryItem")]

public class InventoryItem : ScriptableObject

{
    public types itemType;
    public animTypes itemAnimationType = animTypes.Barehand;

    public string itemName = "NewItem";
    public Sprite itemIcon;
    public GameObject itemPrefabGround;
    public GameObject itemPrefabHands;
    public Mesh itemMesh;
    public Material itemMaterial;

    public float itemWeight;
    public int itemMaxStack = 10;   

    [HideInInspector]//obsolete after ItemPrefab Update (JF) 30.03.2018 - Don't use
    public ItemAbility itemAbility;

    public string GetItemName()
    {
        return itemName;
    }

    public int GetItemMaxStack()
    {
        return itemMaxStack;
    }

    public float GetItemWeigh()
    {
        return itemWeight;
    }

    public types GetItemType()
    {
        return itemType;
    }

    public Mesh GetItemMesh()
    {
        return itemMesh;
    }

    public Material GetItemMaterial()
    {
        return itemMaterial;
    }


    public Sprite GetItemIcon()
    {
        return itemIcon;
    }
    public ItemAbility GetItemAbility()
    {
        return itemAbility;
    }
}
/*
public class VirtualItem : InventoryItem
{
    int itemAmount;
    public VirtualItem(types newItemType, string newItemName, Sprite newItemIcon, Mesh newItemMesh, Material newItemMaterial, int newItemMaxStack, int newItemAmount)
    {
        itemType = newItemType;
        itemName = newItemName;
        itemIcon = newItemIcon;
        itemMesh = newItemMesh;
        itemMaterial = newItemMaterial;
        itemAmount = newItemAmount;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public int GetItemMaxStack()
    {
        return itemMaxStack;
    }
    public int GetItemAmount()
    {
        return itemAmount;
    }
    public void SetItemAmount(int newItemAmount)
    {
        itemAmount = newItemAmount;
    }
}
*/
