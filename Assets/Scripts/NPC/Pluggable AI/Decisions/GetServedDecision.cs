using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/GetServed")]
public class GetServedDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool getServed = GetServed(controller);

        return getServed;
    }

    private bool GetServed(StateController controller)
    {
        if (controller.beenServed)
        {
            return true;
        }
        else return false;
    }
}
