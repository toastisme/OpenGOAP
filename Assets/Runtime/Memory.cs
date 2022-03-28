using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sensors{
public class Memory : MonoBehaviour
{
    Dictionary<GameObject, Vector3> objsInMemory = new Dictionary<GameObject, Vector3>();

    public void Record(GameObject obj){
        objsInMemory[obj] = obj.transform.position;
    }

    public GameObject RememberNearest(Type objType){
        float minD = -1f;
        GameObject nearest = null;
        foreach (var kvp in objsInMemory){
            if (kvp.Key.GetType() == objType){
                float d = Vector3.Distance(transform.position, kvp.Value);
                if (d < minD || minD < 0f){
                    minD =  d;
                    nearest = kvp.Key;
                } 
            }
        }
        return nearest;
    }
}
}
