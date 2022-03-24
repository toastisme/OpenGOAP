using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System.Linq;
using System;

public class Vision : MonoBehaviour
{

    /**
     * Generic vision class, based on
     * https://github.com/GameDevEducation/AdvancedDeepDive_GOAP_Monolithic/blob/master/Assets/Systems/Sensors/VisionSensor.cs
     */ 


    [SerializeField] LayerMask detectionMask = ~0;
    [SerializeField] float fov = 60f;
    float cosFov;
    [SerializeField] float visionRange = 30f;
    [SerializeField] Color fovColor = new Color(1f, 0f, 0f, 0.25f);


    Action<Detectable> OnSeenDetectable;
    Vector3 eyeLocation => transform.position;
    Vector3 eyeDirection => transform.forward;

    void Awake(){
        cosFov = Mathf.Cos(fov * Mathf.Deg2Rad);
    }


    void Update()
    {
        UpdateVision();
    }

    void UpdateVision(){

        /**
         * For any Detectable in range call OnSeenDetectable
         */ 

        if (DetectableManager.Instance == null || OnSeenDetectable == null){return;}

        for (int i = 0; i < DetectableManager.Instance.Detectables.Count; ++i)
        {
            var detectable = DetectableManager.Instance.Detectables[i];

            // Is self
            if (detectable.gameObject == this.gameObject)
                continue;

            var vecToDetectable = detectable.transform.position - eyeLocation;

            // Not in range
            if (vecToDetectable.sqrMagnitude > (visionRange * visionRange))
                continue;

            vecToDetectable.Normalize();

            // Not in vision cone
            if (Vector3.Dot(vecToDetectable, eyeDirection) < cosFov)
                continue;

            // Has line of sight
            RaycastHit hit;
            if (Physics.Raycast(eyeLocation, vecToDetectable, out hit, 
                                visionRange, detectionMask, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.GetComponentInParent<Detectable>() == detectable)
                    OnSeenDetectable(detectable);
            }
        }

    }

    public void SetOnSeenDetectable(Action<Detectable> func){
        OnSeenDetectable = func;
    }
}
