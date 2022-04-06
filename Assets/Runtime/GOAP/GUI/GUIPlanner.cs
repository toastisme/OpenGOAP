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

    float activePlanHeight = 30f;
    float topPropertiesHeight = 0.2f;

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

        GUILayout.Label("Active Plan");
        DrawActivePlan();

        GUI.Label(
            new Rect(
                0, 
                (int)(Screen.height*topPropertiesHeight), 
                100, 
                100), 
                "Top Priorities"
            );
    }

    void OnInspectorUpdate(){
        Repaint();
    }

    void DrawActivePlan(){
        if (planner != null && planner.activePlan != null && planner.activePlan.Count>0){
            DrawActionNodes();
        }
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
                    count-1, 
                    count,
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
                GetNodeRect(count), 
                "",
                activeNodeStyle);

            actionContent.text = "\n\n" + GetTypeString(activePlan[i].GetType());

            // Draw task rect
            GUI.backgroundColor = actionColor;
            GUI.Box(
                GetTaskRect(count), 
                actionContent, 
                activeNodeStyle);

            count++;
        }

        // Draw goal
        DrawLink(
            count-1, 
            count,
            linkColor
        );
        GUI.color = GUIProperties.RunningTint();
        GUI.backgroundColor = backgroundNodeColor;
        GUI.Box(
            GetNodeRect(count), 
            "",
            selectedNodeStyle);
        GUI.backgroundColor = goalColor;
        goalContent.text = "\n\n" + GetTypeString(planner.activeGoal.GetType());
        GUI.Box(
            GetTaskRect(count), 
            goalContent, 
            selectedNodeStyle);
    }

    string GetTypeString(Type type){
        /**
         * Strips type.ToString() of GOAP namespace
         */

         string name = type.ToString();
         return name.Split("GOAP.")[1];

    }

    Rect GetNodeRect(int gridPos){
        return new Rect(
            gridPos * nodeSpacing.x,
            activePlanHeight,
            nodeSize.x,
            nodeSize.y
        );
    }

    Rect GetTaskRect(int gridPos){
        return new Rect(
            gridPos * nodeSpacing.x,
            activePlanHeight + taskNodeSize.y*.25f,
            taskNodeSize.x,
            taskNodeSize.y
        );
    }

    void DrawLink(
        int startGridPos, 
        int endGridPos, 
        Color color,
        float thickness=4f){

        Vector2 startPos = new Vector2(
            startGridPos * nodeSpacing.x + nodeSize.x,
            activePlanHeight + nodeSize.y*.5f
        );

        Vector2 endPos = new Vector2(
            endGridPos * nodeSpacing.x,
            activePlanHeight + nodeSize.y*.5f
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