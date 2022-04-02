using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObject : Detectable, ISmartObject
{
    Renderer rend;
    protected override void Start()
    {
        base.Start();
        rend = GetComponent<Renderer>();
    }

    public virtual void PickedUp(){
        rend.enabled = false;
    }

    public virtual void PutDown(){
        rend.enabled = true;
    }

    public virtual void Add(SmartObject obj){}

    public virtual void Remove(){
        Destroy(this);
    }

}
