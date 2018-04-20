using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class jjGroupObjects : MonoBehaviour
{
    [MenuItem("JJToolkit/Functions/GroupSelectedObjects %g")]


    // create group of objects
    public static void GroupObject()
    {
        Vector3 groupOrigin = FindSelectionCenter();

        GameObject groupEmpty = new GameObject("GROUP (size: " + Selection.transforms.Length + ")" + "(" + Selection.transforms[0].name + ")");
        groupEmpty.transform.position = groupOrigin;

        foreach(Transform gameObj in Selection.transforms)
        {
            gameObj.SetParent(groupEmpty.transform, true);
        }
    }

    static Vector3 FindSelectionCenter()
    {
        Vector3 center = new Vector3(0, 0, 0);
        float count = 0f;

        foreach (Transform obj in Selection.transforms)
        {
            center += obj.position;
            count++;
        }

        return center / count;
    }
}
