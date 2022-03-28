using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class Goal : MonoBehaviour, IGoal
{
    public virtual void Setup(){}

    public virtual float GetPriority(){
        return 0f;
    }

    public virtual bool CanRun(){
        return false;
    }

    public virtual void OnTick(){}
    public virtual void OnActivated(){}
    public virtual void OnDeactivated(){}

    public virtual string GetCondition(){return "";}

}
}