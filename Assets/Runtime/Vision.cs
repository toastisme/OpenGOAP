using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace Sensors{
public class Vision : MonoBehaviour
{

    /**
     * Generic vision class, based on
     * https://github.com/GameDevEducation/AdvancedDeepDive_GOAP_Monolithic/blob/master/Assets/Systems/Sensors/VisionSensor.cs
     */ 


    [SerializeField] LayerMask detectionMask = ~0;
    [Range(1f, 180f)]
    public float fov = 60f;
    float cosFov;
    [Range(1f,500f)]
    public float visionRange = 30f;
    public Color fovColor = new Color(0f, 1f, 0f, 0.1f);


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

            // Not in field of view
            if (Vector3.Dot(vecToDetectable, eyeDirection) < cosFov)
                continue;

            // Has line of sight
            RaycastHit hit;
            if (Physics.Raycast(eyeLocation, vecToDetectable, out hit, 
                                visionRange, detectionMask, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.GetComponentInParent<Detectable>() == detectable){
                    OnSeenDetectable(detectable);
                }
            }
        }

    }

    public void SetOnSeenDetectable(Action<Detectable> func){
        OnSeenDetectable = func;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Vision))]
public class VisionEditor : Editor
{
    /**
     * Draws field of view
     */

    public void OnSceneGUI()
    {
        var vision = target as Vision;

        Vector3 fovStart = Mathf.Cos(-vision.fov * Mathf.Deg2Rad) * vision.transform.forward +
                             Mathf.Sin(-vision.fov * Mathf.Deg2Rad) * vision.transform.right;

        Handles.color = vision.fovColor;
        Handles.DrawSolidArc(
            vision.transform.position, 
            Vector3.up, 
            fovStart, 
            vision.fov * 2f, 
            vision.visionRange
        );        
    }
}
#endif // UNITY_EDITOR
}
