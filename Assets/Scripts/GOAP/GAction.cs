using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target;
    public string targetTag;
    public float duration = 0;
    public WorldState[] preConditions;
    public WorldState[] afterEffects;
    internal NavMeshAgent agent;
    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> effects;

    public GInventory inventory;
    public WorldStates beliefs;

    internal bool running = false;

    public GAction()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if(preConditions != null)
        {
            foreach(WorldState worldState in preConditions)
            {
                preconditions.Add(worldState.key, worldState.value);
            }
        }
        if(afterEffects != null)
        {
            foreach(WorldState worldState in afterEffects)
            {
                effects.Add(worldState.key, worldState.value);
            }
        }
        inventory = GetComponent<GAgent>().inventory;
        beliefs =  GetComponent<GAgent>().beliefs;
    }

    public bool IsAchievable()
    {
        return true;
    }

    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach(KeyValuePair<string, int> condition in preconditions)
        {
            if (!conditions.ContainsKey(condition.Key)) {
                return false;
            } else
            {
                if (condition.Value > conditions[condition.Key])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
