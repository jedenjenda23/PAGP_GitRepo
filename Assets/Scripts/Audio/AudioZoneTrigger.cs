using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioZoneTrigger : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        transform.parent.transform.parent.GetComponent<AudioReverbZones>().ToInside();
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.transform.parent.GetComponent<AudioReverbZones>().ToOutside();
    }
}
