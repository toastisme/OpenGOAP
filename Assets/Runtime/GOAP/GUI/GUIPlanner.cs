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
    Vector2 taskNodeSize;
    GUIStyle guiNodeStyle;
    GUIStyle selectedNodeStyle;
    GUIStyle activeNodeStyle;

    GUIContent actionContent;
    GUIContent goalContent;

    // Colors
    Color backgroundNodeColor;
    Color actionColor;
    Color completedActionColor;
    Color goalColor;
    Color runningTint;
    Color defaultTint;



    public void SetPlanner(GOAPPlanner planner){
        this.planner = planner;
    }

    void OnEnable(){
        nodeSpacing = GUIProperties.NodeSpacing();
        nodeSize = GUIProperties.NodeSize();
        taskNodeSize = GUIProperties.TaskNodeSize();

        guiNodeStyle = GUIProperties.GUINodeStyle();
        selectedNodeStyle = GUIProperties.SelectedGUINodeStyle();
        activeNodeStyle = guiNodeStyle;

        backgroundNodeColor = GUIProperties.BackgroundNodeColor();
        actionColor = GUIProperties.ActionColor();
        completedActionColor = GUIProperties.CompletedActionColor();
        goalColor = GUIProperties.GoalColor();
        runningTint = GUIProperties.RunningTint();
        defaultTint = GUIProperties.DefaultTint();

        actionContent = GUIProperties.ActionContent();
        goalContent = GUIProperties.GoalContent();
    }

    void OnGUI(){
        if (planner != null && planner.activePlan != null){
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
        int count = 0;
        for (int i = 0; i < activePlan.Count; i++){

            GUI.color = GUIProperties.DefaultTint();
            GUI.backgroundColor = backgroundNodeColor;
            if (i==planner.activeActionIdx){
                GUI.color = GUIProperties.RunningTint();
                activeNodeStyle = selectedNodeStyle;
            }
            else{
                activeNodeStyle = guiNodeStyle;
            }
            // Draw node rect
            GUI.Box(
                GetNodeRect(new Vector2(i,0)), 
                "",
                activeNodeStyle);

            if (i<planner.activeActionIdx){
                GUI.backgroundColor = completedActionColor;
            }
            else{
                GUI.backgroundColor = actionColor;
            }

            actionContent.text = "\n" + activePlan[i].GetType().ToString() + "\n";

            GUI.Box(
                GetTaskRect(new Vector2(i,0)), 
                actionContent, 
                activeNodeStyle);
            count++;
        }
        // Draw goal
        GUI.color = GUIProperties.DefaultTint();
        GUI.backgroundColor = backgroundNodeColor;
        GUI.Box(
            GetNodeRect(new Vector2(count,0)), 
            "",
            selectedNodeStyle);
        GUI.backgroundColor = goalColor;
        goalContent.text = "\n" + planner.activeGoal.GetType().ToString() + "\n";
        GUI.Box(
            GetTaskRect(new Vector2(count,0)), 
            goalContent, 
            selectedNodeStyle);

    }

    Rect GetNodeRect(Vector2 gridPos){
        return new Rect(
            gridPos.x * nodeSpacing.x,
            gridPos.y * nodeSpacing.y,
            nodeSize.x,
            nodeSize.y
        );
    }
    Rect GetTaskRect(Vector2 gridPos){
        return new Rect(
            gridPos.x * nodeSpacing.x,
            gridPos.y * nodeSpacing.y + taskNodeSize.y*.25f,
            taskNodeSize.x,
            taskNodeSize.y
        );
    }


    
}

}