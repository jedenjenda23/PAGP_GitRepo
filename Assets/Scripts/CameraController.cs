using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraTarget;
    public Transform audioListener;
    public float cameraFollowSpeed;
    public Vector3 cameraFollowOffset = new Vector3(0, 10, 10);
    public bool hideObjects;
    public float hideRadius;
    public LayerMask raycastLayer;
    GameObject lastObj;

    RaycastHit hit;
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (cameraTarget) audioListener.position = cameraTarget.position;
        else audioListener.position = transform.position;

        transform.position = Vector3.Slerp(transform.position, cameraTarget.position + cameraFollowOffset, cameraFollowSpeed * Time.deltaTime);
        /*
        if(hideObjects)
        {
            if(Physics.SphereCast(transform.position, hideRadius, -(transform.position - cameraTarget.position), out hit, Vector3.Distance(transform.position, cameraTarget.position), raycastLayer))
            {
                if(lastObj != null && lastObj != hit.collider.gameObject)
                {
                    Debug.Log("1");

                    lastObj.SetActive(true);
                    lastObj = hit.collider.gameObject;
                    lastObj.SetActive(false);
                }
                else
                {
                    Debug.Log("2");

                    lastObj = hit.collider.gameObject;
                    lastObj.SetActive(false);
                }                
            }
            else
            {
                Debug.Log("3");

                if (lastObj != null)
                {
                    Debug.Log("");
                    lastObj.SetActive(true);
                    lastObj = null;
                }
            }

            Debug.DrawRay(transform.position, -(transform.position - cameraTarget.position) * Vector3.Distance(transform.position, cameraTarget.position), Color.red);
        }
        */
    }
}
