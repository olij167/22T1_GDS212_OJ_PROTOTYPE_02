using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public State currentState;
    public VisitorStats visitorStats;
    public Transform eyes;
    public State remainState;
    [HideInInspector] public GameObject bar, barLeftEnd, barRightEnd, player;
    //[HideInInspector] public float newDrinkTime;

    [HideInInspector] public NavMeshAgent agent;

    [HideInInspector] public float strollRange, stateTime; //minStrollRange, maxStrollRange, minStateTime, maxStateTime,
    [HideInInspector] public Vector3 strollPoint;
    [HideInInspector] public bool waitingAtBar, beenServed;
    [HideInInspector] public ObjectDetection detection;
    [HideInInspector] public Vector3 barSpot;

    [HideInInspector] public GameObject target;

    [HideInInspector] public float waitTime, newStateTime;


    private bool aiActive;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        detection = GetComponent<ObjectDetection>();
        newStateTime = Random.Range(visitorStats.minWait, visitorStats.maxWait);
        bar = GameObject.FindGameObjectWithTag("Bar");
        barLeftEnd = bar.transform.GetChild(0).gameObject;
        barRightEnd = bar.transform.GetChild(1).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        //stateTime = GenerateStateTime();
    }

    public void SetupAI (bool aiActivation, Vector3 waypoint)
    {
        strollPoint = waypoint;
        aiActive = aiActivation;
        if (aiActive)
        {
            agent.enabled = true;
        }
        else agent.enabled = false;
    }

    private void Update()
    {
        if (!aiActive) return;

        currentState.UpdateState(this);

        
    }

    public Vector3 GenerateStrollPoint()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        int maxIndices = navMeshData.indices.Length - 3;

        // pick the first indice of a random triangle in the nav mesh
        int firstVertexSelected = UnityEngine.Random.Range(0, maxIndices);
        int secondVertexSelected = UnityEngine.Random.Range(0, maxIndices);

        // spawn on verticies
        Vector3 point = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];

        Vector3 firstVertexPosition = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
        Vector3 secondVertexPosition = navMeshData.vertices[navMeshData.indices[secondVertexSelected]];

        // eliminate points that share a similar X or Z position to stop spawining in square grid line formations
        if ((int)firstVertexPosition.x == (int)secondVertexPosition.x || (int)firstVertexPosition.z == (int)secondVertexPosition.z)
        {
            point = GenerateStrollPoint(); // re-roll a position - I'm not happy with this recursion it could be better
        }
        else
        {
            // select a random point on it
            point = Vector3.Lerp(firstVertexPosition, secondVertexPosition, UnityEngine.Random.Range(0.05f, 0.95f));
        }

        return point;
    }

    //public float GenerateStateTime()
    //{
    //    stateTime = Random.Range(visitorStats.minStateTime, visitorStats.maxStateTime);
    //    return stateTime;
    //}

    private void OnDrawGizmos()
    {
        if (currentState != null && eyes != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(eyes.position, visitorStats.lookSphere);
        }
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
        }
    }
}
