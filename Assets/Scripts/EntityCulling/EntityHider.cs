using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHider : MonoBehaviour
{
    [HideInInspector]
    public bool hidden;

    MeshRenderer[] myMeshes;
    SkinnedMeshRenderer[] mySkinnedMeshes;

    private void Awake()
    {
        myMeshes = GetComponentsInChildren<MeshRenderer>();
        mySkinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public void ToggleShow(bool toggle)
    {
        if(myMeshes.Length > 0)
        {
            foreach (MeshRenderer mesh in myMeshes)
            {
                mesh.enabled = toggle;
            }
        }

        if (mySkinnedMeshes.Length > 0)
        {
            foreach (SkinnedMeshRenderer skinnedMesh in mySkinnedMeshes)
            {
                skinnedMesh.enabled = toggle;
            }
        }

        hidden = !toggle;
    }
}
