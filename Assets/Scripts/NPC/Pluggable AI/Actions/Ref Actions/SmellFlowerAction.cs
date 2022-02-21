using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "PluggableAI/Actions/SmellFlower")]
public class SmellFlowerAction : ActionSO
{
    public override void Act(StateController controller)
    {
        SmellFlower(controller);
    }

    public void SmellFlower(StateController controller)
    {
        controller.stateTime -= Time.deltaTime;
        //Debug.Log(controller.stateTime);

        Vector3 flowerStoppingOffset = new Vector3(2.5f, 0f, 2.5f);

        controller.transform.LookAt(controller.target.transform);
        controller.agent.SetDestination(controller.target.transform.position - flowerStoppingOffset);

        Debug.DrawLine(controller.transform.position, controller.target.transform.position, Color.blue);

        if (controller.stateTime <= 0)
        {
            controller.target = null;
        }
    }
}
