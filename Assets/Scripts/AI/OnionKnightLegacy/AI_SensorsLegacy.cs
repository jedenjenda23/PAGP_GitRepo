using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AI_SensorsLegacy : MonoBehaviour
{
    /*
    [Header("Target & Player")]
                                                                        public float distanceToPlayer; //for later purposes - senses performance optimisation based on agent's distanceToPlayer
                                                                        public enum targetTypes { None, Player, Distract, Other}
                                                                        public targetTypes targetType;
                                                                        public Transform Target;
    //[HideInInspector]
                                                                        public Vector3 targetPos;
    //[HideInInspector]
                                                                        public Vector3 targetSoundLastPos;
    //[HideInInspector]
                                                                        public Vector3 targetLastPos;

                                                                        public bool seeTarget;
                                                                        public bool hearTarget;
                                                                        public bool targetIsMoving;
    [Header("Attributes")]
                                                                        public bool canHear = true;
                                                                        public bool canSee = true;
                                                                        public LayerMask lineCastLayer;
    [Tooltip("Reaction time of agent in miliseconds")]                  public float reactionTime = 0.200f;
    [Range(0.0f, 360)]
    [Tooltip("Agend Field Of View")]                                    public float agentFov = 100f;
    [Range(0.5f, 100)]
    [Tooltip("Maximum distance agent can see")]                         public float maxVisionRadius = 34f;
    [Range(0.5f, 100)]
    [Tooltip("Maximum distance agent can hear")]                        public float hearingRadius = 14.5f;
    [Range(0.0f, 1.00f)]
                                                                        public float hearingRadiusFallOff = 0.5f;
    [Range(0.5f, 100)]
    [Tooltip("Maximum distance agent can be disturbed in")]             public float attentionRadius = 11f;
    [Range(0.1f, 10)]
    [Tooltip("Certainity of sound position")]                           public float soundPositionUncertainityFactor = 1f;

                                                                        [HideInInspector]
                                                                        public float soundUncertainity;                                                

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Target != null)
        {
            targetPos = Target.position;
            CompareTargetTag();

            if (targetType == targetTypes.Player)
            {
                StartCoroutine(CheckTargetMovement(Target));
            }

            if (canSee) seeTarget = LineOfSight(Target);
            if (canHear) hearTarget = HearObject(Target);

                     

            if (seeTarget || hearTarget)
            {
                StartCoroutine(TargetUpdateLastPositions());
            }
        }

        else
        {
            targetType = targetTypes.None;
        }
        
    }

    Vector3 FindSoundPosition(Vector3 position)
    {
        float unc = soundUncertainity * soundPositionUncertainityFactor;

        //position.y = position.y + Random.Range(-unc, unc);
        position.x = position.x + Random.Range(-unc, unc);
        position.z = position.z + Random.Range(-unc, unc);

        return position;
    }
    void CompareTargetTag()
    {
        if (Target.CompareTag("Player"))
        {
            targetType = targetTypes.Player;

        }
        else if (Target.CompareTag("Distract")) targetType = targetTypes.Distract;
        else targetType = targetTypes.Other;
    }

    bool HearObject(Transform soundObject)
    {
        float dist = (DistanceToObject(soundObject));

        if (soundObject != Target)
        {
            if (dist < hearingRadius)
            {
                if (LineOfSight(soundObject))
                {
                    soundUncertainity = 0f;
                }

                else
                {
                    if (dist < hearingRadius * hearingRadiusFallOff)
                    {
                        soundUncertainity = 0.5f;
                    }

                    if (dist > hearingRadius * hearingRadiusFallOff)
                    {
                        soundUncertainity = 1f;
                    }
                }
                return true;
            }

            else
            {
                return false;
            }
        }

        else
        {
             if (dist < hearingRadius)
            {
                if (seeTarget)
                {
                    soundUncertainity = 0f;
                    return true;
                }

                else if (!seeTarget && targetIsMoving)
                {

                    if (dist < hearingRadius * hearingRadiusFallOff)
                    {
                        soundUncertainity = 0.5f;
                    }

                    else if (dist > hearingRadius * hearingRadiusFallOff)
                    {
                        soundUncertainity = 1f;
                    }

                    return true;
                }
                else return false;
            }

            else
            {
                return false;
            }
        }
       
    }

    bool LineOfSight(Transform Object)
    {
        if (Target != null && DistanceToObject(Target) < maxVisionRadius)
        {
            if (AngleToObject(Object) < agentFov / 2)
            {
                if (Physics.Linecast(transform.position, Object.position, lineCastLayer))
                {
                    //Debug.DrawLine(transform.position, Target.position, Color.white);
                    return false;
                }
                else
                {
                    //Debug.DrawLine(transform.position, Target.position, Color.green);
                    return true;
                }
            }

            else
            {
               // Debug.DrawLine(transform.position, Target.position, Color.white);
                return false;
            } 
        }
        
        else
        {
            return false;
        }
    }

    public float DistanceToObject(Transform DistantObject)
    {
        float distance = Vector3.Distance(transform.position, DistantObject.position);
        return distance;
    }

    IEnumerator TargetUpdateLastPositions()
    {
        yield return new WaitForSeconds(reactionTime);
        if (seeTarget) targetLastPos = Target.position;
        if (hearTarget) targetSoundLastPos = FindSoundPosition(targetPos);
    }


    private IEnumerator CheckTargetMovement(Transform Object)
    {
        if(targetType == targetTypes.Player)
        {
            targetIsMoving = Target.GetComponent<PlayerController>().moving;
        }

        else
        {
            Vector3 targetPos1 = Object.position;
            yield return new WaitForSeconds(0.1f);
            Vector3 targetPos2 = Object.position;

            if (targetPos1.x != targetPos2.x || targetPos1.y != targetPos2.y || targetPos1.z != targetPos2.z)
            {
                targetIsMoving = true;
            }
            else targetIsMoving = false;
        }
    }
    float AngleToObject(Transform Object)
    {
        Vector3 dir = transform.position - Object.position;
        float angle = Vector3.Angle(-dir, transform.forward);
        return  angle;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        Vector3 normal = Vector3.zero;
        normal.y = 1f;

        pos.y = transform.position.y - 1f;
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(pos, normal, attentionRadius);

        Gizmos.color = Color.yellow;

        if(canHear)
        {
            Handles.color = Color.magenta;
            Handles.DrawWireDisc(pos, normal, hearingRadius);
            Handles.DrawWireDisc(pos, normal, hearingRadius * hearingRadiusFallOff);
        }

        if(canSee)
        {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(pos, normal, maxVisionRadius);

            //FieldOfView vizualization        
            float halfFov = agentFov / 2;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFov, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(halfFov, Vector3.up);
            Vector3 leftRayDirection = leftRayRotation * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;
            Gizmos.DrawRay(pos, leftRayDirection * maxVisionRadius);
            Gizmos.DrawRay(pos, rightRayDirection * maxVisionRadius);
        }
        

        Handles.color = Color.white;

        if (Target != null)
        {
            if (targetLastPos != Vector3.zero)
            {
                Gizmos.DrawWireSphere(targetLastPos, 0.3f);
                Handles.Label(targetLastPos, ("targetLastPos: " + targetLastPos));
                Handles.DrawDottedLine(transform.position, targetLastPos, 1f);
            }

            if (canSee)
            {
                if (LineOfSight(Target))
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(transform.position, Target.position);
                }

                else
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(transform.position, Target.position);
                }

            }

            if(canHear)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(targetSoundLastPos, 0.5f);
            }
        }

    }
#endif
*/
}
