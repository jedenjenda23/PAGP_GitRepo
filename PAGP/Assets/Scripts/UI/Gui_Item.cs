using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Gui_Item : MonoBehaviour
{
   
    public Image itemImage;
    public Text itemNameText;
    public Text itemAmountText;

    int itemAmount;
    int itemInventoryIndex;
    bool selected;
    Vector2 screenPos;
    Inventory myInventory;
    VirtualItem myItem;
    GameObject closestObj;
    Transform prevParent;
    RectTransform rt;
    List<GameObject> items;

 

    public void Start()
    {
        rt = GetComponent<RectTransform>();
        UpdateParent(transform.parent);
        screenPos = GetScreenPosition(rt);
    }

    public void SetItemValues(string newItemName, int newItemAmount, Sprite newItemIcon, int newItemIndex, Inventory newInventory, bool newSelected)
    {
        itemNameText.text = newItemName;
        itemAmount = newItemAmount;
        itemAmountText.text = newItemAmount + "";
        itemImage.sprite = newItemIcon;
        myInventory = newInventory;
        itemInventoryIndex = newItemIndex;
        selected = newSelected;
        myItem = myInventory.GetInventoryItems()[itemInventoryIndex];

        ShowItemAmount(true);

        if (myInventory.GetInventoryItems()[itemInventoryIndex].GetItemType() == types.Null)
        {
            itemImage.enabled = false;
        }
    }

    public void InventoryDropItem()
    {
        myInventory.InventoryDropItem(itemInventoryIndex);
    }

    public void MouseMoveItem(bool toggle)
    {
        Gui_Item closestItem = FindClosestItemOnGui().GetComponent<Gui_Item>();
        float distance = Vector3.Distance(GetScreenPosition(closestItem.rt), GetScreenPosition(rt));
        float distToOriginalSlot = Vector3.Distance(GetScreenPosition(prevParent.GetComponent<RectTransform>()), GetScreenPosition(rt));
        // float distance = Vector3.Distance(GetScreenPosition(closestItem.rt), new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height));

        // If we drag item
        if (toggle)
        {
            rt.SetParent(transform.root);
            rt.position = Input.mousePosition;
        }

        // On release

        else
        {
            // If we have slot close to our itemGui prefab
            if (distance < (rt.sizeDelta.x / Screen.width) * 2)
            {

                //But first...Can we stack those items? :O
                if (closestItem.GetItem().GetItemName() == myItem.GetItemName() &&
                    closestItem.GetItemAmount() < closestItem.GetItem().GetItemMaxStack())
                {
                    // Get free space in stack
                    VirtualItem clItem = closestItem.GetItem();
                    int freeSpace = clItem.GetItemMaxStack() - clItem.GetItemAmount();

    

                    // If we have more free space in stack than amount of dragged item
                    if (myItem.GetItemAmount() <= freeSpace)
                    {
                        // Add amount of dragged item, then delete dragged item
                        clItem.SetItemAmount(clItem.GetItemAmount() + myItem.GetItemAmount());
                        myItem.SetItemAmount(myItem.GetItemAmount() - myItem.GetItemAmount());
                        rt.SetParent(prevParent);
                    }

                    // If we have less space in stack than amount of dragged item
                    else if(myItem.GetItemAmount() > freeSpace)
                    {
                        // Calculate how much we can transfer to stack, transfer values, return dragged item to it's slot
                        int difference = myItem.GetItemAmount() - freeSpace;

                        clItem.SetItemAmount(clItem.GetItemAmount() + (myItem.GetItemAmount() - difference));
                        myItem.SetItemAmount(myItem.GetItemAmount() - (myItem.GetItemAmount() - difference));

                        rt.SetParent(prevParent);

                    }

                    // If we don't have enough free space in stack, swap 
                    else
                    {
                        SwapItems(closestItem);
                    }

                }
                // If not, just swap them baby...
                else
                {
                    SwapItems(closestItem);
                }

                // update inventories
                myInventory.UpdateInventoryAccordingToUI();
                closestItem.GetInventory().UpdateInventoryAccordingToUI();
            }

            // If we DO NOT have any slot close to our itemGui prefab, and it's too far from original slot (if we drag item from inventory)
            // -drop item
           

            else if (distance > (rt.sizeDelta.x / Screen.width) * 2f && distToOriginalSlot > (rt.sizeDelta.x / Screen.width) * 2f)
            {
                myInventory.InventoryDropItem(itemInventoryIndex);
            }

            // Else return to slot
            else
            {
                rt.SetParent(prevParent);
                rt.localPosition = Vector3.zero;
            }

        }
    }

    public GameObject FindClosestItemOnGui()
    {
        GameObject[] itemObj = GameObject.FindGameObjectsWithTag("InventoryGuiItem");
        items = itemObj.ToList();

        float curDistance = 9999.9f;
        GameObject closest = null;
       
        /*
        for(int i = 0; i < items.Count; i++)
        {
            GameObject obj = items[i];
            float dist = Vector3.Distance(obj.GetComponent<Gui_Item>().GetScreenPosition(obj.GetComponent<RectTransform>()), screenPos);
            if (dist < curDistance && obj != gameObject) closest = obj;

            return closest;
        }
        */

        foreach (GameObject obj in items)
        {
            float dist = Vector3.Distance(obj.GetComponent<Gui_Item>().GetScreenPosition(obj.GetComponent<RectTransform>()), screenPos);
            
                if (dist < curDistance && obj != this.gameObject)
                {
                closest = obj;
                curDistance = dist;
                }
        }

        return closest;
    }

    public void ShowItemAmount(bool toggle)
    {
        if (myInventory.GetInventoryItems()[itemInventoryIndex].GetItemType() == types.Null) toggle = false;
        else if (myInventory.GetInventoryItems()[itemInventoryIndex].GetItemType() == types.Weapon) toggle = false;
        itemAmountText.gameObject.SetActive(toggle);
    }
    public void ShowItemName(bool toggle)
    {
        if (myInventory.GetInventoryItems()[itemInventoryIndex].GetItemType() == types.Null) toggle = false;
        itemNameText.gameObject.SetActive(toggle);
    }

    public void UpdateParent(Transform parent)
    {
        prevParent = parent;
        rt.SetParent(prevParent);
    }

    public Inventory GetInventory()
    {
        return myInventory;
    }

    public int GetInventoryIndex()
    {
        return itemInventoryIndex;
    }

    public VirtualItem GetItem()
    {
        return myItem;
    }

    public Vector2 GetScreenPosition(RectTransform rectTr)
    {
        return screenPos = new Vector2(rectTr.position.x / Screen.width, rectTr.position.y / Screen.height);
    }

    public void SetItemAmount(int newAmount)
    {
        itemAmount = newAmount;
    }

    public int GetItemAmount()
    {
        return itemAmount;
    }

    public void SwapItems(Gui_Item closestItem)
    {
        Transform tempRt = closestItem.transform.parent;

        closestItem.UpdateParent(prevParent);
        closestItem.GetComponent<RectTransform>().localPosition = Vector3.zero;
        UpdateParent(tempRt);
        rt.localPosition = Vector3.zero;

        closestItem.GetInventory().UpdateInventoryAccordingToUI();
        myInventory.UpdateInventoryAccordingToUI();
    }
}
