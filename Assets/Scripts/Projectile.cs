using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float liveSpan = 20f;
    [SerializeField]
    int damage = 30;

    [HideInInspector]
    public Transform shooter;

    private void Start()
    {
        Destroy(gameObject, liveSpan);
    }

    private void OnCollisionEnter(Collision collision)
    {

        collision.gameObject.SendMessage("GetDamage", damage);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<AI_Sensors>() != null)
        {
            other.gameObject.GetComponent<AI_Sensors>().target = shooter;
        }
    }
}
