using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class Goal : MonoBehaviour, IGoal
{

    protected WorldState worldState;

    /*
     * Only GOAPActions with this actionLayer will be included during planning
     */
    public string actionLayer{get; protected set;} 

    public Dictionary<string, bool> conditions{get; protected set;}
    public virtual void Setup(){
        actionLayer = "All";
        conditions = new Dictionary<string, bool>();
        worldState = GetComponent<WorldState>();
    }

    public virtual float GetPriority(){
        return 0f;
    }

    public virtual bool PreconditionsSatisfied(){
        return true;
    }

    public virtual bool ConditionsSatisfied(){
        return worldState.IsSubset(conditions);
    }

    public virtual void OnTick(){}
    public virtual void OnActivated(){}
    public virtual void OnDeactivated(){}

}
}