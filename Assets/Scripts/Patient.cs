using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : GAgent
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SubGoal s1 = new("isWaiting", 1, true);
        goals.Add(s1, 5);

        SubGoal s2 = new("isTreated", 1, true);
        goals.Add(s2, 5);

        SubGoal s3 = new("isHome", 1, true);
        goals.Add(s3, 7);

        SubGoal s4 = new("satisfied", 1, false);
        goals.Add(s4, 6);

        Invoke(nameof(GetThirsty), Random.Range(20, 30));

    }

    private void GetThirsty()
    {
        beliefs.SetState("thirsty", 1);
        Invoke(nameof(GetThirsty), Random.Range(20, 30));
    }
}
