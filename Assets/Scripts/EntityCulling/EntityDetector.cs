using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDetector : MonoBehaviour
{
    [Range(0,100)]
    public float entityHidingDistance = 30f;
    [Range(0, 20)]
    public float entityPreserveDistance = 5f;
    [Range(0,360)]
    public float fieldOfView = 120f;

    public LayerMask detectLayer;
    public LayerMask fovLayer;

    void Update()
    {
        HideUnhideEntities(DetectNearbyEntities());
    }

    void HideUnhideEntities(List<Transform>entities)
    {
        foreach (Transform entity in entities)
        {
            if (entity.GetComponent<EntityHider>())
            {
                if (LineOfSight(entity))
                {
                    Debug.DrawLine(transform.position, entity.position, Color.blue);
                    //unhide
                    entity.GetComponent<EntityHider>().ToggleShow(true);
                }

                else
                {
                    Debug.DrawLine(transform.position, entity.position, Color.magenta);
                    //hide
                   entity.GetComponent<EntityHider>().ToggleShow(false);
                }
            }

            else
            {
            Debug.DrawLine(transform.position, entity.position, Color.yellow);
            }

        }
    }

    List<Transform> DetectNearbyEntities()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, entityHidingDistance + 5f, detectLayer);
        List<Transform> relevant = new List<Transform>();

        foreach (Collider col in colliders)
        {
            if (col.gameObject != gameObject) relevant.Add(col.transform);
        }
        return relevant;
    }

    public float AngleToObject(Transform Object)
    {
        Vector3 dir = transform.position - Object.position;
        float angle = Vector3.Angle(-dir, transform.forward);
        return angle;
    }

    public bool LineOfSight(Transform Object)
    {
            if (Vector3.Distance(transform.position, Object.position) < entityHidingDistance)
            {
                if (Vector3.Distance(transform.position, Object.position) > entityPreserveDistance)
                {
                    if (AngleToObject(Object) < fieldOfView / 2)
                    {
                        if (Physics.Linecast(transform.position, Object.position, fovLayer))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    else return false;
                }
                else return true;

            }           

            else
            {
                return false;
            }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        UnityEditor.Handles.color = Color.gray;

        Vector3 pos = transform.position;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, entityHidingDistance);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, entityPreserveDistance);

        float halfFov = fieldOfView / 2;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFov, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFov, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(pos, leftRayDirection * entityHidingDistance);
        Gizmos.DrawRay(pos, rightRayDirection * entityHidingDistance);
    }
#endif
}
