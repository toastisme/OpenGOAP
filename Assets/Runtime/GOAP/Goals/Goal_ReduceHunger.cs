using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Goal_ReduceHunger : GOAPGoal
{
        protected override void SetupDerived(){
            conditions["ReducedHunger"] = true;
            actionLayer = "Food";
        }

        public override float GetPriority()
        {
            return worldState.GetFloatState("Hunger");
        }

}