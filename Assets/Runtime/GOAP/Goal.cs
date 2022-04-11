using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class Goal : MonoBehaviour, IGoal
{

    WorldState personalState;

    public Dictionary<string, bool> conditions{get; protected set;}
    public virtual void Setup(WorldState worldState){
        conditions = new Dictionary<string, bool>();
        personalState = worldState;
    }

    public virtual float GetPriority(){
        return 0f;
    }

    public virtual bool PreconditionsSatisfied(){
        return true;
    }

    public virtual bool ConditionsSatisfied(){
        return personalState.IsSubset(conditions);
    }

    public virtual void OnTick(){}
    public virtual void OnActivated(){}
    public virtual void OnDeactivated(){}

}
}