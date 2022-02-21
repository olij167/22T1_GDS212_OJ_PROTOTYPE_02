using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAI : MonoBehaviour
{
    StateController controller;
    Vector3 startingStrollPoint;
    float startingStrollTime;
    void Start()
    {
        controller = GetComponent<StateController>();
        startingStrollPoint = controller.GenerateStrollPoint();
        //startingStrollTime = controller.GenerateStateTime();
        //controller.detection.beeInLookSphere = false;

        controller.SetupAI(true, startingStrollPoint);
    }
}
