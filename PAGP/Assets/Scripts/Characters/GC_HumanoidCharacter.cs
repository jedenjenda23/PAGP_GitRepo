using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GC_HumanoidCharacter : GameCharacter
{
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
