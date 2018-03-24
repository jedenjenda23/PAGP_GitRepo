using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCharacter : MonoBehaviour {
    public GameObject Anim;                 // Animator refernce
    public AudioClip[] soundsConcrete;      // List of walking sounds
    public AudioClip[] soundsWood;          
    public AudioClip[] soundsGravel;        
    public AudioClip[] soundsRubble;        
    public AudioClip[] soundsMud;           
    public AudioClip[] soundsGrass;         
    public AudioClip[] soundsDirt;          
    public AudioClip[] soundsWater;         
    public AudioClip[] soundsMetal;         
    public AudioClip dashSound;             // Dash sound
    public AudioSource source;

    [Range(0.5f,3f)]
    public float pitchVariation;
    public float volumeVariation;

    private Animator playerAnimator;
    private float currentRightFoot;
    private float lastFrameRightFoot;
    private float currentLeftFoot;
    private float lastFrameLeftFoot;

    private void Start()
    {
        playerAnimator = Anim.GetComponent<Animator>();
    }

    private void Update()
    {
        currentRightFoot = playerAnimator.GetFloat("RightFoot");
        currentLeftFoot = playerAnimator.GetFloat("LeftFoot");

        if (currentRightFoot < 0 && lastFrameRightFoot > 0)
        {
            PlayFootstep("defaul");
            source.pitch = Random.Range(0.85f, 1.4f);
        }

        if (currentLeftFoot < 0 && lastFrameLeftFoot > 0)
        {
            PlayFootstep("default");
            source.pitch = Random.Range(0.85f, 1.4f);
        }

        lastFrameRightFoot = currentRightFoot;
        lastFrameLeftFoot = currentLeftFoot;
    }

    public void PlayFootstep(string material)
    {
        switch (material)
        {
            case "Concrete":
                source.PlayOneShot(soundsConcrete[Random.Range(0, soundsConcrete.Length)]);
                break;
            case "Wood":
                source.PlayOneShot(soundsConcrete[Random.Range(0, soundsConcrete.Length)]);
                break;
            case "Gravel":
                source.PlayOneShot(soundsConcrete[Random.Range(0, soundsConcrete.Length)]);
                break;
            case "Rubble":
                source.PlayOneShot(soundsConcrete[Random.Range(0, soundsConcrete.Length)]);
                break;
            case "Mud":
                source.PlayOneShot(soundsConcrete[Random.Range(0, soundsConcrete.Length)]);
                break;
            case "Grass":
                source.PlayOneShot(soundsConcrete[Random.Range(0, soundsConcrete.Length)]);
                break;
            case "Dirt":
                source.PlayOneShot(soundsConcrete[Random.Range(0, soundsConcrete.Length)]);
                break;
            case "Water":
                source.PlayOneShot(soundsConcrete[Random.Range(0, soundsConcrete.Length)]);
                break;
            case "Metal":
                source.PlayOneShot(soundsConcrete[Random.Range(0, soundsConcrete.Length)]);
                break;
            default:
                source.PlayOneShot(soundsConcrete[Random.Range(0, soundsConcrete.Length)]);
                break;
        } 
    }

    public void PlayDash()
    {
        Debug.Log("Dash");
        source.pitch = 1.2f;
        source.PlayOneShot(dashSound);
    }
}
