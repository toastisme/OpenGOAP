using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GOAPActionMock1 : GOAPAction
{
    protected override void SetupActionLayers(){
        actionLayers.Add("Mock");
    }

    protected override void SetupEffects(){
        effects["mockGoalCondition1"] = true;
    }

    protected override void SetupConditions(){
        preconditions["mockActionCondition1"] = true;
    }

    public override void OnTick(){
        AddState("mockGoalCondition1", true);
        AddTemporaryState("mockTemporaryState", true);
    }    

}
