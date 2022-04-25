using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : SmartObject
{
    protected override void Awake(){
        base.Awake();
        typeName = "Wood";
    }
    protected override void Start()
    {
        base.Start();
        value = 5f;
    }

}
