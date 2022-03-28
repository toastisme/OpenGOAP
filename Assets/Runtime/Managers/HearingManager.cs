using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Sensors{

public struct SoundPacket{}

public class HearingManager : MonoBehaviour
{
    public static HearingManager Instance{get; private set;} = null;
    public List<Hearing> hearingSensors {get; private set;} = new List<Hearing>();
    void Awake(){
        Assert.IsTrue(Instance == null);
        Instance = this;
    }

    public void Add(Hearing hearing){
        hearingSensors.Add(hearing);
    }

    public void Remove(Hearing hearing){
        hearingSensors.Remove(hearing);
    }

    public void OnSoundEmitted(
        GameObject source, 
        Vector3 location,
        SoundPacket data,
        float intensity
    ){
        for (int i = 0; i < hearingSensors.Count; i++){
            hearingSensors[i].OnHeardSound(
                source:source,
                location:location,
                data:data,
                intensity:intensity
            );
        }
    }
}
}
