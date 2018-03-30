using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvetory : Inventory
{
    GC_PlayableHumanoidCharacter playableCharacter;
    private new void Start()
    {
        for (int i = 0; i < maxInventorySlots; i++)
        {
            items.Add(guiPrefabHolder.GetNullItem());
        }

        playableCharacter = GetComponent<GC_PlayableHumanoidCharacter>();
    }

    public override void DrawInventory(bool drawInventory, Vector3 inventoryPosition)
    {
        base.DrawInventory(drawInventory, inventoryPosition);


    }

    public override void DrawInventoryUpdate()
    {
        Vector3 lastPos = lastInventoryPosition;
        DrawInventory(false, lastPos);
        Destroy(myInventoryObject);
        DrawInventory(true, lastPos);

        //draw selected item in game
        DrawSelectedItemInCharacterHands();
    }

    void DrawSelectedItemInCharacterHands()
    {
        playableCharacter.EquipItemFromInventory(GetSelectedSlot());
    }


}
