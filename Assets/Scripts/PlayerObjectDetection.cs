using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectDetection : MonoBehaviour
{
    // object avoidance variables
    //public StateController controller;
    public GameObject eyes, lookPos;
    public float lookSphere;

    public LayerMask interactableLayers;
    public List<int> layerIndex;

    // object detection variables
    public List<GameObject> detectedObjects;
    public GameObject target;



    private void Awake()
    {
        for (int i = 0; i < layerIndex.Count; i++)
            Physics.IgnoreLayerCollision(9, layerIndex[i]);
    }
    void Update()
    {
        DetectObjects();

        if (detectedObjects != null)
        {
            target = detectedObjects[0];
        }
    }

    public void DetectObjects()
    {
        
        RaycastHit hit;

        if (Physics.Raycast(eyes.transform.position, lookPos.transform.position, out hit, lookSphere, interactableLayers) && !detectedObjects.Contains(hit.transform.gameObject)) //(Physics.SphereCast(eyes.transform.position, lookSphere, transform.forward, out hit, lookSphere, interactableLayers)
        {
            Debug.DrawLine(transform.position, hit.point, Color.cyan);
            detectedObjects.Add(hit.transform.gameObject);            
        }

        for (int i = 0; i < detectedObjects.Count; i++)
        {
            if (detectedObjects[i] != Physics.Raycast(eyes.transform.position, lookPos.transform.position, out hit, interactableLayers) && !detectedObjects.Contains(hit.transform.gameObject))
            {
                detectedObjects.Remove(detectedObjects[i]);
            }
        }

            detectedObjects.Sort(SortByDistanceToMe);

        //return objectsAroundAI;
    }

    int SortByDistanceToMe(GameObject a, GameObject b)
    {
        float squaredRangeA = (a.transform.position - transform.position).sqrMagnitude;
        float squaredRangeB = (b.transform.position - transform.position).sqrMagnitude;
        return squaredRangeA.CompareTo(squaredRangeB);
    }

    private void OnDrawGizmos()
    {
        //if (eyes != null)
        //{
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawWireSphere(eyes.transform.position, lookSphere);
        //}

        if (detectedObjects == null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(eyes.transform.position, lookPos.transform.position);
        }
        //else
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawRay(eyes.transform.position, target.transform.position);
        //}
    }

}
