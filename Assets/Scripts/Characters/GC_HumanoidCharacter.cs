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

    protected CharacterAnimationController animationController;
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


        //animatorUpdate
        animationController.targetAnimator.SetLayerWeight(1, 0f);//pistol
        animationController.targetAnimator.SetLayerWeight(2, 0f);//meleeTwoHand
        animationController.targetAnimator.SetLayerWeight(3, 0f);//meleeOneHand
       // animationController.targetAnimator.SetLayerWeight(4, 0f);//rifle

        switch (itemToEquip.itemAnimationType)
        {
            case animTypes.OneHandMelee:
                animationController.targetAnimator.SetLayerWeight(3, 1f);
                break;
            case animTypes.TwoHandMelee:
                animationController.targetAnimator.SetLayerWeight(2, 1f);
                break;
            case animTypes.Pistol:
                animationController.targetAnimator.SetLayerWeight(1, 1f);
                break;
            case animTypes.Rifle:
                //animationController.targetAnimator.SetLayerWeight(4, 1f);
                break;

            default:
                animationController.targetAnimator.SetLayerWeight(1, 0f);//pistol
                animationController.targetAnimator.SetLayerWeight(2, 0f);//meleeTwoHand
                animationController.targetAnimator.SetLayerWeight(3, 0f);//meleeOneHand
                break;
        }
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
