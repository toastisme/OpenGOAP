using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodStore : SmartObject
{
    protected override void Start()
    {
        base.Start();
        value = 0f;
    }

    public override void Add(SmartObject obj){
        if (obj.typeName == "Wood"){
            value += obj.value;
            obj.Remove();
        }
    }

}
