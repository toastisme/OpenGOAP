using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System.Linq;

public class Vision : MonoBehaviour
{
    [SerializeField]
    float radius = 5f;

    [SerializeField]
    List<string> possibleResources;

    WorldState worldState;

    public void Setup(ref WorldState worldState){
        this.worldState = worldState;
    }

    void OnTick(){
        OnTickResourceAwareness();
    }

    public void OnTickResourceAwareness(){
        /**
         * Searches in within radius for possibleResources and updates
         * WorldState if they are found
         */

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < possibleResources.Count; i++){
            worldState.boolKeys[$"{possibleResources}Nearby"] = false;
        }
        for (int i = 0; i < hitColliders.Length; i++){
            if (possibleResources.Contains(hitCollider.tag)){
                worldState.boolKeys[$"{possibleResources}Nearby"] = true;
            }
        }
    }

    public List<GameObject> GetAllNearby(string objectTag){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < hitColliders.Length; i++){
            if (hitColliders[i].tag == objectTag){
                objs.Add(hitColliders[i].gameObject);
            }
        }
        return objs;
    }

    public GameObject GetNearest(string objectTag){
        List<GameObject> objs = GetAllNearby(objectTag);
        List<GameObject> sortedList = objs.OrderBy(o=>(Vector3.squareMagnitude(
            o.transform.position-transform.position))).ToList();
        return sortedList[0];
    }

}
