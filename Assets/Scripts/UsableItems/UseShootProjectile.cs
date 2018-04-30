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

    public override void Use(Transform parent, Vector3 direction)
    {

        if (Time.time > nextUse)
        {
            nextUse = Time.time + cooldown;

            GameObject newProjectile = Instantiate(projectilePrefab, parent.transform.position, parent.rotation);
            newProjectile.GetComponent<Rigidbody>().AddForce(direction * force);
            newProjectile.GetComponent<Projectile>().shooter = parent;

            Physics.IgnoreCollision(newProjectile.GetComponent<Collider>(), parent.GetComponent<Collider>());
        }


    }
}
