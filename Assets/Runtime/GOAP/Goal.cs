using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class Goal : MonoBehaviour, IGoal
{
    public Dictionary<string, bool> conditions{get; protected set;}
    public virtual void Setup(){
        conditions = new Dictionary<string, bool>();
    }

    public virtual float GetPriority(){
        return 0f;
    }

    public virtual bool PreconditionsSatisfied(){
        return false;
    }

    public virtual void OnTick(){}
    public virtual void OnActivated(){}
    public virtual void OnDeactivated(){}

}
}