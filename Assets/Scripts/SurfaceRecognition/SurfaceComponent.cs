using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceComponent : MonoBehaviour
{
    public SurfaceType surfaceType;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (surfaceType != null) Gizmos.color = Color.green;
        else Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }

    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.Label(transform.position, "" + surfaceType);
    }
#endif
}
