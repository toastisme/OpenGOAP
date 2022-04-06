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
    Color goalColor;
    Color runningTint;
    Color defaultTint;
    Color linkColor;



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
        goalColor = GUIProperties.GoalColor();
        runningTint = GUIProperties.RunningTint();
        defaultTint = GUIProperties.DefaultTint();
        linkColor = GUIProperties.LinkColor();

        actionContent = GUIProperties.ActionContent();
        goalContent = GUIProperties.GoalContent();
    }

    void OnGUI(){

        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        if (planner != null && planner.activePlan != null && planner.activePlan.Count>0){
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

            // Draw link
            if (count > 0){
                DrawLink(
                    new Vector2(count-1, 0), 
                    new Vector2(count, 0),
                    linkColor
                );
            }

            GUI.color = defaultTint;
            GUI.backgroundColor = backgroundNodeColor;
            if (i==planner.activeActionIdx){
                GUI.color = runningTint;
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

            actionContent.text = "\n" + activePlan[i].GetType().ToString() + "\n";

            // Draw task rect
            GUI.backgroundColor = actionColor;
            GUI.Box(
                GetTaskRect(new Vector2(i,0)), 
                actionContent, 
                activeNodeStyle);

            count++;
        }

        // Draw goal
        DrawLink(
            new Vector2(count-1, 0), 
            new Vector2(count, 0),
            linkColor
        );
        GUI.color = GUIProperties.RunningTint();
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

    void DrawLink(
        Vector2 startGridPos, 
        Vector2 endGridPos, 
        Color color,
        float thickness=4f){

        Vector2 startPos = new Vector2(
            startGridPos.x * nodeSpacing.x + nodeSize.x,
            startGridPos.y * nodeSpacing.y + nodeSize.y*.5f
        );

        Vector2 endPos = new Vector2(
            endGridPos.x * nodeSpacing.x,
            endGridPos.y * nodeSpacing.y + nodeSize.y*.5f
        );

        Handles.DrawBezier(
            startPos,
            endPos,
            startPos,
            endPos,
            color,
            null,
            thickness
        );
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {

        /**
        * Background grid of the editor
        */

        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(
                new Vector3(gridSpacing * i, -gridSpacing, 0), 
                new Vector3(gridSpacing * i, position.height, 0f)
            );
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(
                new Vector3(-gridSpacing, gridSpacing * j, 0),
                new Vector3(position.width, gridSpacing * j, 0f)
            );
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }


    
}

}