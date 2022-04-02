using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sensors;

[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviour
{
    NavMeshAgent nav;
    float searchRange = 5f;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    void SetDestination(Vector3 target){
        NavMeshHit hit;
        if (NavMesh.SamplePosition(
            target, 
            out hit, 
            searchRange,
            NavMesh.AllAreas)){
                nav.SetDestination(hit.position);
            }
    }

    public void GoTo(Vector3 target){
        SetDestination(target);
    }

    public void GoTo(Vector3 target, float speed){
        nav.speed = speed;
        SetDestination(target);
    }

    public void GoTo(GameObject target){
        GoTo(target.transform.position);
    }

    public void GoTo(GameObject target, float speed){
        GoTo(target:target.transform.position, speed:speed);
    }

    public void GoTo(Detectable target){
        GoTo(target.transform.position);
    }

    public void GoTo(Detectable target, float speed){
        GoTo(target:target.transform.position, speed:speed);
    }

    public bool AtTarget(){
        return nav.remainingDistance < nav.stoppingDistance;
    }

    public Vector3 RandomLocation(float range){
        Vector3 location = transform.position;
        location += Random.Range(-range, range) * Vector3.forward;
        location += Random.Range(-range, range) * Vector3.right;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(location, out hit, searchRange, NavMesh.AllAreas))
            return hit.position;

        return transform.position;
    }



}
