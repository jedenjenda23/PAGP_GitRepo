using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnTrigger : MonoBehaviour
{
    public GameObject child;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("2");

            child.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("2");
            child.SetActive(true);
        }
    }
}
