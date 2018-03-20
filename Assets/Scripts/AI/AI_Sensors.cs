using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AI_Sensors : MonoBehaviour
{
    public LayerMask detectLayer;
    public LayerMask linecastLayer;

    public Transform target;
    public bool seeTarget;
    public Vector3 targetLastPos;

    [Tooltip("Reaction time of agent in miliseconds")] public float reactionTime = 0.200f;
    [Range(0.0f, 360)]
    [Tooltip("Agend Field Of View")]
    public float agentFov = 100f;
    [Range(0.5f, 100)]
    [Tooltip("Maximum distance agent can see")]
    public float maxVisionRadius = 34f;
    [Range(0.5f, 100)]
    [Tooltip("Maximum distance agent can see everything regardless FOV")]
    public float nearDetectionRadius = 2f;
    [Range(0.5f, 100)]
    [Tooltip("Maximum distance agent can detect other entities")]
    public float detectEntitiesRadius = 34f;

    CharacterAttributes charAtributtes;


    [SerializeField]
    List<Transform> relevantTargets;
    [SerializeField]
    List<Transform> enemyTargets;
    [SerializeField]
    Transform closestRelevantTarget;
    [SerializeField]
    Transform closestEnemy;

    [Tooltip("Target tracking memory")]
    public float maxTargetMemoryTime = 5f;
    public float curTargetMemoryTime;

    void Awake()
    {
        charAtributtes = GetComponent<CharacterAttributes>();
    }

    // Update is called once per frame
    void Update()
    {
        relevantTargets = DetectNearbyEntities();
        enemyTargets = FilterNearbyEnemies();

        if(target != null)
        {
            if(curTargetMemoryTime < maxTargetMemoryTime * 0.5f)
            {
                targetLastPos = target.position;
            }
        }
    }

    public Transform GetNearestTarget()
    {
        relevantTargets = DetectNearbyEntities();
        if (relevantTargets.Count > 1) closestRelevantTarget = GetClosestRelevantTarget();
        else closestRelevantTarget = null;

        return closestRelevantTarget;
        /*
        if (target == null)
        {
            //target = closestRelevantTarget;
        }

        else
        {
            seeTarget = LineOfSight(target);
            if (LineOfSight(target))
            {
                Debug.DrawLine(transform.position, targetPos, Color.red);
                targetPos = target.position;
                targetLastPos = targetPos;
            }
        }
        */
    }

    public void SetTargetNearestEnemy()
    {
       if(closestEnemy != null && LineOfSight(closestEnemy))
        {
            target = closestEnemy;
        }
    }

    List<Transform> FilterNearbyEnemies()
    {
        float distance = 999.9f;


        List<Transform> enemies = new List<Transform>();

        foreach(Transform entity in relevantTargets)
        {
            if (entity.GetComponent<CharacterAttributes>() != null)
            {
                factions colFaction = entity.GetComponent<CharacterAttributes>().characterPreset.GetFaction();
                foreach (factions rivalFaction in charAtributtes.characterPreset.GetRivalFactions())
                {
                    if (rivalFaction == colFaction && LineOfSight(entity))
                    {
                        enemies.Add(entity);
                        float dist = Vector3.Distance(transform.position, entity.position);

                        if (dist < distance)
                        {
                            distance = dist;
                            closestEnemy = entity;
                        }
                    }
                }
            }            
        }

        return enemies;
    }
    List<Transform> DetectNearbyEntities()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectEntitiesRadius, detectLayer);
        List<Transform> relevant = new List<Transform>();

        foreach (Collider col in colliders)
        {
            if (col.gameObject != gameObject) relevant.Add(col.transform);            
        }
        return relevant;
    }

    Transform GetClosestRelevantTarget()
    {
        Transform closest = null;
        float curDist = 999.9f;

        foreach (Transform cur in relevantTargets)
        {
            if (LineOfSight(cur) && GetDistance(transform.position, cur.position) < curDist && cur.gameObject != gameObject)
            {
                closest = cur;
                curDist = GetDistance(transform.position, cur.position);
            }
        }

        return closest;
    }

    public float AngleToObject(Transform Object)
    {
        Vector3 dir = transform.position - Object.position;
        float angle = Vector3.Angle(-dir, transform.forward);
        return angle;
    }
    float GetDistance(Vector3 pos1, Vector3 pos2)
    {
        return Vector3.Distance(pos1, pos2);
    }
    public bool LineOfSight(Transform Object)
    {
        if (GetDistance(transform.position, Object.position) < maxVisionRadius)
        {
            if (GetDistance(transform.position, Object.position) < nearDetectionRadius)
            {
                if (Physics.Linecast(transform.position, Object.position, linecastLayer))
                {
                    return false;
                }

                else
                {
                    return true;
                }
            }

            else if (AngleToObject(Object) < agentFov / 2)
            {
                if (Physics.Linecast(transform.position, Object.position, linecastLayer))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        //vis close entities
        for (int i = 0; i < relevantTargets.Count; i++)
        {
            if (relevantTargets[i] == closestRelevantTarget) Gizmos.color = Color.red;
            else Gizmos.color = Color.white;

            Gizmos.DrawLine(transform.position, relevantTargets[i].position);
        }

        for (int i = 0; i < enemyTargets.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, enemyTargets[i].position);
        }

        if (closestEnemy != null && LineOfSight(closestEnemy))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(closestEnemy.position, 0.5f);
        }

        if(targetLastPos != Vector3.zero)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(targetLastPos, 0.8f);
        }

        Vector3 pos = transform.position;
        Vector3 normal = Vector3.zero;
        normal.y = 1f;

        pos.y = transform.position.y - 1f;

        Gizmos.color = Color.yellow;

            Handles.color = Color.white;
            Handles.DrawWireDisc(pos, normal, detectEntitiesRadius);

            Handles.color = Color.yellow;
            Handles.DrawWireDisc(pos, normal, maxVisionRadius);
        
            Handles.color = Color.magenta;
            Handles.DrawWireDisc(pos, normal, nearDetectionRadius);

        //FieldOfView vizualization        
        float halfFov = agentFov / 2;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFov, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(halfFov, Vector3.up);
            Vector3 leftRayDirection = leftRayRotation * transform.forward;
            Vector3 rightRayDirection = rightRayRotation * transform.forward;
            Gizmos.DrawRay(pos, leftRayDirection * maxVisionRadius);
            Gizmos.DrawRay(pos, rightRayDirection * maxVisionRadius);        
    }
#endif
}
