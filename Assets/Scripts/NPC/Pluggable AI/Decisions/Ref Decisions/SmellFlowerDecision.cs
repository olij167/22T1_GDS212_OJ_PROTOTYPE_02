using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "PluggableAI/Decisions/SmellFlowers")]
public class SmellFlowerDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool seeAFlower = SeeFlower(controller);
        return seeAFlower;
    }

    private bool SeeFlower(StateController controller)
    {
        if (controller.target != null)
        {
            if (controller.detection.CompareTag("Flower"))
            {
                return true;
            }
            else return false;
        }
        else return false;
    }
}
