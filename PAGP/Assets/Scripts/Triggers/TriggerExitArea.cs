using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExitArea : MonoBehaviour
{
    float waitingTime = 3f;
    float timer = 0f;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == GlobalGameManager.instance.deployedCharacter)
        {
            timer += 1 * Time.deltaTime;
            Debug.Log(timer);

            if(Mathf.Approximately(timer, waitingTime))
            {
                if(MissionGameManager.mgmInstance != null)
                {
                    MissionGameManager.mgmInstance.MGM_EndMisson();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GlobalGameManager.instance.deployedCharacter)
        {
            timer = 0;
        }
    }
}
