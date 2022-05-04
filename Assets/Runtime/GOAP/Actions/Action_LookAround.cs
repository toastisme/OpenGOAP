using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using Sensors;

public class Action_LookAround : GOAPAction
{
    Movement movement;
    Vision vision;
    public override float GetCost(){
        return 0.3f * worldState.GetFloatState("Fatigue");
    }

    Coroutine surveying;
    bool isSurveying=false;

    public override void Setup(){
        base.Setup();
        movement = GetComponent<Movement>();
        vision = GetComponent<Vision>();
    }

    IEnumerator SurveyArea(){
        isSurveying = true;
        movement.ClearTarget();
        Vector3 moveTarget = movement.RandomLocation(vision.visionRange);
        Vector3 moveTargetDir = moveTarget.normalized;
        int numPointsToLookAt = UnityEngine.Random.Range(2, 5);
        float turnSpeed;
        for (int i=0; i<numPointsToLookAt-1; i++){
            Vector3 dir = UnityEngine.Random.Range(0,10) > 5 ? transform.right : -transform.right;
            float scaleFactor = UnityEngine.Random.Range(5f, 20f);
            Vector3 newVec = (dir * scaleFactor).normalized;
            turnSpeed = UnityEngine.Random.Range(.0001f, .003f);
            while(Vector3.Distance(transform.forward, newVec) > .5f){
                Vector3 newDir = Vector3.RotateTowards(transform.forward, newVec, turnSpeed * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
                yield return null;
            }
        }    
        turnSpeed = UnityEngine.Random.Range(.0001f, .003f);
        while(Vector3.Distance(transform.forward, moveTargetDir) > .5f){
            Vector3 newDir = Vector3.RotateTowards(transform.forward, moveTargetDir, turnSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            yield return null;
        }
        isSurveying = false;
        movement.GoTo(movement.RandomLocation(vision.visionRange));
    }

    public override void OnTick(){
        if (!movement.HasTarget() || movement.AtTarget() && !isSurveying){
            surveying = StartCoroutine(SurveyArea());
        }
    }

    public override void OnDeactivate(){
        StopCoroutine(surveying);
        movement.ClearTarget();
        isSurveying = false;
    }

    protected override void SetupActionLayers(){
        base.SetupActionLayers();
        actionLayers.Add("Food");
        actionLayers.Add("Wood");
    }

    protected override void SetupEffects(){
        base.SetupEffects();
        effects["WoodNearby"] = true;
        effects["FoodNearby"] = true;
    }
}
