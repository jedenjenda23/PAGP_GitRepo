using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareDot : MonoBehaviour
{
    public GameObject particle;
    public int damage = 10;
    public float burningTime = 5f;
    public float damageFrequency = 0.5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<GameCharacter>())
        {
            collision.collider.gameObject.AddComponent<DamageOverTime>();
            collision.collider.GetComponent<DamageOverTime>().SetDamageOverTime(damage, damageFrequency, burningTime);

            GameObject newParticleEffect = Instantiate(particle);
            if(collision.collider.GetComponent<GameCharacter>().effectsOrigin!= null) newParticleEffect.transform.SetParent(collision.collider.GetComponent<GameCharacter>().effectsOrigin);
            else newParticleEffect.transform.SetParent(collision.collider.transform);

            newParticleEffect.transform.localPosition = Vector3.zero;

            Debug.Log("fire" + newParticleEffect);
            Destroy(newParticleEffect, burningTime);
            Destroy(collision.collider.GetComponent<DamageOverTime>(), burningTime);
        }
    }
}
