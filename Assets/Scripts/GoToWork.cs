using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWork : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        beliefs.ModifyState("isWorking", 1);
        GWorld.Instance.GetWorld().ModifyState("WorkingNurse", 1);
        return true;
    }
}
