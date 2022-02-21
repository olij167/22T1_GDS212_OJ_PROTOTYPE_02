using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "PluggableAI/Decisions/ReactToBee")]
public class ReactToBee : Decision
{
    public override bool Decide(StateController controller)
    {
        bool reactToBee = ReactionToBee(controller);
        return reactToBee;
    }

    private bool ReactionToBee(StateController controller)
    {
        int coinflip = Random.Range(0, 2);

        if (coinflip == 0)
            return true;
        else return false;
    }
}
