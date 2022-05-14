using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class GOAPGoal : MonoBehaviour, IGoal
{

    /**
     * \class GOAP.GOAPGoal
     * A goal that can be achieved via a sequence of GOAPActions.
     * Defined in terms of a priority, conditions required to achieve the goal,
     * preconditions required to attempt the goal, and an actionLayer that determines
     * what layer of GOAPActions will be considered for valid action plans.
     */

    protected WorldState worldState;
    
    // Only GOAPActions with this actionLayer will be included during planning
    public string actionLayer{get; protected set;} 

    // What must be in worldState for the goal to be complete
    public Dictionary<string, bool> conditions{get; protected set;}

    // What must be in worldState for the goal to be considered
    public Dictionary<string, bool> preconditions{get; protected set;}

    void Awake(){
        actionLayer = "All"; // By default consider all GOAPActions to achieve conditions
        conditions = new Dictionary<string, bool>();
        preconditions = new Dictionary<string, bool>();
        worldState = GetComponent<WorldState>();
        SetupDerived();
    }

    protected virtual void SetupDerived(){

    }

    public virtual float GetPriority(){

        /**
         * Assumed to be between 0f and 1f
         */ 

        return 0f;
    }

    public virtual bool PreconditionsSatisfied(WorldState worldState){
        // Will return true if preconditions are empty
        return worldState.IsSubset(preconditions);
    }

    public virtual bool ConditionsSatisfied(WorldState worldState){
        return worldState.IsSubset(conditions);
    }

    public virtual void OnTick(){

        /**
         * Called every frame by GOAPPlanner
         */

    }

    public virtual void OnActivate(){

        /**
         * Called when selected by GOAPPlanner
         */

    }

    public virtual void OnDeactivate(){

        /**
         * Called by GOAPPlanner when goal achieved or plan cancelled
         */

    }

}
}