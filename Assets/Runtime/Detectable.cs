using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detectable : MonoBehaviour
{
    public string typeName {get; protected set;}
    protected virtual void Start()
    {
        DetectableManager.Instance?.Add(this);
        typeName = "Detectable";
    }

    void OnDestroy(){
        DetectableManager.Instance?.Remove(this);
    }
}