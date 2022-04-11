
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class HarvestFood : Goal
{
        public override void Setup(WorldState worldState){
            base.Setup(worldState);
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