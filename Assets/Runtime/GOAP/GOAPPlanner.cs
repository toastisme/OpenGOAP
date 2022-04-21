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

public class ActionNode{
    public ActionNode parent;
    public GOAPAction action;
    public ActionNode(ActionNode parent, GOAPAction action){
        this.parent = parent;
        this.action = action;
    }
}

public class GOAPPlanner : MonoBehaviour
{
    [SerializeField]
    Logger logger;
    WorldState worldState;
    public List<Goal> goals{get; private set;}
    public List<GOAPAction> actions{get; private set;}

    //// Active
    public Goal activeGoal{get; private set;}
    public int activeActionIdx{get; private set;}
    public List<GOAPAction> activePlan{get; private set;}

    //// Optimal
    Goal optimalGoal;
    List<GOAPAction> optimalPlan;

    //// GUI bookkeeping
    public bool displayPlanner = false;
    bool displayingPlanner = false;

    void Start(){
        worldState = GetComponent<WorldState>();
        goals = new List<Goal>(GetComponents<Goal>());
        actions = new List<GOAPAction>(GetComponents<GOAPAction>());
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

        // Goal no longer viable
        if (!activeGoal.PreconditionsSatisfied()){
            OnFailActivePlan();
            return;
        }

        // Plan no longer viable
        if (!(activePlan[activeActionIdx].PreconditionsSatisfied())){ 
            OnFailActivePlan(); 
            return;
        }

        activePlan[activeActionIdx].OnTick();

        // Goal complete
        if (activeGoal.ConditionsSatisfied()){
            OnCompleteActivePlan();
            return;
        }

        if (activeActionIdx < activePlan.Count-1){
            // At least one more action after activeAction
            if (activePlan[activeActionIdx + 1].PreconditionsSatisfied()){
                // Can move to next action
                activePlan[activeActionIdx].OnDeactivated();
                activeActionIdx++;
                activePlan[activeActionIdx].OnActivated();
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
                currentState:worldState,
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

        bool InList(GOAPAction action, List<ActionNode> nodeList){
            for(int i = 0; i < nodeList.Count; i++){
                if (nodeList[i].action == action){
                    return true;
                }
            }
            return false;
        }

        Log($"Searching for plan for {goal.GetType().ToString()}");

        List<ActionNode> openList = new List<ActionNode>();
        List<ActionNode> closedList = new List<ActionNode>();

        // Get starting node
        GOAPAction startAction = null;
        float minCost = -1;
        for (int i = 0; i< actions.Count; i++){
            if (actions[i].SatisfiesConditions(goal.conditions)){
                Log($"{actions[i].GetType().ToString()} satisfies goal");
                float cost = actions[i].GetCost();
                if (minCost < 0 || cost < minCost){
                    minCost = cost;
                    startAction = actions[i];
                }
            }
        }

        // No path found
        if (startAction == null){
            Log("No starting node found");
            return null;}
        openList.Add(new ActionNode(null, startAction));
        Dictionary<string, bool> requiredState = goal.conditions;

        while (openList.Count != 0){

            ActionNode currentNode = GetNextNode(
                requiredState:requiredState,
                availableNodes:openList);

            // No path found
            if (currentNode == null){
                return null;
            }
            closedList.Add(currentNode);            
            openList.Remove(currentNode);

            // Found complete path
            if (currentState.IsSubset(currentNode.action.preconditions)){
                Log("Found complete path");
                return GeneratePath(closedList);
            }

            requiredState = currentNode.action.preconditions;
            List<GOAPAction> linkedActions = GetLinkedActions(
                node:currentNode.action,
                availableNodes:actions
            );

            Log($"Found {linkedActions.Count} linked to current node");

            for (int i = 0; i < linkedActions.Count; i++){
                if (!InList(linkedActions[i], closedList)){
                    if (!InList(linkedActions[i], openList)){
                        openList.Add(new ActionNode(currentNode, linkedActions[i]));
                    }
                }
            }
        }

        Log("No path found");
        // No path found
        return null;
    }

    List<GOAPAction> GeneratePath(List<ActionNode> closedList){
        List<GOAPAction> path = new List<GOAPAction>();
        ActionNode currentNode = closedList[closedList.Count - 1];
        while(currentNode.parent != null){
            path.Add(currentNode.action);
            currentNode = currentNode.parent;
        }
        path.Add(currentNode.action);
        return path;
    }    

    ActionNode GetNextNode(
        Dictionary<string, bool> requiredState, 
        List<ActionNode> availableNodes){
            /**
             * Searches for the node in availableNodes with the smallest
             * cost that satisfies requiredState
             */

            Log($"Checking {availableNodes.Count} nodes for one that satisfies:");
            foreach (var i in requiredState){
                Log($"{i.Key} {i.Value}");
            }
            float minCost = -1f;
            ActionNode nextNode = null;
            for (int i = 0; i < availableNodes.Count; i++){
                if (!availableNodes[i].action.SatisfiesConditions(requiredState)){
                    Log($"{availableNodes[i].action.GetType().ToString()} does not satisfy conditions");
                    continue;
                }
                Log($"{availableNodes[i].action.GetType().ToString()} satisfies conditions");
                float cost = availableNodes[i].action.GetCost();
                if (minCost < 0 || cost < minCost){
                    nextNode = availableNodes[i];
                    minCost = cost;
                }
            }
            Log($"Selected {nextNode.action.GetType().ToString()}");
            return nextNode;
        }

    List<GOAPAction> GetLinkedActions(
        GOAPAction node, 
        List<GOAPAction> availableNodes){
        /**
         * Searches availableNodes for all those that satisfy node.preconditions 
         * and that are not in path  
         */
        Log($"Finding actions linked to {node.GetType().ToString()}");
        List<GOAPAction> linkedNodes = new List<GOAPAction>();
        for (int i = 0; i < availableNodes.Count; i++){
            if (availableNodes[i].SatisfiesConditions(node.preconditions)){
                Log($"{availableNodes[i].GetType().ToString()} satisfies conditions");
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

    void Log(object message){
        if(logger){
            logger.Log(message, this);
        }
    }

}
}