using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AI_Sensors))]
[RequireComponent(typeof(Rigidbody))]

public class GC_Monster : GameCharacter
{
    public enum States { Patrol, Wander, Attack, Idle, Chase }

    AI_Sensors sensors;
    NavMeshAgent navAgent;
    Rigidbody rb;

    [Header("GC_Monster")]
    public States myState;
    public States myDefaultState = States.Wander;

    public float relaxedSpeed = 1f;
    float initSpeed;

    [Range(0.0f, 100)]
    public float damageDealRadius = 2f;

    public float damageDealRate = 1f;
    public float recoveryAfterCharge = 1f;
    public float chargeRate = 2f;
    public float chargeForce = 200f;
    public float chargeAngle = 60f;

    float nextCharge;
    float lastCharge;
    float nextDamageDeal;
    float lastDamageDeal;

    Vector3 initPos;

    void Awake()
    {
        sensors = GetComponent<AI_Sensors>();
        navAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        charAttributes = GetComponent<CharacterAttributes>();
        initSpeed = navAgent.speed;
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        sensors.SetTargetNearestEnemy();
        StateMachineUpdate();
    }

    void StateMachineUpdate()
    {
        switch(myState)
        {
            case States.Idle:
                myState = myDefaultState;
                navAgent.speed = relaxedSpeed;
                break;
            case States.Chase:
                Chase();
                navAgent.speed = initSpeed;
                break;

            case States.Wander:
                Wander();
                navAgent.speed = relaxedSpeed;
                break;
        }
    }

    void Wander()
    {
        if (sensors.target != null)
        {
            myState = States.Chase;
        }

        else
        {

            if (navAgent.remainingDistance < 3f)
            {
                    Vector3 newPos = initPos;
                    newPos.x += Random.Range(-10f, 10f);
                    newPos.z += Random.Range(-10f, 10f);
                    navAgent.SetDestination(newPos); // send agent to random position
            }
        }

    }

    void Chase()
    {
        if (sensors.target == null)
        {
            myState = States.Idle;
        }

        else
        {

            if (Time.time > lastCharge + recoveryAfterCharge &&
             Time.time > lastDamageDeal + damageDealRate &&
             GetDistance(transform.position, sensors.target.position) < sensors.maxVisionRadius)
            {
                if (sensors.LineOfSight(sensors.target))
                {
                    if (GetDistance(transform.position, sensors.target.position) > 1)
                    {
                        navAgent.SetDestination(sensors.target.position);
                    }

                    if (sensors.AngleToObject(sensors.target) < chargeAngle && GetDistance(transform.position, sensors.target.position) > 1.1f && GetDistance(transform.position, sensors.target.position) < 3f)
                    {
                        Charge();
                    }

                    if (sensors.AngleToObject(sensors.target) < sensors.agentFov && GetDistance(transform.position, sensors.target.position) < damageDealRadius && Time.time > nextDamageDeal)
                    {
                        nextDamageDeal = Time.time + damageDealRate;
                        lastDamageDeal = Time.time;
                        DealDamage(sensors.target);
                    }

                    sensors.curTargetMemoryTime = 0;
                }


                else
                {
                    sensors.curTargetMemoryTime += 1 * Time.deltaTime;
                    navAgent.SetDestination(sensors.targetLastPos);
                }


                if (sensors.curTargetMemoryTime >= sensors.maxTargetMemoryTime)
                {
                    sensors.target = null;
                    sensors.curTargetMemoryTime = 0;
                }
            }
        }
    }


    public void Charge()
    {
        if (Time.time > nextCharge)
        {
        Debug.Log("Charging");
        Vector3 chargeVector = transform.TransformDirection(Vector3.forward);

            if (GetDistance(transform.position, sensors.target.position) > 2) rb.AddForce(chargeVector * chargeForce * 100);
            else rb.AddForce(chargeVector * chargeForce * 50);

        nextCharge = Time.time + chargeRate;
        lastCharge = Time.time;

        nextDamageDeal = Time.time;
        }

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        //FieldOfView vizualization        
        float halfFov = chargeAngle;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFov, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFov, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(pos, leftRayDirection * 6);
        Gizmos.DrawRay(pos, rightRayDirection * 6);

        pos.y = transform.position.y - 1f;
        Handles.color = Color.red;
        Handles.DrawWireDisc(pos, Vector3.up, damageDealRadius);
    }
#endif

}
