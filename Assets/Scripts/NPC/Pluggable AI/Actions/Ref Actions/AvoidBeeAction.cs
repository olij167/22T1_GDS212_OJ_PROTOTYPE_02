using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "PluggableAI/Actions/AvoidBees")]
public class AvoidBeeAction : ActionSO
{
    public override void Act(StateController controller)
    {
        AvoidBees(controller);
    }

    public void AvoidBees(StateController controller)
    {
        controller.stateTime -= Time.deltaTime;
        //Debug.Log(controller.stateTime);

        Vector3 beeAvoidDestination = new Vector3(-controller.target.transform.position.x, controller.transform.position.y, -controller.target.transform.position.z);

        controller.transform.LookAt(-controller.target.transform.position);
        controller.agent.SetDestination(beeAvoidDestination);

        Debug.DrawLine(controller.transform.position, controller.target.transform.position, Color.blue);

        //if (controller.stateTime <= 0 || !controller.detection.detectedObjects.Contains(controller.target))
        //{
        //    controller.target = null;
        //}
    }
}
