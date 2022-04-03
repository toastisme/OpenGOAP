using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using UnityEditor;

namespace Sensors{
public class NearbyObject{
    Detectable obj;
    public float memory;
    float memoryDecay;

    public NearbyObject(Detectable obj, float memory, float memoryDecay){
        this.obj = obj;
        this.memory = memory;
        this.memoryDecay = memoryDecay;
    }

    public void DecayAwareness(){
        memory -= memoryDecay * Time.deltaTime;
    }

    public bool Forgotten(){
        return memory <= 0f;
    }

}

public class Awareness : MonoBehaviour
{
    public Color editorColor = new Color(0f, 1f, 0f, 0.1f);
    public float editorLineThickness = 1f;

    public Dictionary<Detectable,NearbyObject> nearbyObjects {
        get; 
        private set;
        } = new Dictionary<Detectable, NearbyObject>();

    Dictionary<string, int> nearbyObjectCounts = new Dictionary<string, int>();

    public bool Nearby(string name){
        return (nearbyObjectCounts.ContainsKey(name) && nearbyObjectCounts[name] > 0);

    }

    public Detectable GetNearest(string name){
        if (!Nearby(name)){
            return null;
        }
        int totalCount = nearbyObjectCounts[name];
        int count = 0;
        Detectable nearest;
        float minDistance = -1f;
        foreach(var obj in nearbyObjects){
            if (obj.Key.typeName == name){
                float distance = (obj.Key.transform.position - transform.position).sqrMagnitude;
                if (minDistance < 0 || distance < minDistance){
                    nearest = obj.Key;
                    minDistance = distance;
                    count++;
                    if (count == totalCount){
                        return nearest;
                    } 
                }
            }
        }
        return null;
    }

    public void OnTick(){
        DecayAwareness();
    }

    void DecayAwareness(){
        List<Detectable> toForget = new List<Detectable>();
        foreach (var nObj in nearbyObjects){
            nObj.Value.DecayAwareness();
            if (nObj.Value.Forgotten()){
                toForget.Add(nObj.Key);
            }
        }
        for (int i = toForget.Count-1; i >= 0; i--){
            Forget(toForget[i]);
        }
    }

    void Forget(Detectable obj){
        nearbyObjects.Remove(obj);
        nearbyObjectCounts[obj.typeName] -= 1;
    }

    public void Add(Detectable obj, float memory, float memoryDecay){
        nearbyObjects[obj] = new NearbyObject(obj, memory, memoryDecay);
        if (nearbyObjectCounts.ContainsKey(obj.typeName)){
            nearbyObjectCounts[obj.typeName] += 1;
        }
        else{
            nearbyObjectCounts[obj.typeName] = 1;
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(Awareness))]
public class AwarenessEditor : Editor
{
    /**
     * Draws lines to nearby objects
     */

    public void OnSceneGUI()
    {
        var awareness = target as Awareness;

        Handles.color = awareness.editorColor;
        foreach(var nObj in awareness.nearbyObjects){
            Handles.DrawLine(
                awareness.transform.position, 
                nObj.Key.transform.position, 
                awareness.editorLineThickness);

        }
    }
}
#endif // UNITY_EDITOR
}