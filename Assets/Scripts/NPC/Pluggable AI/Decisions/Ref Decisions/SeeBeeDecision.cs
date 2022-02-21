using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "PluggableAI/Decisions/SeeBee")]
public class SeeBeeDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool seeABee = SeeBee(controller);

        //bool reactToBee = ReactionToBee(controller);
        return seeABee;

        //if (seeABee)
        //{
        //    bool reactToBee = ReactionToBee(controller);
        //    return reactToBee;
        //}
        //else return false;
    }

    private bool SeeBee(StateController controller)
    {
        if (controller.target != null) // not right
        {
            return true;
        }
        else return false;
    }

    //private bool ReactionToBee(StateController controller)
    //{
    //    int coinflip = Random.Range(0, 2);

    //    if (coinflip == 0)
    //        return true;
    //    else return false;
    //}
}
