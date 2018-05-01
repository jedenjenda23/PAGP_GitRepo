using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.instance.StartAttackTheme();
    }

    private void OnTriggerExit(Collider other)
    {
        AudioManager.instance.StopAttackTheme();
    }
}
