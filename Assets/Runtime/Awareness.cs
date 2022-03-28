using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sensors{
public class NearbyObject{
    GameObject obj;
    float memory;
    float memoryDecay;

    public NearbyObject(GameObject obj, float memory, float memoryDecay){
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
    public Dictionary<GameObject,NearbyObject> nearbyObjects {
        get; 
        private set;
        } = new Dictionary<GameObject, NearbyObject>();

    void DecayAwareness(){
        foreach (var nObj in nearbyObjects){
            nObj.Value.DecayAwareness();
            if (nObj.Value.Forgotten()){
                Forget(nObj.Key);
            }
        }
    }

    void Forget(GameObject obj){
        nearbyObjects.Remove(obj);
    }

    void Add(GameObject obj, float memory, float memoryDecay){
        nearbyObjects[obj] = new NearbyObject(obj, memory, memoryDecay);
    }


}
}