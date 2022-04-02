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
    public WorldState worldState;

    public Dictionary<Detectable,NearbyObject> nearbyObjects {
        get; 
        private set;
        } = new Dictionary<Detectable, NearbyObject>();

    Dictionary<string, int> nearbyObjectCounts = new Dictionary<string, int>();

    public void SetWorldState(ref WorldState worldState){
        this.worldState = worldState;
    }

    public void OnTick(){
        DecayAwareness();
        UpdateWorldState();
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

    void UpdateWorldState(){
        foreach(var nObj in nearbyObjectCounts){
            if (nObj.Value > 0){
                worldState.boolKeys[$"{nObj.Key}Nearby"] = true;
            }
            else{
                worldState.boolKeys[$"{nObj.Key}Nearby"] = false;
            }
        }
    }

    void Forget(Detectable obj){
        nearbyObjects.Remove(obj);
        nearbyObjectCounts[obj.Name()] -= 1;
    }

    public void Add(Detectable obj, float memory, float memoryDecay){
        nearbyObjects[obj] = new NearbyObject(obj, memory, memoryDecay);
        if (nearbyObjectCounts.ContainsKey(obj.Name())){
            nearbyObjectCounts[obj.Name()] += 1;
        }
        else{
            nearbyObjectCounts[obj.Name()] = 1;
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