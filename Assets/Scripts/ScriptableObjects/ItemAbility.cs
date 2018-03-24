using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum abilities { Heal, ShootProjectile, MeleeAttack }

[CreateAssetMenu(menuName = "PAGP/ItemAbility")]
public class ItemAbility : ScriptableObject
{
    public abilities itemAbility;
    public int abilityValue;
    public GameObject projectile;

    public void Heal()
    {
        Debug.Log("Pls purchase DLC for this action");
    }

    public void ShootProjectile(Transform barrel)
    {
        GameObject newProjectile =  Instantiate(projectile, barrel.position, barrel.rotation);
        newProjectile.GetComponent<Rigidbody>().AddForce(barrel.transform.forward * abilityValue);
        Physics.IgnoreCollision(newProjectile.GetComponent<Collider>(), barrel.GetComponent<Collider>());
    }

    public void MeleeAttack()
    {
        Debug.Log("Pls purchase DLC for this action");
    }


    public abilities GetAbility()
    {
        return itemAbility;
    }

}