using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : MonoBehaviour
{

    [SerializeField]
    protected float cooldown;
    [SerializeField]
    protected float nextUse;

    public virtual void Use(Transform parent)
    {
    }
}
