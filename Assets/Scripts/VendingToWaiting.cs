using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingToWaiting : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    public override bool PostPerform()
    {
        beliefs.ModifyState("atHospital", 1);
        GWorld.Instance.GetWorld().ModifyState("Waiting", 1);
        GWorld.Instance.AddPatient(gameObject);
        return true;
    }
}
