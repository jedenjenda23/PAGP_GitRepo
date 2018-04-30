using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    int damage;
    float frequency;
    float dealingTime;
    float nextDeal;

    float curDealingTime;

    public void SetDamageOverTime(int newDamage, float newFrequency, float newDealingTime)
    {
        damage = newDamage;
        frequency = newFrequency;
        dealingTime = newDealingTime;
    }

    private void Update()
    {
        if (curDealingTime < dealingTime)
        {
            dealingTime += Time.deltaTime;

            if (Time.time > nextDeal)
            {
                nextDeal = Time.time + frequency;
                SendMessage("GetDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }

        else Destroy(this);
    }
}
