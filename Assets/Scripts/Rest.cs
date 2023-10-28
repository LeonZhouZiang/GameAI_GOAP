using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rest : GAction
{
    public override bool PrePerform()
    {
        GWorld.Instance.GetWorld().ModifyState("WorkingNurse", -1);
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("WorkingNurse", 1);
        beliefs.RemoveState("exhausted");
        return true;
    }
}
