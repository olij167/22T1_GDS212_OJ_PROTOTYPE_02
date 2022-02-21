using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/Stroll")]
public class StrollDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool startStroll = StartStrolling(controller);
        return startStroll;
    }

    private bool StartStrolling(StateController controller)
    {
        if (controller.beenServed == true)
        {
            return true;
        }
        else return false;
    }
}
