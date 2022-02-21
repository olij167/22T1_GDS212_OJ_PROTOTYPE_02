using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/WaitAtBar")]
public class WaitAtBarDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool atBar = AtBar(controller);

        //bool reactToBee = ReactionToBee(controller);
        return atBar;

        //if (seeABee)
        //{
        //    bool reactToBee = ReactionToBee(controller);
        //    return reactToBee;
        //}
        //else return false;
    }

    private bool AtBar(StateController controller)
    {
        if (controller.waitingAtBar)
        {
            return true;
        }
        else return false;
    }
}
