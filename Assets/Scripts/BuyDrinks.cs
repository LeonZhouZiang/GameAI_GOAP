using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyDrinks : GAction
{
    public override bool PrePerform()
    {
        GameObject machine = GWorld.Instance.RemoveMachine();
        if(machine == null){
            return false;
        }
        if (!GWorld.Instance.RemovePatient(gameObject))
        {
            GWorld.Instance.AddMachine(machine);
            return false;
        }
        target = machine;
        inventory.AddItem(machine);
        GWorld.Instance.GetWorld().ModifyState("FreeVendingMachine", -1);

        beliefs.RemoveState("atHospital");
        GWorld.Instance.GetWorld().ModifyState("Waiting", -1);
        return true;
    }

    public override bool PostPerform()
    {
        inventory.RemoveItem(target);
        GWorld.Instance.AddMachine(target);
        GWorld.Instance.GetWorld().ModifyState("FreeVendingMachine", 1);
        GWorld.Instance.GetWorld().ModifyState("DrinksSold", 1);
        beliefs.RemoveState("thirsty");
        return true;
    }
}
