using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP{
public class GoalIdle : Goal
{
        public override float GetPriority()
        {
            return .1f;
        }

        public override bool CanRun(){
            return true;
        }

    
}
}