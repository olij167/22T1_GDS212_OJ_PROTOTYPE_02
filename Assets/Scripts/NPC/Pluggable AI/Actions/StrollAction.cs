using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="PluggableAI/Actions/Stroll")]
public class StrollAction : ActionSO
{
    public override void Act(StateController controller)
    {
        Stroll(controller);
    }

    public void Stroll(StateController controller)
    {
        controller.agent.isStopped = false;
        //if (controller.newStateTime <= controller.visitorStats.minWait)
        //{
        //    controller.newStateTime = Random.Range(controller.visitorStats.minWait, controller.visitorStats.maxWait);
            
        //}
        //if (controller.visitorStats.strollTime < controller.visitorStats.minWait)
        //{
        //    controller.visitorStats.strollTime = Random.Range(controller.visitorStats.minWait, controller.visitorStats.maxWait);
        //}

        controller.newStateTime -= Time.deltaTime;
        
        if (Vector3.Distance(controller.transform.position, controller.strollPoint) <= controller.visitorStats.lookSphere)
        {
            controller.strollPoint = controller.GenerateStrollPoint();
        }

        
        
        controller.agent.destination = controller.strollPoint;

        Debug.DrawLine(controller.transform.position, controller.strollPoint, Color.blue);

        //Debug.Log("AI Strolling to: " + controller.strollPoint);
        //Debug.Log("AI Stroll time: " + controller.strollTime);
        controller.agent.isStopped = false; // ensure visitor is strolling
        controller.waitingAtBar = false;
        

        //if (controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending)
        //{
        //    controller.strollPoint = controller.GenerateStrollPoint();
        //    Debug.Log("new stroll point generated");
        //}
    }


}
