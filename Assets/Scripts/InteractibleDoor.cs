using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleDoor : MonoBehaviour
{
    public GameObject myUI;
    bool open;
    Animator animator;
    public AudioClip soundDoorOpen;
    public AudioClip soundDoorClose;
    public AudioSource source;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Open", open);
        myUI.SetActive(false);
    }

    public void DoorToggleOpen()
    {
        open = !open;
        animator.SetBool("Open", open);
        if (open)
        {
            source.PlayOneShot(soundDoorOpen);
        }
        else
        {
            source.PlayOneShot(soundDoorClose);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) myUI.SetActive(true);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) myUI.SetActive(false);
    }
}
