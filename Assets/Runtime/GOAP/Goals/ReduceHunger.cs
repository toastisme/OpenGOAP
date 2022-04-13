using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class ReduceHunger : Goal
{
        public override void Setup(){
            base.Setup();
            conditions["HungerReduced"] = true;
        }

        public override float GetPriority()
        {
            return worldState.GetFloatState("hunger");
        }

}