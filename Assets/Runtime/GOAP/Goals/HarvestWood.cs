using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class HarvestWood : Goal
{

        public override void Setup(){
            base.Setup();
            conditions["WoodHarvested"] = true;
        }

        public override float GetPriority()
        {
            return .2f;
        }

        public override bool CanRun(){
            return true;
        }
}
}