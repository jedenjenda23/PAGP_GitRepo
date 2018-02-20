﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Spawner : MonoBehaviour
{
    public virtual void Spawn()
    {
       
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, gameObject.name);
    }
#endif
}
