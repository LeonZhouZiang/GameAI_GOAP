using System;
using System.Collections;
using System.Collections.Generic;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public Node(Node parent, float cost, Dictionary<string, int> state, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(state);
        this.action = action;
    }

    public Node(Node parent, float cost, Dictionary<string, int> state, Dictionary<string, int> beliefState, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(state);
        foreach(KeyValuePair<string, int> kvp in beliefState)
        {
            this.state.TryAdd(kvp.Key, kvp.Value);
        }
        this.action = action;
    }
}

public class GPlanner
{
    public Queue<GAction> Plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates states)
    {
        List<GAction> usableActions = new();
        foreach (GAction action in actions)
        {
            if (action.IsAchievable()) { usableActions.Add(action); }
        }

        List<Node> leaves = new();
        Node start = new(null, 0, GWorld.Instance.GetWorld().GetStates(), states.GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);
        if(!success)
        {
            return null;
        }

        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if(cheapest == null)
            {
                cheapest = leaf;
            } else
            {
                if(leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        Stack<GAction> result = new();
        Node n = cheapest;
        while(n != null)
        {
            if(n.action != null)
            {
                result.Push(n.action);
            }
            n = n.parent;
        }
        Queue<GAction> queue = new();
        while(result.Count > 0)
        {
            queue.Enqueue(result.Pop());
        }
        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> useableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach(GAction gAction in useableActions)
        {
            if (gAction.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach(KeyValuePair<string, int> eff in gAction.effects)
                {
                    if (!currentState.ContainsKey(eff.Key))
                    {
                        currentState.Add(eff.Key, eff.Value);
                    }
                }
                
                Node node = new(parent, parent.cost + gAction.cost, currentState, gAction);
                if(GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                } else
                {
                    List<GAction> subset = ActionSubset(useableActions, gAction);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if(found) foundPath = true;
                }
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key)) return false;
        }
        return true;
    }

    private List<GAction> ActionSubset(List<GAction> actions, GAction gAction)
    {
        List<GAction> subset = new List<GAction>();
        foreach(GAction action in actions)
        {
            if(!action.Equals(gAction))
            {
                subset.Add(action);
            }
        }
        return subset;
    }
}
