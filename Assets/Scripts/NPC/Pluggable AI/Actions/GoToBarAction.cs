using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/GoToBar")]

public class GoToBarAction : ActionSO
{
    public override void Act(StateController controller)
    {
        GoToBar(controller);
    }

    public void GoToBar(StateController controller)
    {
        //controller.visitorStats.waitTime = 0;

        //float barLength = Vector3.Distance(controller.barLeftEnd.transform.position, controller.barRightEnd.transform.position);

        controller.barSpot = new Vector3(controller.bar.transform.position.x - controller.visitorStats.lookSphere, controller.transform.position.y, Random.Range(controller.barLeftEnd.transform.position.z, controller.barRightEnd.transform.position.z));

        controller.agent.SetDestination(controller.barSpot);

        controller.agent.destination = controller.barSpot;

        if (Vector3.Distance(controller.barSpot, controller.transform.position) <= controller.visitorStats.lookSphere)
        {
            controller.waitingAtBar = true;
        }

        //if (controller.visitorStats.lookSphere)

        // after a drink has been served: controller.visitorStats.waitTime = Random.Range(controller.visitorStats.minWait, controller.visitorStats.maxWait);

    }
}
