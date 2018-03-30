using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GC_HumanoidCharacter : GameCharacter
{
    [Header("Hands")]
    public GameObject handsPoint;
    GameObject currentItemPrefab;

    [SerializeField]
    protected float aimingSpeed;
    protected bool aiming;

    [Header("Looting")]
    [SerializeField]
    protected float lootingDistance = 2f;

    protected LootInventory lootInv;
    public new void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    public void ItemToInventory(Inventory inventory, VirtualItem item, ItemContainer itemContainer )
    {
        Vector3 itemPos = itemContainer.gameObject.transform.position;

        if (GetDistance(transform.position, itemPos) < lootingDistance)
        inventory.AddNewItem(item, itemContainer);
    }

    public void TakeItem(GameObject item, bool skipTagRecognition)
    {
        charAnim.PickUpAnimation();
        //skip if we know that is not ItemPickUpContainer
        if(item.CompareTag("ItemPickUpContainer") && !skipTagRecognition)
        {
            VirtualItem newItem;
            newItem = (VirtualItem)item.gameObject.GetComponent<ItemContainer>().GetVirtualItem();
            ItemToInventory(inventory, newItem, item.gameObject.GetComponent<ItemContainer>());
        }

        else
        {
            VirtualItem newItem;
            newItem = (VirtualItem)item.gameObject.GetComponent<ItemContainer>().GetVirtualItem();
            ItemToInventory(inventory, newItem, item.gameObject.GetComponent<ItemContainer>());
        }
    }

    public void EquipItemFromInventory(int itemIndex)
    {
        VirtualItem itemToEquip = inventory.GetInventoryItems()[itemIndex];
        //if item to equip is weapon
        if (currentItemPrefab) Destroy(currentItemPrefab);

        if (itemToEquip.GetItemType() == types.Weapon)
        {
            if (itemToEquip.itemPrefabHands)
            {
                CallItemPrefab(itemToEquip.itemPrefabHands);
            }

            else CallItemModel(itemToEquip.GetItemMesh(), itemToEquip.GetItemMaterial(), itemToEquip.GetItemName());           
        }
        else CallItemModel(null, null, itemToEquip.GetItemName());

    }

    public void CallItemModel(Mesh itemMesh, Material itemMaterial, string itemName)
    {
        MeshRenderer itemRenderer = handsPoint.GetComponent<MeshRenderer>();
        MeshFilter itemMeshFilter = handsPoint.GetComponent<MeshFilter>();

        itemMeshFilter.mesh = itemMesh;
        itemRenderer.material = itemMaterial;
        handsPoint.name = itemName;
    }

    public void CallItemPrefab(GameObject handsPrefab)
    {
        GameObject newHandsModel = Instantiate(handsPrefab, handsPoint.transform);
        currentItemPrefab = newHandsModel;
        handsPoint.name = "modelFromPrefab";
    }

    public virtual void HumanoidCharacterMovement()
    {

    }

#if UNITY_EDITOR
    public virtual void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        pos.y = transform.position.y - 1f;

        Handles.color = Color.yellow;
        Handles.DrawWireDisc(pos, Vector3.up, lootingDistance);
    }
#endif

}
