using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float liveSpan = 20f;
    [SerializeField]
    int damage = 30;

    private void Start()
    {
        Destroy(gameObject, liveSpan);
    }

    private void OnCollisionEnter(Collision collision)
    {

        collision.gameObject.SendMessage("GetDamage", damage);
        Destroy(gameObject);
    }
}
