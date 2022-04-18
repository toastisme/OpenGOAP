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

    // Styles
    GUIStyle guiNodeStyle;
    GUIStyle selectedNodeStyle;
    GUIStyle activeNodeStyle;
    GUIStyle panelStyle;
    GUIStyle goalLabelStyle;
    GUIStyle disabledGoalLabelStyle;

    // GUIContent
    GUIContent actionContent;
    GUIContent goalContent;

    Rect activePlanPanel;
    Rect goalPrioritiesPanel;

    float activePlanHeight = 30f;
    float maxPriorityRectWidth;
    float priorityRectHeight = 40f;
    float prioritySpacing = 25f;

    // Colors
    Color backgroundNodeColor;
    Color actionColor;
    Color goalColor;
    Color runningTint;
    Color defaultTint;
    Color linkColor;
    Color panelColor;



    public void SetPlanner(GOAPPlanner planner){
        this.planner = planner;
    }

    void OnEnable(){

        activePlanPanel = new Rect(
            0,
            0,
            position.width,
            position.height*.4f
        );
        goalPrioritiesPanel = new Rect(
            0,
            position.height*.4f,
            position.width,
            position.height
        );

        maxPriorityRectWidth = position.width - 10f;

        nodeSpacing = GUIProperties.NodeSpacing();
        nodeSize = GUIProperties.NodeSize();
        taskNodeSize = GUIProperties.TaskNodeSize();

        guiNodeStyle = GUIProperties.GUINodeStyle();
        selectedNodeStyle = GUIProperties.SelectedGUINodeStyle();
        activeNodeStyle = guiNodeStyle;
        panelStyle = GUIProperties.GUIPlannerStyle();
        goalLabelStyle = GUIProperties.GoalLabelStyle();
        disabledGoalLabelStyle = GUIProperties.DisabledGoalLabelStyle();

        backgroundNodeColor = GUIProperties.BackgroundNodeColor();
        actionColor = GUIProperties.ActionColor();
        goalColor = GUIProperties.GoalColor();
        runningTint = GUIProperties.RunningTint();
        defaultTint = GUIProperties.DefaultTint();
        linkColor = GUIProperties.LinkColor();
        panelColor = GUIProperties.PanelColor();

        actionContent = GUIProperties.ActionContent();
        goalContent = GUIProperties.GoalContent();
    }

    void OnGUI(){

        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        DrawActivePlanPanel();
        DrawGoalPrioritiesPanel();
    }


    void DrawActivePlanPanel(){
        GUI.color = runningTint;
        GUI.backgroundColor = panelColor;
        activePlanPanel = new Rect(
            0,
            0,
            position.width,
            position.height*.4f
        );
        BeginWindows();
        activePlanPanel = GUILayout.Window(
            1,
            activePlanPanel,
            DrawActivePlan,
            "Active Plan",
            panelStyle
        );
        EndWindows();
    }

    void DrawGoalPrioritiesPanel(){
        GUI.color = runningTint;
        GUI.backgroundColor = panelColor;
        goalPrioritiesPanel = new Rect(
            0,
            position.height*.4f,
            position.width,
            position.height
        );
        BeginWindows();
        goalPrioritiesPanel = GUILayout.Window(
            2,
            goalPrioritiesPanel,
            DrawGoalPriorities,
            "Goal Priorities",
            panelStyle
        );
        EndWindows();
    }

    void OnInspectorUpdate(){
        Repaint();
    }

    bool IsActive(){
        return (
            planner != null 
            && planner.activePlan != null 
            && planner.activePlan.Count>0
        );
    }

    void DrawActivePlan(int unusedWindowID){
        if (!IsActive()){return;}
        DrawActionNodes(unusedWindowID);
    }

    void DrawGoalPriorities(int unusedWindowID){
        if (!IsActive()){return;}
        List<GoalData> goalData = planner.GetSortedGoalData();
        GUILayout.Label("\n\n");
        GUI.color = runningTint;
        GUI.backgroundColor = backgroundNodeColor;
        for(int i=0; i<goalData.Count; i++){
            string name = GetTypeString(goalData[i].goalType);
            if (goalData[i].canRun){
                GUI.Box(
                    new Rect(
                        0,
                        30f + i*prioritySpacing,
                        Mathf.Clamp(goalData[i].priority, 0.05f, 1f) * maxPriorityRectWidth,
                        priorityRectHeight
                    ),
                    name,
                    goalLabelStyle
                );
            }
            else{
                GUI.Box(
                    new Rect(
                        0,
                        30f + i*prioritySpacing,
                        Mathf.Clamp(goalData[i].priority, 0.05f, 1f) * maxPriorityRectWidth,
                        priorityRectHeight
                    ),
                    name,
                    disabledGoalLabelStyle
                );
            }
        }

    }

    void DrawActionNodes(int unusedWindowID){
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
        GUI.color = runningTint;
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
        string typeString = type.ToString();
        if (typeString.Contains("_")){
            return typeString.Split("_")[1];
        }
        return typeString;
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