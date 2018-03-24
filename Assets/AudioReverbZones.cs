using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReverbZones : MonoBehaviour {
    public GameObject Inside;
    public GameObject Outside;

    public void ToInside()
    {
        Inside.SetActive(true);
        Outside.SetActive(false);
    }

    public void ToOutside()
    {
        Inside.SetActive(false);
        Outside.SetActive(true);
    }
}
