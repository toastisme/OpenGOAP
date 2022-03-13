using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace GOAP{
public class GOAPGUI : EditorWindow
{
    /**
    * Class to visualise a GameObject's GOAP planner
    */

    GOAPPlanner goapPlanner;
    Dictionary<string, bool> displayActions;
    
    // Bar apppearance
    float barThickness = 40f;
    GUIStyle barStyle;
    Color defaultBarColor = Color.white;
    Color disabledBarColor = Color.grey;
    Color activeBarColor = Color.green;
    float minDrawCost = 0.05f; // Min action cost to draw cost bar

    // Labels
    GUIStyle goalStyle;
    GUIStyle goalHeaderStyle;
    GUIStyle actionStyle;

    void OnEnable(){
        SetupStyles();
    }

    void SetupStyles(){
        barStyle = new GUIStyle();
        barStyle.normal.background = EditorGUIUtility.Load("builtin skins/lightskin/images/node0.png") as Texture2D;
        barStyle.border = new RectOffset(12, 12, 12, 12);
        barStyle.padding=new RectOffset(10,0,0,0);

        goalStyle = new GUIStyle();
        goalStyle.normal.textColor = Color.white;
        goalStyle.alignment = TextAnchor.MiddleLeft;
        goalStyle.fontStyle = FontStyle.Bold;

        goalHeaderStyle = new GUIStyle();
        goalHeaderStyle.normal.textColor = Color.white;
        goalHeaderStyle.alignment = TextAnchor.MiddleCenter;
        goalHeaderStyle.fontStyle = FontStyle.Bold;

        actionStyle = new GUIStyle();
        actionStyle.normal.textColor = Color.white;
        actionStyle.alignment = TextAnchor.MiddleLeft;

    }

    void OnInspectorUpdate(){
        Repaint();
    }

    
    private void OnGUI(){
        DrawPlanner();
    }

    public void SetPlanner(GOAPPlanner _goapPlanner){
        goapPlanner = _goapPlanner;
        displayActions = new Dictionary<string, bool>();
    }

    List<Goal> GetGoalsOrderedByPriority(Dictionary<Goal, List<GOAPAction>> goalActionMap)
    {
        List<Goal> goals = goalActionMap.Keys.ToList();
        goals.Sort((x,y) => x.GetPriority().CompareTo(y.GetPriority()));
        return goals;
    }

    List<GOAPAction> GetActionsOrderedByCost(List<GOAPAction> goapActions){
        goapActions.Sort((x,y) => x.GetCost().CompareTo(y.GetCost()));
        goapActions.Reverse();
        return goapActions;

    }

    void DrawPlanner(){
        
        if (goapPlanner == null || !Application.isPlaying){
            return;
        }

        Color currentColor = GUI.backgroundColor;        

        Dictionary<Goal, List<GOAPAction>> goalActionMap = goapPlanner.GetGoalActionMap();
        List<Goal> sortedGoals = GetGoalsOrderedByPriority(goalActionMap);

        GUILayout.Label("Goals", goalHeaderStyle);
        bool runningGoalFound = false;

        for (int i = 0; i < sortedGoals.Count; i++){

            string goalName = sortedGoals[i].GetType().ToString();
            float priority = sortedGoals[i].GetPriority();
            float goalBarWidth = priority * position.width * 0.8f;

            if (!displayActions.ContainsKey(goalName)){
                displayActions[goalName] = false;
            }

            GUILayout.BeginHorizontal();
            displayActions[goalName] = GUILayout.Toggle(displayActions[goalName], goalName, goalStyle);

            if (sortedGoals[i].CanRun()){
                if (!runningGoalFound){
                    runningGoalFound = true;
                    GUI.backgroundColor = activeBarColor;
                }
                else{
                    GUI.backgroundColor = defaultBarColor;
                }
            }
            else{
                GUI.backgroundColor = disabledBarColor;
            }
            GUILayout.Box("", barStyle, GUILayout.Width(goalBarWidth), GUILayout.Height(barThickness));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (displayActions[goalName]){
                List<GOAPAction> goalActions = GetActionsOrderedByCost(goalActionMap[sortedGoals[i]]);
                GUILayout.Label("    Actions", actionStyle);
                GUI.backgroundColor = defaultBarColor;
                for (int j = 0; j < goalActions.Count; j++){
                    float cost = goalActions[j].GetCost();
                    float actionBarWidth = cost * position.width * 0.7f;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"    {goalActions[j].GetType().ToString()}", actionStyle);
                    if (cost > minDrawCost){
                        GUILayout.Box("", barStyle, GUILayout.Width(actionBarWidth), GUILayout.Height(barThickness));
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }
        GUI.backgroundColor = currentColor;
    }


}
}