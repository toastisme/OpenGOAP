using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    NavMeshAgent nav;
    [SerializeField]
    float minTargetDistance=1f;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    public void GoTo(Vector3 target){
        nav.SetDesination(target);
    }

    public void GoTo(Vector3 target, float speed){
        nav.speed = speed;
        nav.SetDesination(target);
    }

    public void GoTo(GameObject target){
        GoTo(target.transform.position);
    }

    public void GoTo(GameObject target, float speed){
        GoTo(taget:target.transform.position, speed:speed);
    }

    public bool AtTarget(){
        return nav.remainingDistance < minTargetDistance;
    }



}
