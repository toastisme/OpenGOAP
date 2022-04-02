using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class HarvestWood : Goal
{
        public override float GetPriority()
        {
            return .2f;
        }

        public override bool CanRun(){
            return true;
        }

        public override string GetCondition(){
            return "WoodHarvested";
        }


    
}
}