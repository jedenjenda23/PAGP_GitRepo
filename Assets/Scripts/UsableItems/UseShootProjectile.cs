using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseShootProjectile : Usable
{
    public GameObject projectilePrefab;
    public float force = 2000f;

    private void Start()
    {
    }

    public override void Use(Transform parent)
    {

        if (Time.time > nextUse)
        {
            nextUse = Time.time + cooldown;

            GameObject newProjectile = Instantiate(projectilePrefab, parent.position, parent.rotation);
            newProjectile.GetComponent<Rigidbody>().AddForce(parent.transform.forward * force);

            Physics.IgnoreCollision(newProjectile.GetComponent<Collider>(), parent.GetComponent<Collider>());
        }


    }
}
