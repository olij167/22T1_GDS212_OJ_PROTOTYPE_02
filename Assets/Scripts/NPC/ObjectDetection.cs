using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
   // object avoidance variables
    public StateMachine controller;
    //public LayerMask ignoreMasks;
    //public List<int> layerIndex;
    //public State goToBarState;
    public float lookDistance;
    Vector3 target;
    bool obstacleAvoid;
    //public bool beeInLookSphere;

    private Transform obstacleInPath;

    // object detection variables
    public GameObject player;



    //public LayerMask interestingLayers;

    private void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
       DetectObjects();
        //ObstacleAvoidance();
        if (player != null)
        {
            Debug.Log("Player seen");
            if (player.transform.gameObject.GetComponent<ServeDrinks>().isSucking)
            {
                Debug.Log("Player Sucking");
                if (!controller.isWitness)
                {
                    Debug.Log("is a witness");
                    controller.isWitness = true;
                    //controller.currentState = "Witness";
                    //controller.stateColour = Color.red;
                    player.GetComponent<ServeDrinks>().witnessCount += 1;
                }


            }
        }

    }

    public void DetectObjects()
    {
        //int interestingLayer = 1 << 13;
        RaycastHit hit;

        if (Physics.SphereCast(controller.eyes.transform.position, controller.lookSphere, transform.forward, out hit, controller.lookSphere))
        {
            if (hit.transform.gameObject.CompareTag("Wall"))
            {
                player = null;
            }

            if (hit.transform.gameObject.CompareTag("Player"))
            {
                //transform.LookAt(hit.transform);
                Debug.DrawLine(transform.position, hit.point, Color.yellow);

                player = hit.transform.gameObject;
                

                //objectAroundAI.GetComponent<ServeDrinks>().isSeen = true;

                
            }
            
        }
        else player = null;
    }

    private void ObstacleAvoidance()
    {
        target = controller.wayPoint;

        RaycastHit hit;
        Vector3 dir = (target - transform.position).normalized;

        if (Physics.SphereCast(controller.eyes.transform.position, controller.lookSphere, transform.forward, out hit, controller.lookSphere))
        {
            obstacleAvoid = true;
            Debug.DrawLine(transform.position, hit.point, Color.red);
            controller.agent.isStopped = true;
            controller.agent.ResetPath();

            if (hit.transform != transform)
            {
                obstacleInPath = hit.transform;
                //Debug.Log("Obstacle in Path = " + hit.transform.gameObject.name);
                dir += hit.normal; //* controller.agent.;

                //Debug.Log("Moving around an object");
            }
        }

        if (obstacleInPath != null)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 toOther = obstacleInPath.position - transform.position;
            if (Vector3.Dot(forward, toOther) < 0)
            {
                //print("The other transform is behind me!");
                //Debug.Log("Back on Navigation! unit - " + gameObject.name);
                obstacleAvoid = false; // don't let Unity nav and our avoidance nav fight, character does odd things
                obstacleInPath = null; // Hakuna Matata
                controller.agent.ResetPath();
                controller.agent.SetDestination(controller.wayPoint);
                controller.agent.isStopped = true; // Unity nav can resume movement control
            }
        }

        if (obstacleAvoid)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);
            transform.position += transform.forward * controller.agent.speed * Time.deltaTime;
        }
    }


}
