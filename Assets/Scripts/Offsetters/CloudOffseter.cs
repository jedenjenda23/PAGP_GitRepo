using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudOffseter : MonoBehaviour
{
    public float updateTime = 50f;
    float nextUpdate;

    public float cloudMovementSpeed = 0.0009f;
    public float cloudScalingSpeed = 0.01f;

    public float cloudSizeRandomMultiplier = 2f;
    Light myLight;
    Vector3 initPos;
    float initCloudSize;

    float newCloudSize;
    Vector3 newPos;

    // Use this for initialization
    void Awake ()
    {
        myLight = GetComponent<Light>();
        initPos = transform.position;
        initCloudSize = myLight.cookieSize;
	}
	
	// Update is called once per frame
	void Update ()
    {
       if(Time.time > nextUpdate)
        {
            nextUpdate = Time.time + updateTime;

            newPos = initPos;
            newPos.x += Random.Range(-1000, 1000);
            newPos.z += Random.Range(-1000, 1000);

            newCloudSize = initCloudSize + Random.Range(-initCloudSize * cloudSizeRandomMultiplier, initCloudSize * cloudSizeRandomMultiplier);
        }

        //lerp pos
        transform.position = Vector3.Lerp(transform.position, newPos, cloudMovementSpeed * Time.deltaTime);
        //lerp size
        myLight.cookieSize = Mathf.Lerp(initCloudSize, newCloudSize, cloudScalingSpeed * Time.deltaTime);
    }
}
