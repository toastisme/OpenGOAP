using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sensors{
public class Detectable : MonoBehaviour
{
    protected virtual void Start()
    {
        DetectableManager.Instance?.Add(this);
    }

    void OnDestroy(){
        DetectableManager.Instance?.Remove(this);
    }

    public virtual string Name(){
        return "Detectable";
    }
}
}