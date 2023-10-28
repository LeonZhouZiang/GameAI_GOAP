using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPatient : GAction
{
    private GameObject resource;
    public override bool PrePerform()
    {
        target = GWorld.Instance.RemovePatient();
        if(target == null)
        {
            return false;
        }
        resource = GWorld.Instance.RemoveCubicles();
        if(resource == null)
        {
            GWorld.Instance.AddPatient(target);
            target = null;
            return false;
        }
        inventory.AddItem(resource);
        GWorld.Instance.GetWorld().ModifyState("FreeCubicle", -1);
        target.GetComponent<GAgent>().beliefs.RemoveState("atHospital");
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("Waiting", -1);
        if (target)
        {
            target.GetComponent<GAgent>().inventory.AddItem(resource);
            target.GetComponent<GAgent>().beliefs.AddState("readyToCured", 1);
        }
        return true;
    }
}
