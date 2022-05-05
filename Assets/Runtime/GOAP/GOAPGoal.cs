using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class GOAPGoal : MonoBehaviour, IGoal
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

    public virtual bool PreconditionsSatisfied(WorldState worldState){
        return true;
    }

    public virtual bool ConditionsSatisfied(WorldState worldState){
        return worldState.IsSubset(conditions);
    }

    public virtual void OnTick(){}
    public virtual void OnActivate(){}
    public virtual void OnDeactivate(){}

}
}