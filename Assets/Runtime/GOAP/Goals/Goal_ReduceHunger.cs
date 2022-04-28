using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Goal_ReduceHunger : Goal
{
        public override void Setup(){
            base.Setup();
            conditions["ReducedHunger"] = true;
            actionLayer = "Food";
        }

        public override float GetPriority()
        {
            return worldState.GetFloatState("Hunger");
        }

}