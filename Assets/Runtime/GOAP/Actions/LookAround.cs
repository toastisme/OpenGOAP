using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using Sensors;

public class LookAround : GOAPAction
{
    Movement movement;
    Vision vision;
    public override float GetCost(){
        return 0.0f;
    }

    Coroutine surveying;

    Vector3 moveTarget;

    public override void Setup(){
        base.Setup();
        movement = GetComponent<Movement>();
        vision = GetComponent<Vision>();
        effects["WoodNearby"] = true;
    }

    IEnumerator SurveyArea(){
        int numPointsToLookAt = UnityEngine.Random.Range(2, 5);
        for (int i=0; i<numPointsToLookAt; i++){
            Vector3 dir = UnityEngine.Random.Range(0,10) > 5 ? transform.right : -transform.right;
            float scaleFactor = UnityEngine.Random.Range(5f, 20f);
            Vector3 newVec = (dir * scaleFactor).normalized;
            float turnSpeed = UnityEngine.Random.Range(.01f, 3f);
            while(Vector3.Distance(transform.forward, newVec) > .5f){
                Vector3 newDir = Vector3.RotateTowards(transform.forward, newVec, turnSpeed * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
                yield return null;
            }
        }        
    }

    public override void OnTick(){
        if (PreconditionsSatisfied()){
            if (moveTarget == null || movement.AtTarget()){
                surveying = StartCoroutine(SurveyArea());
                moveTarget = movement.RandomLocation(vision.visionRange);
                movement.GoTo(moveTarget);
            }
        }
        else{
            StopAction();
            return;
        }
    }

    public override void OnDeactivated(){
        StopCoroutine(surveying);
        movement.ClearTarget();
    }


}
