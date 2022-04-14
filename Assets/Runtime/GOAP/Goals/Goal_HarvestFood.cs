using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Goal_HarvestFood : Goal
{
        public override void Setup(){
            base.Setup();
            conditions["FoodHarvested"] = true;
        }

        public override float GetPriority()
        {
            return .3f;
        }

        public override bool PreconditionsSatisfied(){
            return true;
        }
}