using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;

    [SerializeField] private AudioClip clipCalm;
    [SerializeField] private AudioClip clipStress;
    [SerializeField] private AudioClip clipAttack;

    private AudioSource source1;                // Hlavní hudební ambient
    private AudioSource source2;                // Podkres hudebního ambientu
    private AudioSource source3;                // Sekundární hudební ambient
    private AudioSource source4;                // Hlavní ambient prostředí
    private AudioSource source5;                // Sekundární ambient prostředí

    private bool turnOffSource1 = false;
    private bool turnOffSource2 = false;
    private bool turnOffSource3 = false;
    private bool turnOffSource4 = false;
    private bool turnOffSource5 = false;
    private int calmingTimmer = 0;


    private void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;
    }

    // Use this for initialization
    void Start () {
        source1 = GetComponents<AudioSource>()[0];
        source2 = GetComponents<AudioSource>()[1];
        source3 = GetComponents<AudioSource>()[2];
        source4 = GetComponents<AudioSource>()[3];
        source5 = GetComponents<AudioSource>()[4];
        source1.clip = clipCalm;
        source2.clip = clipStress;
        source3.clip = clipAttack;

        source1.Play();
	}
	
	// Update is called once per frame
	void Update () {

        if (source2.volume <= 0.1f) source2.volume = 0f;
        if (turnOffSource2)
            if (calmingTimmer == 0) AdjustVolume(source2,0,0.01f);
            else calmingTimmer -= 1;
        if (source2.volume == 0f) turnOffSource2 = false;
    }

    public void Attack()
    {
        source2.volume = 1;
        source2.volume = 1;
        source3.volume = 1;
        turnOffSource2 = false;
        source1.Stop();
        source2.Play();
        source3.Play();
    }

    public void StopAttack(int x)
    {
        calmingTimmer = x;
        turnOffSource2 = true;
        source1.Play();
        source3.Stop();
    }

    private void AdjustVolume(AudioSource source, float volume, float time)
    {
        source.volume = Mathf.Lerp(source.volume, volume, time);
        if (source.volume <= 0.1f) source.volume = 0;
    }
}
