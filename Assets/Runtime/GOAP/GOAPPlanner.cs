using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using System;

namespace GOAP{

public struct GoalData{
    public Type goalType;
    public float priority;
    public bool canRun;

    public GoalData(Type goalType, float priority, bool canRun){
        this.priority = priority;
        this.goalType = goalType;
        this.canRun = canRun;
    }
}
public class GOAPPlanner : MonoBehaviour
{
    WorldState personalState;
    public List<Goal> goals{get; private set;}
    public List<GOAPAction> actions{get; private set;}

    //// Active
    public Goal activeGoal{get; private set;}
    public int activeActionIdx{get; private set;}
    public List<GOAPAction> activePlan{get; private set;}

    //// Optimal
    Goal optimalGoal;
    List<GOAPAction> optimalPlan;

    //// Planner
    public bool displayPlanner = false;
    bool displayingPlanner = false;

    void Start(){
        goals = new List<Goal>(GetComponents<Goal>());
        actions = new List<GOAPAction>(GetComponents<GOAPAction>());
        personalState = GetComponent<WorldState>();
        for (int i = 0; i < goals.Count; i++){
            goals[i].Setup();
        }
        for (int i = 0; i < actions.Count; i++){
            actions[i].Setup();
        }
    }

    void Update(){

        OnTick();

        GetHighestPriorityGoal(chosenGoal:out optimalGoal, chosenPlan:out optimalPlan);

        if ((NoActiveGoal() && GoalAvailable()) || BetterGoalAvailable()){
            StartCurrentBestGoal();
        } 

        UpdateDisplayPlanner();

    }

    bool NoActiveGoal(){
        return activeGoal == null;
    }


    bool BetterGoalAvailable(){
        return optimalGoal != null && optimalGoal != activeGoal;
    }

    bool GoalAvailable(){
        return optimalPlan != null && optimalGoal != null;
    }

    void StartCurrentBestGoal(){
        if (activeGoal != null){
            activeGoal.OnDeactivated();
        }
        if (activePlan != null && activeActionIdx < activePlan.Count){
            activePlan[activeActionIdx].OnDeactivated();
        }

        activeActionIdx = 0;
        activeGoal = optimalGoal;
        activePlan = optimalPlan;
        activeGoal.OnActivated();
        activePlan[activeActionIdx].OnActivated();
    }

    void OnTick(){
        OnTickGoals();
        OnTickActivePlan();
    }

    void OnTickGoals(){
        if (goals != null){
            for (int i = 0; i < goals.Count; i++){
                goals[i].OnTick();
            }
        }
    }

    void OnTickActivePlan(){

        // Nothing to run
        if (!(activeGoal != null && activePlan != null)){ return; }

        // Plan no longer viable
        if (!(activePlan[activeActionIdx].PreconditionsSatisfied())){ 
            OnFailActivePlan(); 
            return;
        }

        activePlan[activeActionIdx].OnTick();

        if (activePlan[activeActionIdx].EffectsSatisfied()){
            // Action complete
            activePlan[activeActionIdx].OnDeactivated();
            activeActionIdx++;

            if (activeActionIdx < activePlan.Count){
                // Start new action
                activePlan[activeActionIdx].OnActivated();
            }
            else{
                // Goal complete
                OnCompleteActivePlan();
            }
        }
    }

    void OnCompleteActivePlan(){
        activeGoal.OnDeactivated();
        activeGoal = null;
        activePlan = null;
    }

    void OnFailActivePlan(){
        activeGoal.OnDeactivated();
        activeGoal = null;
        activePlan = null;
    }

    void GetHighestPriorityGoal(out Goal chosenGoal, out List<GOAPAction> chosenPlan){

        /**
         * Updates chosenGoal and chosenPlan with the highest priorty goal that 
         * has a valid plan
         */

        chosenGoal = null;
        chosenPlan = null;
        if (goals == null){return;}

        for (int i = 0; i < goals.Count; i++){

            if (!goals[i].PreconditionsSatisfied()){
                continue;
            }

            if (!(chosenGoal == null || HasHigherPriority(goals[i], chosenGoal))){
                continue;
            }

            List<GOAPAction> candidatePath = GetOptimalPath(
                currentState:personalState,
                goal:goals[i], 
                actions:actions
                );

            if (candidatePath != null){
                chosenGoal = goals[i];
                chosenPlan = candidatePath;
            }
        }
    }

    bool HasHigherPriority(Goal goal, Goal other){
        return goal.GetPriority() > other.GetPriority();
    }


    //// A*

    List<GOAPAction> GetOptimalPath(
        WorldState currentState, 
        Goal goal, 
        List<GOAPAction> actions){

        List<GOAPAction> availableNodes = new List<GOAPAction>();
        List<GOAPAction> path = new List<GOAPAction>();

        // Get starting node
        GOAPAction startNode = null;
        float minCost = -1;
        for (int i = 0; i< actions.Count; i++){
            if (actions[i].SatisfiesConditions(goal.conditions)){
                float cost = actions[i].GetCost();
                if (minCost < 0 || cost < minCost){
                    minCost = cost;
                    startNode = actions[i];
                }
            }
        }

        // No path found
        if (startNode == null){return null;}
        availableNodes.Add(startNode);
        Dictionary<string, bool> requiredState = goal.conditions;

        while (availableNodes.Count != 0){

            GOAPAction currentNode = GetNextNode(
                requiredState:requiredState,
                path:path,
                availableNodes:availableNodes);

            // No path found
            if (currentNode == null){
                return null;
            }
            path.Add(currentNode);            

            // Found complete path
            if (currentState.IsSubset(currentNode.preconditions)){
                path.Reverse();
                return path;
            }

            requiredState = currentNode.preconditions;
            List<GOAPAction> linkedNodes = GetLinkedNodes(
                node:currentNode,
                path:path,
                availableNodes:actions
                );
            for (int i = 0; i < linkedNodes.Count; i++){
                availableNodes.Add(linkedNodes[i]);
            }
        }
        // No path found
        return null;
    }

    GOAPAction GetNextNode(
        Dictionary<string, bool> requiredState, 
        List<GOAPAction> path, 
        List<GOAPAction> availableNodes){
            /**
             * Searches for the node in availableNodes with the smallest
             * cost that satisfies requiredState
             */
            
            float minCost = -1f;
            GOAPAction nextNode = null;
            for (int i = 0; i < availableNodes.Count; i++){
                if (path.Contains(availableNodes[i])){continue;}
                if (!availableNodes[i].SatisfiesConditions(requiredState)){
                    continue;
                }
                float cost = availableNodes[i].GetCost();
                if (minCost < 0 || cost < minCost){
                    nextNode = availableNodes[i];
                    minCost = cost;
                }
            }
            return nextNode;
        }

    List<GOAPAction> GetLinkedNodes(
        GOAPAction node, 
        List<GOAPAction> path, 
        List<GOAPAction> availableNodes){
        /**
         * Searches availableNodes for all those that satisfy node.preconditions 
         * and that are not in path  
         */
        List<GOAPAction> linkedNodes = new List<GOAPAction>();
        for (int i = 0; i < availableNodes.Count; i++){
            if (path.Contains(availableNodes[i])){continue;}
            if (availableNodes[i].SatisfiesConditions(node.preconditions)){
                linkedNodes.Add(availableNodes[i]);
            }
        }
        return linkedNodes;
    }

    //// Planner display

    void UpdateDisplayPlanner(){

        if (Selection.activeObject == this.gameObject && !displayingPlanner && displayPlanner){
            DisplayPlanner();
        }
        else if (Selection.activeObject != this.gameObject && displayingPlanner){
            StopDisplayingPlanner();
        }
    }

    void DisplayPlanner(){
        displayingPlanner = true;
        GUIPlanner window = EditorWindow.GetWindow(typeof(GUIPlanner)) as GUIPlanner;
        window.SetPlanner(this);
    }

    void StopDisplayingPlanner(){
        displayingPlanner = false;
    }

    public List<GoalData> GetSortedGoalData(){

        /**
         * Returns list of GoalData reverse sorted by priority
         */

        List<GoalData> goalData = new List<GoalData>();
        for (int i=0; i<goals.Count; i++){
            goalData.Add(
                new GoalData(
                    goals[i].GetType(), 
                    goals[i].GetPriority(), 
                    goals[i].PreconditionsSatisfied()
                    )
            );
        }

        goalData.Sort((x, y) => x.priority.CompareTo(y.priority));
        goalData.Reverse();
        return goalData;
    }

}
}