using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvetory : Inventory
{

    public override void DrawInventory(bool drawInventory, Vector3 inventoryPosition)
    {
        // Remember where to spawn (only for RefreshInventory() function)
        lastInventoryPosition = inventoryPosition;

        if (drawInventory)
        {
            GameObject myHotbar = UI_PlayerUI.instance.playerHotbar;

            Debug.Log(myHotbar + "_" + guiPrefabHolder.guiItemSlotPrefab + "_" + guiPrefabHolder.guiItemPrefab);
            if (myHotbar != null)
            {
                //Insantiante ItemSlots
                itemSlots = new List<GameObject>();
                for (int i = 0; i < maxInventorySlots; i++)
                {
                    GameObject newItemSlot = Instantiate(guiPrefabHolder.guiItemSlotPrefab, myHotbar.GetComponent<RectTransform>());
                    newItemSlot.name = gameObject.name + "_" + i;
                    itemSlots.Add(newItemSlot);

                    // Scale selected slot
                    //if(selectedSlot == i)newItemSlot.GetComponent<RectTransform>().localScale *= 1.2f;
                }

                // Resize Hotbar acording to item slot count and size
                myHotbar.GetComponent<RectTransform>().sizeDelta = (itemSlots[1].GetComponent<RectTransform>().sizeDelta) * itemSlots.Count;


                //Instantiate Items
                List<GameObject> guiItems = new List<GameObject>();

                for (int v = 0; v < items.Count; v++)
                {
                    Debug.Log("instantiateItem " + GetInventoryItems()[v]);
                        GameObject newGuiItem = Instantiate(guiPrefabHolder.guiItemPrefab, myInventoryObject.GetComponent<RectTransform>());
                        newGuiItem.name = items[v].GetItemName();
                        guiItems.Add(newGuiItem);
                }

                Debug.Log("sda:" + items.Count);
                //Give items information
                for (int x = 0; x < GetInventoryItems().Count; x++)
                {
                    Debug.Log("infoItem " + GetInventoryItems()[x]);
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

                Debug.Log("7");
                // Scale selected slot
                if (!lockSelection) itemSlots[selectedSlot].GetComponent<RectTransform>().localScale *= 1.2f;


            }
        }


        else DestroyObject(myInventoryObject);

    }
    public override void DrawInventoryUpdate()
    {
        Vector3 lastPos = lastInventoryPosition;
        DrawInventory(false, lastPos);
        Destroy(myInventoryObject);
        DrawInventory(true, lastPos);
    }

}
