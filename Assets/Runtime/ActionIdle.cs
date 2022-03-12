using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class ActionIdle : GOAPAction
{
    List<System.Type> SupportedGoals = new List<System.Type>(new System.Type[] {typeof(GoalIdle)});

    public override List<System.Type> GetSupportedGoals(){
        return SupportedGoals;
    }

    public override float GetCost(){
        return 0f;
    }

}
}