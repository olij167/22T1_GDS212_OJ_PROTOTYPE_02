using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="PluggableAI/Actions/WaitAtBar")]

public class WaitAtBarAction : ActionSO
{
    public override void Act(StateController controller)
    {
        WaitAtBar(controller);
    }

    public void WaitAtBar(StateController controller)
    {
        controller.waitTime = 0;

        if (controller.gameObject.transform.position == controller.agent.destination)
        {
            controller.agent.isStopped = true;
            //controller.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX & RigidbodyConstraints.FreezePositionZ;
            
            controller.waitTime += Time.deltaTime;
            //Debug.Log("wait time = " + controller.waitTime);
            if (controller.player.GetComponent<ServeDrinks>().ableToServe)
            {
                controller.transform.LookAt(controller.player.transform);
            }
        }

        // after a drink has been served: controller.visitorStats.newDrinkTime = Random.Range(controller.visitorStats.minWait, controller.visitorStats.maxWait);

    }
}
