﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PAGP/PrefabHolder")]

public class PrefabHolder : ScriptableObject
{
    public GameObject guiItemPrefab;
    public GameObject guiItemSlotPrefab;
    public GameObject[] prefabs;
    internal int Length;
    public VirtualItem nullItem;

    public GameObject GetObject(int index)
    {
        return prefabs[index].gameObject;
    }
    public string GetName(int index)
    {
        return prefabs[index].name;
    }

    public int GetLength()
    {
        return prefabs.Length;
    }

    public VirtualItem GetNullItem()
    {
        return nullItem;
    }


}
