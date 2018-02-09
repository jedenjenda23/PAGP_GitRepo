using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    int damage = 30;
    private void OnCollisionEnter(Collision collision)
    {

        collision.gameObject.SendMessage("GetDamage", damage);
        Destroy(gameObject);
    }
}
