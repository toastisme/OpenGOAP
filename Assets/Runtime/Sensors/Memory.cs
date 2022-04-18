using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GOAP;

namespace Sensors{
public class Memory : MonoBehaviour
{
    Dictionary<Detectable, Vector3> objsInMemory = new Dictionary<Detectable, Vector3>();

    public void Record(Detectable obj){
        objsInMemory[obj] = obj.transform.position;
    }

    public Detectable RememberNearest(Type objType){
        float minD = -1f;
        Detectable nearest = null;
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

    public Detectable RememberNearest(string typeName){
        float minD = -1f;
        Detectable nearest = null;
        foreach (var kvp in objsInMemory){
            if (kvp.Key.typeName == typeName){
                float d = Vector3.Distance(transform.position, kvp.Value);
                if (d < minD || minD < 0f){
                    minD =  d;
                    nearest = kvp.Key;
                } 
            }
        }
        return nearest;
    }

    public bool InMemory(Type objType){
        foreach(var kvp in objsInMemory){
            if (kvp.Key.GetType() == objType){
                return true;
            }
        }
        return false;
    }

    public bool InMemory(string typeName){
        foreach(var kvp in objsInMemory){
            if (kvp.Key.typeName == typeName){
                return true;
            }
        }
        return false;
    }
}
}
