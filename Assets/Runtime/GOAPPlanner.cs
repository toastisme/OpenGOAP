using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;

namespace GOAP{
public class GOAPPlanner : MonoBehaviour
{
    List<Goal> goals;
    List<GOAPAction> actions;

    Goal activeGoal;
    Goal bestGoal;
    GOAPAction activeAction;
    GOAPAction bestAction;

    Dictionary<Goal, List<GOAPAction> > GoalActionMap; 

    public bool displayPlanner = false;
    bool displayingPlanner = false;

    void Awake(){
        goals = new List<Goal>(GetComponents<Goal>());
        actions = new List<GOAPAction>(GetComponents<GOAPAction>());
        UpdateGoalActionMap();
    }

    void Update(){

        UpdateBestOptions();

        if ((NoActiveGoal() && GoalAvailable()) || BetterGoalAvailable()){
            StartCurrentBestGoal();
        } 
        else if (BetterActionAvailable()){
            StartCurrentBestAction();
        }

        UpdateActiveAction();

        UpdateDisplayPlanner();

    }

    bool NoActiveGoal(){
        return activeGoal == null;
    }

    bool BetterGoalAvailable(){
        return bestGoal != null && bestGoal != activeGoal;
    }

    bool GoalAvailable(){
        return bestAction != null && bestGoal != null;
    }

    void StartCurrentBestGoal(){
        if (activeGoal != null){
            activeGoal.OnDeactivated();
        }
        if (activeAction != null){
            activeAction.OnDeactivated();
        }

        activeGoal = bestGoal;
        activeAction = bestAction;
        activeGoal.OnActivated(activeAction);
        activeAction.OnActivated(activeGoal);
    }

    bool ActionAvailable(){
        return bestAction != null;
    }

    bool BetterActionAvailable(){
        return bestAction != null && bestAction != activeAction;
    }

    void StartCurrentBestAction(){
        Assert.IsTrue(activeGoal != null);
        Assert.IsTrue(bestAction != null);
        if (activeAction != null){
            activeAction.OnDeactivated();
        }
        activeAction = bestAction;
        bestAction.OnActivated(activeGoal);
    }

    void UpdateActiveAction(){
        if (activeAction != null){
            activeAction.OnTick();
        }
    }

    void UpdateBestOptions(){
        bestGoal = null;
        bestAction = null;
        GetHighestPriorityGoal(chosenGoal:out bestGoal, chosenAction:out bestAction);
    }

    void UpdateGoalActionMap(){
        if (goals == null || actions == null){
            return;
        }
        GoalActionMap = new Dictionary<Goal, List<GOAPAction>>();
        for (int i = 0; i < goals.Count; i++){
            List<GOAPAction> goalActions = new List<GOAPAction>();
            for (int j = 0; j < actions.Count; j++){
                if (actions[j].GetSupportedGoals().Contains(goals[i].GetType())){
                    goalActions.Add(actions[j]);
                }
            }
            GoalActionMap[goals[i]] = goalActions;
        }

    }

    void GetHighestPriorityGoal(out Goal chosenGoal, out GOAPAction chosenAction){


        chosenGoal = null;
        chosenAction = null;
        if (goals == null){return;}

        for (int i = 0; i < goals.Count; i++){

            goals[i].OnTick();

            if (!goals[i].CanRun()){
                continue;
            }

            if (!(chosenGoal == null || HasHigherPriority(goals[i], chosenGoal))){
                continue;
            }

            GOAPAction candidateAction = GetLowestCostAction(GoalActionMap[goals[i]]);

            if (candidateAction != null){
                chosenGoal = goals[i];
                chosenAction = candidateAction;
            }
        }
    }

    bool HasHigherPriority(Goal goal, Goal other){
        return goal.CalculatePriority() > other.CalculatePriority();
    }

    bool HasLowerCost(GOAPAction _action, GOAPAction other){
        return _action.GetCost() < other.GetCost();
    }

    GOAPAction GetLowestCostAction(List<GOAPAction> _actions){
        if (_actions == null || _actions.Count == 0){
            return null;
        }
        GOAPAction chosenAction = null;
        for (int i = 0; i < _actions.Count; i++){
            if (chosenAction == null || HasLowerCost(_actions[i], chosenAction)){
                chosenAction = _actions[i];
            }
        }       
        return chosenAction;
    }

    public Dictionary<Goal, List<GOAPAction>> GetGoalActionMap(){
        return GoalActionMap;
    }

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
        GOAPGUI window = EditorWindow.GetWindow(typeof(GOAPGUI)) as GOAPGUI;
        window.SetPlanner(this);
    }

    void StopDisplayingPlanner(){
        displayingPlanner = false;
    }

}
}