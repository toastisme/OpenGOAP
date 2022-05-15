using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class GOAPActionMock2 : GOAPAction
{
    protected override void SetupActionLayers(){
        actionLayers.Add("Mock");
    }

    protected override void SetupEffects(){
        effects["mockActionCondition1"] = true;
    }

    public override void OnTick(){
        AddState("mockActionCondition1", true);
    }    

}
