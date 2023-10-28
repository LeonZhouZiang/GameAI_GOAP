using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubGoal
{
    public Dictionary<string, int> sgoals;
    public bool remove;
    
    public SubGoal(string s, int i, bool r) 
    {
        sgoals = new Dictionary<string, int>
        {
            { s, i }
        };
        remove = r;

    }
}

public class GAgent : MonoBehaviour
{
    internal List<GAction> actions = new();
    internal Dictionary<SubGoal, int> goals = new();
    public GInventory inventory = new();
    public WorldStates beliefs = new();

    private GPlanner planner;
    private Queue<GAction> actionQueue;
    public GAction currentAction;
    public SubGoal currentGoal;

    protected virtual void Start()
    {
        GAction[] acts = GetComponents<GAction>();
        foreach (GAction act in acts)
        {
            actions.Add(act);
        }
    }

    bool invoked = false;
    private void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    private void OnDrawGizmos()
    {
        if(currentAction != null && currentAction.target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, currentAction.target.transform.position);
        }
    }

    private void LateUpdate()
    {
        if(currentAction != null && currentAction.running)
        {
            float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, transform.position);
            if(currentAction.agent.hasPath && distanceToTarget < 3f)
            {
                if (!invoked)
                {
                    Invoke(nameof(CompleteAction), currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        if(planner == null || actionQueue == null)
        {
            planner = new();
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            foreach(KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.Plan(actions, sg.Key.sgoals, beliefs);
                if(actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }   

        if(actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
        }

        if(actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                if(currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                }

                if(currentAction.target != null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            } else
            {
                Debug.Log("Action Failed: " + currentAction.actionName);
                actionQueue = null;
            }
        }
    }
}
