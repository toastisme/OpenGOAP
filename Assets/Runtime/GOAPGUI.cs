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
    
    // Grid appearance
    float majorGridSpacing = 100f;
    float minorGridSpacing = 20f;
    float majorGridOpacity = 0.4f;
    float minorGridOpacity = 0.2f;
    Color gridColor = Color.grey;

    
    private void OnGUI(){
        DrawGrid(minorGridSpacing, minorGridOpacity, Color.grey);
        DrawGrid(majorGridSpacing, majorGridOpacity, Color.grey);
        DrawPlanner();
    }

    public void SetPlanner(GOAPPlanner _goapPlanner){
        goapPlanner = _goapPlanner;
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
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0), new Vector3(gridSpacing * i, position.height, 0f));
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0), new Vector3(position.width, gridSpacing * j, 0f));
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    void DrawPlanner(){
        
        if (goapPlanner == null){
            return;
        }

        Dictionary<Goal, List<GOAPAction>> goalActionMap = goapPlanner.GetGoalActionMap();

        for (int i = 0; i < goalActionMap.Count; i++){
            var element = goalActionMap.ElementAt(i);
            string goalName = element.Key.GetType().ToString();
            GUILayout.Label(goalName);

        }


    }


}
}