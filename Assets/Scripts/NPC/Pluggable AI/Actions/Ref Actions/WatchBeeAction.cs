using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "PluggableAI/Actions/WatchBees")]
public class WatchBeeAction : ActionSO
{
    public override void Act(StateController controller)
    {
        WatchBees(controller);
    }

    public void WatchBees(StateController controller)
    {
        controller.stateTime -= Time.deltaTime;
        //Debug.Log(controller.stateTime);

        Vector3 beeFollowOffset = new Vector3(2.5f, 0f, 2.5f);    

        controller.transform.LookAt(controller.target.transform);
        controller.agent.SetDestination(controller.target.transform.position - beeFollowOffset);

        Debug.DrawLine(controller.transform.position, controller.target.transform.position, Color.blue);

        if (controller.stateTime <= 0)
        {
            controller.target = null;
        }
    }
}
