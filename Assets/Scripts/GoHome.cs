using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHome : GAction
{
    public override bool PrePerform()
    {
        if (gameObject.CompareTag("Nurse"))
        {
            GWorld.Instance.GetWorld().ModifyState("WorkingNurse", -1);
        }
        return true;
    }

    public override bool PostPerform()
    {
        Destroy(gameObject);
        if (gameObject.CompareTag("Nurse"))
        {
            Spawner.instance.SpawnNurse();
        }
        return true;
    }
}
