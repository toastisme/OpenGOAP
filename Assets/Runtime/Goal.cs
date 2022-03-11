using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class Goal : MonoBehaviour, IGoal
{

    public GOAPAction LinkedAction {get; protected set;}
    public virtual void Setup(){}

    public virtual float CalculatePriority(){
        return 0f;
    }

    public virtual bool CanRun(){
        return false;
    }

    public virtual void OnTick(){}
    public virtual void OnActivated(GOAPAction linkedAction){}
    public virtual void OnDeactivated(){}

    void Awake(){
        Setup();
    }

    void Update(){
        OnTick();
    }

}
}