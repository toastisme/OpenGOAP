using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObject : Detectable, ISmartObject
{
    public float value{get; protected set;}
    protected override void Start()
    {
        base.Start();
        value = 0f;
    }

    public virtual void PickedUp(){
        this.gameObject.SetActive(false);
    }

    public virtual void PutDown(){
    }

    public virtual void Add(SmartObject obj){}

    public virtual void Remove(){
        Destroy(this);
    }

}
