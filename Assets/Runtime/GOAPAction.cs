using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class GOAPAction : MonoBehaviour, IAction
{
    public Goal LinkedGoal{get; protected set;}

    void Awake(){
        Setup();
    }

    public void Setup(){}

    public virtual List<System.Type> GetSupportedGoals(){
        return null;
    }

    public virtual float GetCost(){
        return 0f;
    }

    public virtual void OnActivated(Goal linkedGoal){
        LinkedGoal = linkedGoal;
    }

    public virtual void OnDeactivated(){
        LinkedGoal = null;
    }

    public virtual void OnTick(){}
}
}