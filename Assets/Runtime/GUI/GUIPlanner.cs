using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace GOAP{
public class GUIPlanner : EditorWindow
{
    GOAPPlanner planner; // The planner being visualised
    Vector2 nodeSpacing;
    Vector2 nodeSize;

    public void SetPlanner(GOAPPlanner planner){
        this.planner = planner;
    }

    void OnEnable(){
        nodeSpacing = GUIProperties.NodeSpacing();
        nodeSize = GUIProperties.NodeSize();
    }

    void OnGUI(){
        if (planner != null){
            DrawActionNodes();
        }
    }

    void OnInspectorUpdate(){
        Repaint();
    }

    void DrawActionNodes(){
        List<GOAPAction> activePlan = planner.activePlan;
        List<GOAPAction> actions = planner.actions;
        GOAPAction activeAction = activePlan[planner.activeActionIdx];
        GUI.color = GUIProperties.InPlanTint();
        GUI.backgroundColor = GUIProperties.DefaultNodeColor();
        for (int i = 0; i < activePlan.Count; i++){
            GUI.Box(GetNodeRect(new Vector2(i,0)), activePlan[i].GetType().ToString());
        }
    }

    Rect GetNodeRect(Vector2 gridPos){
        return new Rect(
            gridPos.x * nodeSpacing.x,
            gridPos.y * nodeSpacing.y,
            nodeSize.x,
            nodeSize.y
        );
    }

    
}

}