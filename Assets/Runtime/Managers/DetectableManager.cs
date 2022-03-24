using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DetectableManager : MonoBehaviour
{
    public static DetectableManager Instance {get; private set;} = null;
    public List<Detectable> Detectables {get; private set;} = new List<Detectable>();

    void Awake(){
        Assert.IsTrue(Instance == null);
        Instance = this;
    }

    public void Add(Detectable detectable){
        Detectables.Add(detectable);
    }

    public void Remove(Detectable detectable){
        Detectables.Remove(detectable);
    }
}
