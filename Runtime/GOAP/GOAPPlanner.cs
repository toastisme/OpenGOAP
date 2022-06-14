using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using System;

namespace GOAP{

public struct GoalData{

    /**
     * \struct GoalData
     * Used to package GOAPGoal data for other classes (e.g. GUIPlanner)
     */

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

    /**
     * \class ActionNode
     * Used to create a linked list of GOAPActions
     */

    public ActionNode parent;
    public GOAPAction action;
    public ActionNode(ActionNode parent, GOAPAction action){
        this.parent = parent;
        this.action = action;
    }
}


[RequireComponent(typeof(WorldState))]
public class GOAPPlanner : MonoBehaviour
{

    /**
     * \class GOAPPlanner
     * Identifies the highest priority GOAPGoal with a viable action plan
     * and carries out that action plan.
     */

    // Loggers
    [SerializeField]
    Logger plannerLogger;
    [SerializeField]
    Logger activePlanLogger;

    WorldState worldState;
    public List<GOAPGoal> goals{get; private set;}
    public Dictionary<string, List<GOAPAction> > actions{get; private set;}

    //// Active
    public GOAPGoal activeGoal{get; private set;}
    public int activeActionIdx{get; private set;}
    public List<GOAPAction> activePlan{get; private set;}

    //// Optimal
    GOAPGoal optimalGoal;
    List<GOAPAction> optimalPlan;

    //// GUI bookkeeping
    public bool displayPlanner = false;
    bool displayingPlanner = false;

    void Start(){
        worldState = GetComponent<WorldState>();
        actions = new Dictionary<string, List<GOAPAction> >();
        goals = new List<GOAPGoal>(GetComponents<GOAPGoal>());
        SetupGoals();
        SetupActions();
    }

    void SetupActions(){

        /**
         * Caches the actionLayer of each GOAPAction.
         */

        List<GOAPAction> allActions = new List<GOAPAction>(GetComponents<GOAPAction>());
        for (int i = 0; i < allActions.Count; i++){
            actions["All"].Add(allActions[i]);
            for(int j = 0; j < allActions[i].actionLayers.Count; j++){
                string actionLayer = allActions[i].actionLayers[j];
                if (!actions.ContainsKey(actionLayer)){
                    actions[actionLayer] = new List<GOAPAction>();
                }
                if (!actions[actionLayer].Contains(allActions[i])){
                    actions[actionLayer].Add(allActions[i]);
                }
            }
        }
    }

    void SetupGoals(){

        /**
         * Adds the actionLayer of each GOAPGoal as a key in actions.
         */

        actions["All"] = new List<GOAPAction>();
        for (int i = 0; i < goals.Count; i++){
            // This ensures no key errors when finding optimal plans
            if (!actions.ContainsKey(goals[i].actionLayer)){
                actions[goals[i].actionLayer] = new List<GOAPAction>();
            }
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
            activeGoal.OnDeactivate();
        }
        if (activePlan != null && activeActionIdx < activePlan.Count){
            activePlan[activeActionIdx].OnDeactivate();
        }

        activeActionIdx = 0;
        activeGoal = optimalGoal;
        activePlan = optimalPlan;
        ActivePlanLog($"Starting new plan for {activeGoal}", bold:true);
        activeGoal.OnActivate();
        ActivePlanLog($"Starting {activePlan[activeActionIdx]}");
        activePlan[activeActionIdx].OnActivate();
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
        if (!activeGoal.PreconditionsSatisfied(worldState)){
            ActivePlanLog(
                $"{activeGoal} failed as preconditions are no longer satisfied",
                bold:true
            );
            OnFailActivePlan();
            return;
        }

        // Plan no longer viable
        if (!(activePlan[activeActionIdx].PreconditionsSatisfied(worldState))){ 
            ActivePlanLog(
                $"{activePlan[activeActionIdx]} failed as preconditions are no longer satisfied",
                bold:true
                );
            OnFailActivePlan(); 
            return;
        }

        activePlan[activeActionIdx].OnTick();

        // Goal complete
        if (activeGoal.ConditionsSatisfied(worldState)){
            ActivePlanLog($"{activeGoal} completed", bold:true);
            OnCompleteActivePlan();
            return;
        }

        if (activeActionIdx < activePlan.Count-1){
            // At least one more action after activeAction
            if (activePlan[activeActionIdx + 1].PreconditionsSatisfied(worldState)){
                // Can move to next action
                ActivePlanLog($"{activePlan[activeActionIdx]} complete");
                activePlan[activeActionIdx].OnDeactivate();
                activeActionIdx++;
                ActivePlanLog($"Moving to new action: {activePlan[activeActionIdx]}");
                activePlan[activeActionIdx].OnActivate();
            }
        }
    }

    void OnCompleteActivePlan(){
        if (activePlan != null){
            activePlan[activeActionIdx].OnDeactivate();
        }
        activeGoal?.OnDeactivate();
        activeGoal = null;
        activePlan = null;
    }

    void OnFailActivePlan(){
        if (activePlan != null){
            activePlan[activeActionIdx].OnDeactivate();
        }
        activeGoal?.OnDeactivate();
        activeGoal = null;
        activePlan = null;
    }

    void GetHighestPriorityGoal(out GOAPGoal chosenGoal, out List<GOAPAction> chosenPlan){

        /**
         * Updates chosenGoal and chosenPlan with the highest priorty goal that 
         * has a valid plan
         */

        chosenGoal = null;
        chosenPlan = null;
        PlannerLog("Searching for highest priority goal", bold:true);
        if (goals == null){
            PlannerLog("No goals found");
            return;
        }

        for (int i = 0; i < goals.Count; i++){

            if (!goals[i].PreconditionsSatisfied(worldState)){
                PlannerLog($"{goals[i]} not valid as preconditions not satisfied");
                continue;
            }

            if (!(chosenGoal == null || HasHigherPriority(goals[i], chosenGoal))){
                continue;
            }

            if (chosenGoal != null){
                PlannerLog($"{goals[i]} has higher priority than {chosenGoal}");
            }

            List<GOAPAction> candidatePath = GetOptimalPath(
                currentState:worldState,
                goal:goals[i], 
                actions:actions[goals[i].actionLayer]
                );

            if (candidatePath != null){
                chosenGoal = goals[i];
                chosenPlan = candidatePath;
                PlannerLog($"Path found. Chosen goal is now {goals[i]}", bold:true);
            }
        }
    }

    bool HasHigherPriority(GOAPGoal goal, GOAPGoal other){
        return goal.GetPriority() > other.GetPriority();
    }

    //// A*

    List<GOAPAction> GetOptimalPath(
        WorldState currentState, 
        GOAPGoal goal, 
        List<GOAPAction> actions){


        /**
         * Uses A* searching algorithm to find the lowest cost path for goal
         */

        bool InList(GOAPAction action, List<ActionNode> nodeList){
            for(int i = 0; i < nodeList.Count; i++){
                if (nodeList[i].action == action){
                    return true;
                }
            }
            return false;
        }

        PlannerLog($"Searching for plan for {goal}", bold:true);

        List<ActionNode> openList = new List<ActionNode>();
        List<ActionNode> closedList = new List<ActionNode>();

        // Get starting node
        GOAPAction startAction = null;
        float minCost = -1;
        for (int i = 0; i< actions.Count; i++){
            if (actions[i].SatisfiesConditions(goal.conditions)){
                PlannerLog($"{actions[i]} satisfies goal conditions");
                float cost = actions[i].GetCost();
                if (minCost < 0 || cost < minCost){
                    minCost = cost;
                    startAction = actions[i];
                }
            }
        }

        // No path found
        if (startAction == null){
            PlannerLog($"No actions found to satisfy {goal} conditions");
            return null;
        }

        openList.Add(new ActionNode(null, startAction));
        PlannerLog($"Selected {startAction}", bold:true);

        while (openList.Count != 0){

            ActionNode currentNode = null;
            if (closedList.Count>0){
                currentNode = GetNextNode(
                    closedList:closedList,
                    openList:openList
                );
            }
            else{
                float nodeCost;
                currentNode = GetNextNode(
                    requiredState:goal.conditions,
                    openList:openList,
                    nodeCost: out nodeCost,
                    isStartNode:true
                );
            }

            if (currentNode == null){
                PlannerLog("No path found");
                return null;
            }
            closedList.Add(currentNode);            
            openList.Remove(currentNode);

            if (currentState.IsSubset(currentNode.action.preconditions)){
                return GeneratePath(closedList);
            }

            List<GOAPAction> linkedActions = GetLinkedActions(
                node:currentNode.action,
                availableNodes:actions
            );

            for (int i = 0; i < linkedActions.Count; i++){
                if (!InList(linkedActions[i], closedList)){
                    if (!InList(linkedActions[i], openList)){
                        openList.Add(new ActionNode(currentNode, linkedActions[i]));
                    }
                }
            }
        }

        PlannerLog("No path found.");
        return null;
    }

    List<GOAPAction> GeneratePath(List<ActionNode> closedList){

        /**
         * Iterates through parents starting with the last ActionNode in closedList
         * to return a list of GOAPActions.
         */

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
        List<ActionNode> closedList, 
        List<ActionNode> openList){

        /**
         * Finds the ActionNode in openList that satisfies
         * conditions of an ActionNode in closedList with the lowest cost.
         */

        float minCost = -1f;
        ActionNode nextNode=null;
        ActionNode currentNode;
        for (int i = 0; i < closedList.Count; i++){
            float nodeCost;
            currentNode = GetNextNode(
                closedList[i].action.preconditions,
                openList,
                out nodeCost
            );
            if ((minCost < 0 || nodeCost < minCost) && currentNode != null){
                nextNode = currentNode;
                minCost = nodeCost;
            }
        }
        if (nextNode!=null){
            PlannerLog($"Selected {nextNode.action}", bold:true);
        }
        else{
            PlannerLog("Could not find next action");
        }
        return nextNode;
    }

    ActionNode GetNextNode(
        Dictionary<string, bool> requiredState, 
        List<ActionNode> openList,
        out float nodeCost,
        bool isStartNode=false
        ){

            /**
             * Searches for the node in openList with the smallest
             * cost that satisfies requiredState
             */

            string logString = "";
            foreach(var i in requiredState){
                logString += $" {i.Key}={i.Value}, ";

            }
            float minCost = -1f;
            ActionNode nextNode = null;
            for (int i = 0; i < openList.Count; i++){
                if (!openList[i].action.SatisfiesConditions(requiredState)){
                    PlannerLog($"{openList[i].action} does not satisfy conditions");
                    continue;
                }
                if (!isStartNode){
                    PlannerLog($"{openList[i].action} satisfies conditions");
                }
                float cost = openList[i].action.GetCost();
                if (minCost < 0 || cost < minCost){
                    nextNode = openList[i];
                    minCost = cost;
                }
            }
            nodeCost = minCost;
            return nextNode;
        }

    List<GOAPAction> GetLinkedActions(
        GOAPAction node, 
        List<GOAPAction> availableNodes){

        /**
         * Searches availableNodes for all those that satisfy node.preconditions 
         * and that are not in path. 
         */

        PlannerLog($"Finding actions linked to {node}");
        List<GOAPAction> linkedNodes = new List<GOAPAction>();
        for (int i = 0; i < availableNodes.Count; i++){
            if (availableNodes[i].SatisfiesConditions(node.preconditions)){
                PlannerLog($"{availableNodes[i]} is linked to {node}");
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
                    goals[i].PreconditionsSatisfied(worldState)
                    )
            );
        }

        goalData.Sort((x, y) => x.priority.CompareTo(y.priority));
        goalData.Reverse();
        return goalData;
    }

    void ActivePlanLog(object message, bool bold=false){
        if(activePlanLogger){
            activePlanLogger.Log(message, this, bold:bold);
        }
    }

    void PlannerLog(object message, bool bold=false){
        if(plannerLogger){
            plannerLogger.Log(message, this, bold:bold);
        }
    }

}
}