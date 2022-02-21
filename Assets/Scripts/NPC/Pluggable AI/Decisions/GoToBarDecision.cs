using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/GoToBar")]

public class GoToBarDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool goToBar = GoToBar(controller);

        //bool reactToBee = ReactionToBee(controller);
        return goToBar;

        //if (seeABee)
        //{
        //    bool reactToBee = ReactionToBee(controller);
        //    return reactToBee;
        //}
        //else return false;
    }

    private bool GoToBar(StateController controller)
    {
        if (controller.newStateTime <= 0)
        {
            return true;
        }
        else return false;
    }
}
