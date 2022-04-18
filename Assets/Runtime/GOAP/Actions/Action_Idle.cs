using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Action_Idle : GOAPAction
{
    bool _wasIdle;
    public override float GetCost(){
        return 0.0f;
    }

    public override void Setup(){
        base.Setup();
        _wasIdle = false;
    }
    public override void OnActivated(){
        _wasIdle = false;
    }

    public override void OnDeactivated(){
        _wasIdle = false;
    }

    public override void OnTick(){
        _wasIdle = true;
    }

    public bool WasIdle(){return _wasIdle;}

}