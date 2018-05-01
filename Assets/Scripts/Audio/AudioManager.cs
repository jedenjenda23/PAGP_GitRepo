using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;

    [SerializeField] AudioSource[] Sources;

    [SerializeField] private AudioClip ambientMain;
    [SerializeField] private AudioClip ambientStress;
    [SerializeField] private AudioClip ambientAttack;
    [SerializeField] private AudioClip atmosphereMain;
    [SerializeField] private AudioClip atmosphereSecondary;

    [SerializeField] private float transitionTimeMain = 0.04f;
    [SerializeField] private float transitionTimeStress = 0.04f;
    [SerializeField] private float transitionTimeAttack = 0.04f;
    [SerializeField] private float[] transitionTimes;

    [HideInInspector]
    public bool attackMusicPlaying;
    private bool newLevel = true;

    //private AudioSource source1;                // Hlavní hudební ambient
    //private AudioSource source2;                // Podkres hudebního ambientu
    //private AudioSource source3;                // Sekundární hudební ambient
    //private AudioSource source4;                // Hlavní ambient prostředí
    //private AudioSource source5;                // Sekundární ambient prostředí

    [SerializeField] private bool[] activeSources = new bool[] { true, false, false, false, false };

    private void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;
    }

    void Start() {
        transitionTimes = new float[] {transitionTimeMain, transitionTimeStress , transitionTimeAttack, transitionTimeMain, transitionTimeMain};
        Sources = GetComponents<AudioSource>();
        Sources[0].clip = ambientMain;
        Sources[1].clip = ambientStress;
        Sources[2].clip = ambientAttack;
        Sources[3].clip = atmosphereMain;
        Sources[4].clip = atmosphereSecondary;
    }

    // Update is called once per frame
    void Update() {
        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 6) ChangeVolumes();
    }

    public void PlayAudio()
    {
        Sources[0].Play();
        Sources[3].Play();
        Sources[5].Play();
        //if ((SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 6) && newLevel)
        //{
        //    newLevel = false;
        //    Sources[0].Play();
        //    Sources[3].Play();
        //    Sources[5].Play();
        //}
    }

    public void StopAudio()
    {
        newLevel = false;
        foreach(AudioSource source in Sources)
        {
            source.Stop();
        }

        //Sources[0].Stop();
        //Sources[1].Stop();
        //Sources[2].Stop();
        //Sources[3].Stop();
        //Sources[4].Stop();
        //Sources[5].Stop();
    }

    private void ChangeVolumes()
    {
        for (int i = 0; i < 5; i++)
        {
            AdjustVolume(Sources[i],activeSources[i],transitionTimes[i]);
        }
    }

    public void StartAttackTheme()
    {
        Sources[0].Stop();
        Sources[1].Stop();
        Sources[1].Play();
        Sources[2].Play();
        activeSources[0] = false;
        activeSources[1] = true;
        activeSources[2] = true;

        attackMusicPlaying = true;
    }

    public void StopAttackTheme()
    {
        Sources[0].Play();
        activeSources[0] = true;
        activeSources[1] = false;
        activeSources[2] = false;

        attackMusicPlaying = false;

        Debug.Log("stopAttackTheme");
    }

    private void AdjustVolume(AudioSource source, bool turnUp, float time)
    {
        //if ((source == Sources[2] && turnUp) || (source == Sources[3] && turnUp)) source.volume = 1;

        if (turnUp) source.volume = Mathf.Lerp(source.volume, 1, time);
        else source.volume = Mathf.Lerp(source.volume, 0, time);


        //if (source.volume <= 0.01f) source.volume = 0;
        //if (source.volume >= 0.98f) source.volume = 1;
    }
}
