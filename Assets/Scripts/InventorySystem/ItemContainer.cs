using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemContainer : MonoBehaviour
{
    public bool useItemMesh = true;
    public InventoryItem item;
    public int amount;
    VirtualItem myItem;
    void Start()
    {
        myItem = GetVirtualItem();
        RenderItem();
	}


    private void LateUpdate()
    {
        /*
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;

        float emission = Mathf.PingPong(Time.time, 1.0f);
        Color baseColor = Color.white;

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
        */
    }

    public void RenderItem()
    {
        if (useItemMesh)
        {
            GetComponent<MeshFilter>().mesh = item.itemMesh;
            GetComponent<MeshRenderer>().material = item.itemMaterial;
        }
    }
    public VirtualItem GetVirtualItem()
    {
        myItem = (VirtualItem)ScriptableObject.CreateInstance("VirtualItem");
        myItem.SetAllVariables(item, amount);
        return myItem;
    }

    public void SetVirtualItem(VirtualItem inventoryItem, int newAmount)
    {
        item =  inventoryItem;
        amount = newAmount;
    }

    public void ItemContainerSetAmount()
    {
        amount = myItem.GetItemAmount();
        if(amount <= 0)
        {
            if (!gameObject.GetComponent<LootInventory>())
            {
                DestroyObject(gameObject);
            }
        }
    }
    

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Handles.Label(transform.position, "" + item + "(amount: " + amount + ")");
    }
#endif
}
