using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public PrefabHolder guiPrefabHolder;
    public bool canDropItems = true;
    public int maxInventorySlots = 5;
    public int selectedSlot;
    public bool playerInventory = false;
    public bool lockSelection = true;
    protected GameObject myInventoryObject;
    protected Vector3 lastInventoryPosition;
    protected List<GameObject> itemSlots;
    protected List<GameObject> equipementItemSlots;

    [SerializeField]
    protected List<VirtualItem> items = new List<VirtualItem>(1);
    protected List<VirtualItem> equipedItems = new List<VirtualItem>(1);


    protected void Start()
    {
        for (int i = 0; i < maxInventorySlots; i++)
        {
            items.Add(guiPrefabHolder.GetNullItem());
        }
    }
    public void AddNewItemFromList(VirtualItem newInventoryItem)
    {
        VirtualItem newItem = CreateVirtualItem(newInventoryItem);
        items.Add(newItem);

        DrawInventoryUpdate();
    }
    public void AddNewItem(VirtualItem newInventoryItem, ItemContainer containerReference)
    {
        int i = 0;

        // First search if this item already exists in our inventory
        if (items.Count != 0)
        {
            for (i = 0; i < items.Count; i++)
            {
                // If we find  full slot
                if (items[i] != null)
                {
                    VirtualItem newItem1 = CreateVirtualItem(newInventoryItem);
                    VirtualItem currentItem = items[i];

                    // If we find  occupyied slot that is the same item
                    if (items[i].GetItemName() == newItem1.GetItemName())
                    {

                        // Calculate free space in stack
                        int freeSpace = GetFreeSpaceInStack(currentItem.GetItemAmount(), newItem1.GetItemMaxStack(), newItem1.GetItemAmount(), 0);
                        // Debug.Log("fs:" + freeSpace);

                        if (freeSpace > 0 && freeSpace > newItem1.GetItemAmount())
                        {

                            freeSpace = Mathf.Clamp(freeSpace, 1, newItem1.GetItemAmount());

                            //   Debug.Log("3" + " " + i + "_" + (currentItem.GetItemAmount() + freeSpace));

                            currentItem.SetItemAmount(currentItem.GetItemAmount() + freeSpace);

                            if(containerReference)SubtractValuesFromSource(containerReference, newInventoryItem, freeSpace);

                        }

                        else if (freeSpace >= 0 && freeSpace < newItem1.GetItemAmount())
                        {


                            // Debug.Log("4" + " *" + newItem1.GetItemAmount());

                            currentItem.SetItemAmount(currentItem.GetItemAmount() + freeSpace);
                            //  Debug.Log("4" + " " + i + "_" + currentItem.GetItemAmount());
                            if (containerReference) SubtractValuesFromSource(containerReference, newInventoryItem, freeSpace);
                        }
                    }
                }
            }
        }


        // Then we search for empty slots
        if (i == items.Count || items.Count == 0)
        {
            // If there is no slot, create one
            if (items.Count == 0 || items.Count < maxInventorySlots) items.Add(guiPrefabHolder.GetNullItem());

            for (int b = 0; b < items.Count; b++)
            {
                // If we find empty slot
                if (items[b] == null && b < maxInventorySlots || items[b].GetItemType() == types.Null && b < maxInventorySlots)
                {
                    VirtualItem newItem = CreateVirtualItem(newInventoryItem);

                    // If source itemItemAmount() is smaller than max stack
                    if (newItem.GetItemAmount() < newItem.GetItemMaxStack() && newItem.GetItemAmount() > 0)
                    {
                        //   Debug.Log("2" + " " + b + "_" + newItem.GetItemAmount());
                        // items.Insert(b, newItem);
                        InsertItemInSlot(newItem, b);
                        if (containerReference) SubtractValuesFromSource(containerReference, newInventoryItem, newItem.GetItemAmount());
                    }

                    // If source itemItemAmount() is bigger or same than max stack
                    else if (newItem.GetItemAmount() >= newItem.GetItemMaxStack() && newItem.GetItemAmount() > 0)
                    {
                        int z = newItem.GetItemAmount() - newItem.GetItemMaxStack();

                        newItem.SetItemAmount(newItem.GetItemAmount() - z);
                        //items.Insert(b, newItem);
                        InsertItemInSlot(newItem, b);

                        //   Debug.Log("1" + " " + i + "_" + (newItem.GetItemAmount()));
                        if (containerReference) SubtractValuesFromSource(containerReference, newInventoryItem, newItem.GetItemAmount());
                    }
                }
            }
        }

        DrawInventoryUpdate();
    }
    private void SubtractValuesFromSource(ItemContainer containerReference, VirtualItem newInventoryItem, int value)
    {
        newInventoryItem.SetItemAmount(newInventoryItem.GetItemAmount() - value);
        // Debug.Log("subtractedNewInventoryItem:" + (newInventoryItem.GetItemAmount()));
        containerReference.ItemContainerSetAmount();

    }

    public int StackItems(VirtualItem item1, VirtualItem item2, bool item2IsExternal)
    {
        int freeSpace = GetFreeSpaceInStack(item1.GetItemAmount(), item2.GetItemMaxStack(), item1.GetItemAmount(), 0);
        freeSpace = Mathf.Clamp(freeSpace, 1, item2.GetItemAmount());

        item1.SetItemAmount(item1.GetItemAmount() + freeSpace);
        if (!item2IsExternal) item2.SetItemAmount(item1.GetItemAmount() - freeSpace);

        DrawInventoryUpdate();
        return freeSpace;

    }

    public int GetFreeSpaceInStack(int currentStack, int maxStack, int newStack, int method)
    {
        int a = newStack - (newStack - (maxStack - currentStack));
        int b = (maxStack - (currentStack + newStack));



        if (method == 0)
        {
            //   Debug.Log(newStack + " + " + "(" + newStack + "-" + "(" + maxStack + "-" + currentStack + "))" + "=" + a + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + method);
            return a;
        }
        else//this method is shit...
        {
            //  Debug.Log("(m" + maxStack + "-" + "(n" + newStack + "+c" + currentStack + "))" + "=" + b + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + method);
            return b;
        }

    }

    private VirtualItem CreateVirtualItem(VirtualItem addedItem)
    {
        VirtualItem createdItem = (VirtualItem)ScriptableObject.CreateInstance("VirtualItem");
        createdItem.SetAllVariables(addedItem, addedItem.GetItemAmount());
        return createdItem;
    }

    public List<VirtualItem> GetInventoryItems()
    {
        return items;
    }

    public List<VirtualItem> GetEquipedItems()
    {
        return equipedItems;
    }

    public virtual void DrawInventory(bool drawInventory, Vector3 inventoryPosition)
    {
        // Remember where to spawn (only for RefreshInventory() function)
        lastInventoryPosition = inventoryPosition;

        if (drawInventory)
        {
            GameObject myCanvas = null;
            GameObject myHotbar = null;
            GameObject myItemSlot = null;
            GameObject myGuiItem = null;


            // Find GUI prefabs for the inventory
            for (int a = 0; a < guiPrefabHolder.GetLength(); a++)
            {
                if (guiPrefabHolder.GetName(a) == "GUI_Canvas") myCanvas = guiPrefabHolder.GetObject(a);
                else if (guiPrefabHolder.GetName(a) == "GUI_Hotbar") myHotbar = guiPrefabHolder.GetObject(a);
                else if (guiPrefabHolder.GetName(a) == "GUI_ItemSlot") myItemSlot = guiPrefabHolder.GetObject(a);
                else if (guiPrefabHolder.GetName(a) == "GUI_Item") myGuiItem = guiPrefabHolder.GetObject(a);
            }

            if (myCanvas != null && myHotbar != null && myItemSlot != null && myGuiItem != null)
            {
                //Insantiante Inventory Canvas
                myInventoryObject = Instantiate(myCanvas, Vector3.zero, Quaternion.identity);
                myInventoryObject.name = "Inventory of: " + gameObject.name;

                //Insantiante Hotbar
                GameObject newHotbar = Instantiate(myHotbar, myInventoryObject.GetComponent<RectTransform>());
                newHotbar.GetComponent<RectTransform>().localPosition = inventoryPosition;

                //Insantiante ItemSlots
                itemSlots = new List<GameObject>();
                for (int i = 0; i < maxInventorySlots; i++)
                {
                    GameObject newItemSlot = Instantiate(myItemSlot, newHotbar.GetComponent<RectTransform>());
                    newItemSlot.name = gameObject.name + "_" + i;
                    itemSlots.Add(newItemSlot);

                    // Scale selected slot
                    //if(selectedSlot == i)newItemSlot.GetComponent<RectTransform>().localScale *= 1.2f;
                }

                // Resize Hotbar acording to item slot count and size
                newHotbar.GetComponent<RectTransform>().sizeDelta = (itemSlots[1].GetComponent<RectTransform>().sizeDelta) * itemSlots.Count;


                //Instantiate Items
                List<GameObject> guiItems = new List<GameObject>();

                for (int v = 0; v < GetInventoryItems().Count; v++)
                {
                    if (GetInventoryItems()[v] != null)
                    {
                        GameObject newGuiItem = Instantiate(myGuiItem, myInventoryObject.GetComponent<RectTransform>());
                        newGuiItem.name = items[v].GetItemName();
                        guiItems.Add(newGuiItem);
                    }

                }

                //Give items information
                for (int x = 0; x < GetInventoryItems().Count; x++)
                {
                    if (GetInventoryItems()[x] != null)
                    {
                        //guiItems[x].GetComponent<RectTransform>().position = itemSlots[x].GetComponent<RectTransform>().position;
                        bool selected = false;
                        if (selectedSlot == x) selected = true;

                        if (GetInventoryItems()[x].GetItemAmount() <= 0)
                        {
                            GetInventoryItems()[x].MakeItemNull();
                        }

                        guiItems[x].GetComponent<RectTransform>().SetParent(itemSlots[x].GetComponent<RectTransform>());
                        guiItems[x].GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

                        guiItems[x].GetComponent<Gui_Item>().SetItemValues(
                            GetInventoryItems()[x].GetItemName(),
                            GetInventoryItems()[x].GetItemAmount(),
                            GetInventoryItems()[x].GetItemIcon(),
                            x,
                            this,
                            selected);
                    }

                }

                // Scale selected slot
                if (!lockSelection) itemSlots[selectedSlot].GetComponent<RectTransform>().localScale *= 1.2f;


            }
        }


        else DestroyObject(myInventoryObject);

    }

    public virtual void DrawInventoryUpdate()
    {
        Debug.Log("updating inventory");
        //if we update nonexistent inventory ui
        bool inventoryActive;
        if (myInventoryObject == null) inventoryActive = false;
        else inventoryActive = true;

        Vector3 lastPos = lastInventoryPosition;
        DrawInventory(false, lastPos);
        Destroy(myInventoryObject);
        DrawInventory(inventoryActive, lastPos);
    }
    public void DebugInventory()
    {
        Debug.Log("----------------NEW INVENTORY LOG----------------");
        Debug.Log("inventory of:" + gameObject.name);
        for (int a = 0; a < items.Count; a++)
        {

            Debug.Log("Slot " + a + items[a].GetItemName() + " (" + items[a].GetItemAmount() + "/" + items[a].GetItemMaxStack() + ")");
        }
    }

    public void InventoryDropItem(int index)
    {
        if (items[index] != null && index < maxInventorySlots)
        {
            // Get spawn position in radius
            float dropRadius = 1.5f;
            Vector3 spawnPos = transform.position;

            spawnPos.x = spawnPos.x + Random.Range(-dropRadius, dropRadius);
            spawnPos.z = spawnPos.z + Random.Range(-dropRadius, dropRadius);
            RaycastHit hit;

            // We want place our item on surface
            if (Physics.Raycast(spawnPos, Vector3.down, out hit))
            {
                spawnPos.y = hit.distance - 0.1f;
            }

            // Get item to drop
            VirtualItem itemToDrop = items[index];

            // Create Empty Object
            GameObject droppedItem = new GameObject(itemToDrop.GetItemName());

            // Create Components
            // droppedItem.AddComponent<Transform>();
            droppedItem.AddComponent<SphereCollider>();
            droppedItem.AddComponent<MeshFilter>().mesh = itemToDrop.GetItemMesh();
            droppedItem.AddComponent<MeshRenderer>().material = itemToDrop.GetItemMaterial();

            ItemContainer itemContainer = droppedItem.AddComponent<ItemContainer>();

            // Set Components
            droppedItem.transform.position = spawnPos;
            droppedItem.gameObject.tag = "ItemPickUpContainer";
            droppedItem.GetComponent<SphereCollider>().isTrigger = true;
            itemContainer.SetVirtualItem(CreateVirtualItem(itemToDrop), itemToDrop.GetItemAmount());

            // Remove item from list and update inventory
            RemoveItemFromList(index, true);
        }

    }

    public void InsertItemInSlot(VirtualItem item, int slotIndex)
    {
        items[slotIndex] = item;
        DrawInventoryUpdate();
       // Debug.Log(items[slotIndex]);
    }

    public void RemoveItemFromList(int index, bool updateInventory)
    {
        items[index].MakeItemNull();
        if (updateInventory) DrawInventoryUpdate();
    }

    public void SwapItems(VirtualItem item1, VirtualItem item2, Inventory inventory1, Inventory inventory2)
    {
        VirtualItem tempItem1 = item1;
        int index1 = inventory1.GetInventoryItems().IndexOf(item1);

        int index2 = inventory2.GetInventoryItems().IndexOf(item2);

        inventory1.InsertItemInSlot(item2, index1);
        inventory2.InsertItemInSlot(tempItem1, index2);

        // inventory1.GetInventoryItems()[index1] = item2;
        // inventory2.GetInventoryItems()[index2] = tempItem1;
        DrawInventoryUpdate();
    }

    public void SetSelectedSlot(int index)
    {
        if (!lockSelection)
        {
            if (index >= items.Count) index = 0;
            else if (index < 0) index = items.Count - 1;
            selectedSlot = index;
            DrawInventoryUpdate();
        }
    }

    public int GetSelectedSlot()
    {
        return selectedSlot;
    }

    public void UpdateInventoryAccordingToUI()
    {
        List<VirtualItem> guiItems = new List<VirtualItem>();
        for (int i = 0; i < itemSlots.Count; i++)
        {
            VirtualItem itm = itemSlots[i].GetComponentInChildren<Gui_Item>().GetItem();

            if (itm == null)
            {
                itm = CreateVirtualItem(guiPrefabHolder.nullItem);
                itm.MakeItemNull();
            }

            guiItems.Add(itm);
        }

        if (playerInventory)
        {
            List<VirtualItem> guiEqItems = new List<VirtualItem>();
            for (int i = 0; i < equipedItems.Count; i++)
            {
                VirtualItem eqItm = equipementItemSlots[i].GetComponentInChildren<Gui_Item>().GetItem();

                if (eqItm == null)
                {
                    eqItm = CreateVirtualItem(guiPrefabHolder.nullItem);
                    eqItm.MakeItemNull();
                }

                guiEqItems.Add(eqItm);
            }

            equipedItems = guiEqItems;
        }

        items = guiItems;
        DrawInventoryUpdate();
    }
}