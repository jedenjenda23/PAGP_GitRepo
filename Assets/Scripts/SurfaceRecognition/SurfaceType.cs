using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PAGP/SurfaceType")]

public class SurfaceType : ScriptableObject
{


    public float humanoidMovementMultiplier = 1f;
    public float monsterMovementMultiplier = 1f;
    public AudioClip[] surfaceStepSounds;
    public AudioClip[] surfaceImpactSounds;
    public GameObject[] surfaceStepParticles;
    public GameObject[] surfaceImpactParticles;


    public AudioClip GetRandomStepSound()
    {
        return surfaceStepSounds[randomIndex(surfaceStepSounds.Length)];
    }
    public AudioClip GetRandomImpactSound()
    {
        return surfaceImpactSounds[randomIndex(surfaceImpactSounds.Length)];
    }
    public GameObject GetRandomStepParticle()
    {
        return surfaceStepParticles[randomIndex(surfaceStepParticles.Length)];
    }
    public GameObject GetRandomImpactParticle()
    {
        return surfaceImpactParticles[randomIndex(surfaceImpactParticles.Length)];
    }

    int randomIndex(int max)
    {
        return Random.Range(0, max);
    }

    public float GetHumanoidMovementMultiplier()
    {
        return humanoidMovementMultiplier;
    }

    public float GetMonsterMovementMultiplier()
    {
        return monsterMovementMultiplier;
    }
}
