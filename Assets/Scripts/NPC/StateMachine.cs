using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class StateMachine : MonoBehaviour
{
    public string currentState;
    public TextMeshProUGUI commentText;
    public List<string> goToBarComments, goToDanceComments, danceComments, goToToiletComments, witnessComments;

    public List<AudioClip> witnessAudio;

    AudioSource voicBox;
    
    public float lookSphere, minStateTime, maxStateTime, waitTime, stateTime, personalRating, commentTimer = 1f, commentTimerReset = 1f, maxWaitTime = 30f;

    public Camera cam;

    public NavMeshTriangulation danceFloorMeshData;
    
    public NavMeshAgent agent;
    public Vector3 wayPoint, barSpot, danceSpot;

    public bool isWaiting, isDrinking, hasDrunk, isPeeing, justPeed, inState, isGrounded, decreaseStateTime, enteredToilet, isWitness, isBitten;

    public GameObject eyes;

    public Color stateColour;
    [HideInInspector] public GameObject bar, barLeftEnd, barRightEnd, danceFloor, danceFloorLeftEnd, danceFloorRightEnd, toilet, player, entrance; //toiletEntrance,

    Color originalColour;

    float distance;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        wayPoint = new Vector3();

        bar = GameObject.FindGameObjectWithTag("Bar");
        barLeftEnd = bar.transform.GetChild(0).gameObject;
        barRightEnd = bar.transform.GetChild(1).gameObject;

        danceFloor = GameObject.FindGameObjectWithTag("DanceFloor");
        danceFloorLeftEnd = danceFloor.transform.GetChild(0).gameObject;
        danceFloorRightEnd = danceFloor.transform.GetChild(1).gameObject;

        toilet = GameObject.FindGameObjectWithTag("Toilet");

        player = GameObject.FindGameObjectWithTag("Player");

        entrance = GameObject.FindGameObjectWithTag("Entrance");

        originalColour = transform.GetChild(1).GetComponent<MeshRenderer>().material.color;

        cam = Camera.main;

        voicBox = transform.GetComponent<AudioSource>();

        inState = false;
        isDrinking = false;
        hasDrunk = false;
        justPeed = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(wayPoint, transform.position);

        commentText.transform.LookAt(cam.transform);
        commentText.transform.Rotate(0, 180, 0);


        if (player.GetComponent<ServeDrinks>().customer == gameObject)
        {
            transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
        else transform.GetChild(1).GetComponent<MeshRenderer>().material.color = originalColour;

        if (isWitness)
        {
            Leave();
            inState = true;
        }

        if (isPeeing)
        {
            commentText.text = ((int)stateTime).ToString();
        }

        if (isWaiting)
        {
            waitTime += Time.deltaTime;
            commentText.text = ((int)waitTime).ToString();

            //transform.LookAt(player.transform);
        }

        if (isDrinking && isWaiting || waitTime >= maxWaitTime)
        {
            isWaiting = false;
            waitTime = 0f;
            inState = false;
        }

        if (!inState)
        {
            currentState = null;
            stateColour = Color.gray;
            ChooseState();
        }

        if (decreaseStateTime)
        {
            stateTime -= Time.deltaTime;

            if (stateTime <= 0)
            {
                GetComponent<Rotator>().enabled = false;

                if (isDrinking)
                {
                    Destroy(transform.GetChild(0).GetChild(0).gameObject);
                    isDrinking = false;
                    hasDrunk = true;
                    
                }

                if (justPeed)
                {
                    justPeed = false;
                }

                if (isPeeing)
                {
                    justPeed = true;
                    isPeeing = false;
                }


                inState = false;
                currentState = null;

                //if (isWitness)
                //{
                //    Leave();
                //}
            }
        }

        

        if (isBitten)
        {
            transform.GetChild(1).GetComponent<MeshRenderer>().material.color = player.GetComponent<ServeDrinks>().suckedColour;

            if (isWitness)
            {
                player.GetComponent<ServeDrinks>().witnessCount -= 1;
                player.GetComponent<ServeDrinks>().witnessEscapeCount -= 1;

                isWitness = false;
            }

            Wander();
        }

        if (Vector3.Distance(transform.position, barSpot) <= lookSphere && currentState == "Go To Bar")
        {
            WaitAtBar();
        }

        if (currentState == "Wait at Bar" && isDrinking)
        {
            ChooseState();
            //commentText.text = "";
        }

        if (Vector3.Distance(transform.position, toilet.transform.position) <= lookSphere && currentState == "Go To Toilet")
        {
            UseToilet();
        }
        
        if (Vector3.Distance(transform.position, danceSpot) <= lookSphere && currentState == "Go To DanceFloor")
        {
            Dance();
        }
    }

    void ChooseState()
    {
        decreaseStateTime = false;
        int coinFlip;

        //if (isWitness)
        //{
        //    Leave();
        //    inState = true;
        //}

        if (!hasDrunk && !isDrinking && !justPeed && !isWitness) // false, false, false
        {
            coinFlip = Random.Range(0, 3);
            if (coinFlip == 0)
            {
                Wander();
                inState = true;
            }

            if (coinFlip == 1)
            {
                GoToBar();
                inState = true;
            }

            if (coinFlip == 2)
            {
                GoToDanceFloor();
                inState = true;
            }

            //if (coinFlip == 3)
            //{
            //    GoToToilet();
            //    inState = true;
            //}
        }

        if (!hasDrunk && isDrinking && !justPeed && !isWitness) // false, true, false
        {
            coinFlip = Random.Range(0, 3);

            if (coinFlip == 0)
            {
                Wander();
                inState = true;
            }

            //if (coinFlip == 1)
            //{
            //    GoToBar();
            //    inState = true;
            //}

            if (coinFlip == 2)
            {
                GoToDanceFloor();
                inState = true;
            }

            if (coinFlip == 1)
            {
                GoToToilet();
                inState = true;
            }
        }

        if (hasDrunk && !isDrinking && justPeed && !isWitness) // true, false, true
        {
            coinFlip = Random.Range(0, 3);

            if (coinFlip == 0)
            {
                Wander();
                inState = true;
            }

            if (coinFlip == 1)
            {
                GoToBar();
                inState = true;
            }

            if (coinFlip == 2)
            {
                GoToDanceFloor();
                inState = true;
            }

            //if (coinFlip == 3)
            //{
            //    GoToToilet();
            //    inState = true;
            //}
        }


        if (hasDrunk && !isDrinking && !justPeed && !isWitness)// true, false, false
        {
            coinFlip = Random.Range(0, 4);
            if (coinFlip == 0)
            {
                Wander();
                inState = true;
            }

            if (coinFlip == 1)
            {
                GoToBar();
                inState = true;
            }

            if (coinFlip == 2)
            {
                GoToDanceFloor();
                inState = true;
            }

            if (coinFlip == 3)
            {
                GoToToilet();
                inState = true;
            }
        }


        if (!hasDrunk && isDrinking && justPeed && !isWitness)// false, true, true
        {
            coinFlip = Random.Range(0, 2);
            if (coinFlip == 0)
            {
                Wander();
                inState = true;
            }

            //if (coinFlip == 1)
            //{
            //    GoToBar();
            //    inState = true;
            //}

            if (coinFlip == 1)
            {
                GoToDanceFloor();
                inState = true;
            }

            //if (coinFlip == 3)
            //{
            //    GoToToilet();
            //    inState = true;
            //}
        }

        if (!hasDrunk && !isDrinking && justPeed && !isWitness)// false, false, true
        {
            coinFlip = Random.Range(0, 3);

            if (coinFlip == 0)
            {
                Wander();
                inState = true;
            }

            if (coinFlip == 1)
            {
                GoToBar();
                inState = true;
            }

            if (coinFlip == 2)
            {
                GoToDanceFloor();
                inState = true;
            }

            //if (coinFlip == 3)
            //{
            //    GoToToilet();
            //    inState = true;
            //}
        }

        if (hasDrunk && isDrinking && !justPeed && !isWitness)// true, true, false
        {
            coinFlip = Random.Range(0, 3);

            if (coinFlip == 0)
            {
                Wander();
                inState = true;
            }

            //if (coinFlip == 1)
            //{
            //    GoToBar();
            //    inState = true;
            //}

            if (coinFlip == 2)
            {
                GoToDanceFloor();
                inState = true;
            }

            if (coinFlip == 1)
            {
                GoToToilet();
                inState = true;
            }
        }

        if (hasDrunk && isDrinking && justPeed && !isWitness)// true, true, true
        {
            coinFlip = Random.Range(0, 2);

            if (coinFlip == 0)
            {
                Wander();
                inState = true;
            }

            //if (coinFlip == 1)
            //{
            //    GoToBar();
            //    inState = true;
            //}

            if (coinFlip == 1)
            {
                GoToDanceFloor();
                inState = true;
            }

            //if (coinFlip == 3)
            //{
            //    GoToToilet();
            //    inState = true;
            //}
        }
    }

    void Wander()
    {

        stateColour = Color.white;
        currentState = "Wander";
        //inState = true;

        stateTime = Random.Range(minStateTime, maxInclusive:maxStateTime);
        wayPoint = GenerateRandomWayPoint();
        decreaseStateTime = true;
        commentText.text = "";
        agent.SetDestination(wayPoint);

        //inState = true;
        //stateTime -= Time.deltaTime;

        if (transform.position == wayPoint)
        {
            wayPoint = GenerateRandomWayPoint();
        }

        //if (stateTime <= 0)
        //{
        //    inState = false;

        //}
        Debug.Log("Wander State");
    }
    
    void GoToBar()
    {
        currentState = "Go To Bar";
        barSpot = new Vector3(bar.transform.position.x, transform.position.y, Random.Range(barLeftEnd.transform.position.z, barRightEnd.transform.position.z));
        wayPoint = barSpot;
        commentText.text = goToBarComments[Random.Range(0, goToBarComments.Count)];
        //inState = true;
        stateColour = Color.blue;
        decreaseStateTime = false;
        

        agent.SetDestination(wayPoint);

       
        Debug.Log("Go To Bar State");
    }

    void WaitAtBar()
    {
        currentState = "Wait at Bar";
        
        //inState = true;
        stateColour = Color.magenta;
        decreaseStateTime = false;
        

        if (!isDrinking)
        {
            isWaiting = true;
        }
        else
        {
            isWaiting = false;
            inState = false;
            currentState = null;
            commentText.text = "";
        }
        Debug.Log("Wait at Bar State");
    }

    void GoToDanceFloor()
    {
        currentState = "Go To DanceFloor";
        commentText.text = goToDanceComments[Random.Range(0, goToDanceComments.Count)];
        //inState = true;
        stateColour = Color.cyan;
        decreaseStateTime = false;
        danceSpot = new Vector3(danceFloor.transform.position.x, transform.position.y, Random.Range(danceFloorLeftEnd.transform.position.z, danceFloorRightEnd.transform.position.z));
        wayPoint = danceSpot;
        //wayPoint = GenerateRandomDancePoint();

        agent.SetDestination(wayPoint);

        if (distance <= lookSphere)
        {
            Dance();
        }
        Debug.Log("Go To Dance Floor State");
    }

    void Dance()
    {
        currentState = "Dance";
        commentText.text = danceComments[Random.Range(0, danceComments.Count)];
        //inState = true;
        stateColour = Color.green;
        stateTime = Random.Range(minStateTime, maxInclusive: maxStateTime);
        stateTime -= Time.deltaTime;
        decreaseStateTime = true;

        GetComponent<Rotator>().enabled = true;

        //if (stateTime <= 0)
        //{
        //    GetComponent<Rotator>().enabled = false;
        //    //inState = false;
        //}
        Debug.Log("Dance State");
    }

    void GoToToilet()
    {
        
        currentState = "Go To Toilet";
        wayPoint = toilet.transform.position;
        commentText.text = goToToiletComments[Random.Range(0, goToToiletComments.Count)];
        //inState = true;
        stateColour = Color.yellow;
        decreaseStateTime = false;
        

        

        agent.SetDestination(wayPoint);

        //bool enteredToilet = false;
        
        //if (transform.position == wayPoint && !enteredToilet)
        //{
        //    wayPoint = toilet.transform.position;
        //    enteredToilet = true;
        //}

        if (transform.position == toilet.transform.position)
        {
            UseToilet();
        }

        Debug.Log("Go To Toilet");
    }

    void UseToilet()
    {
        currentState = "Use Toilet";
        //inState = true;
        //stateColour = Color.yellow;
        stateTime = Random.Range(minStateTime, maxInclusive: maxStateTime);
        decreaseStateTime = true;

        isPeeing = true;

        

        //wayPoint = toilet.transform.position;

        stateTime = Random.Range(minStateTime, maxStateTime);

        stateTime -= Time.deltaTime;
       

        //if (stateTime <= 0)
        //{
        //    hasDrunk = false;
        //    justPeed = true;
        //    //inState = false;
        //}
        Debug.Log("Use Toilet State");
    }

    void Leave()
    {
        
        if (currentState != "Leaving")
        {
            commentText.text = witnessComments[Random.Range(0, witnessComments.Count)];
            voicBox.clip = witnessAudio[Random.Range(0, witnessAudio.Count)];
            voicBox.Play();
        }
        else commentTimer -= Time.deltaTime;

        if (commentTimer <= 0)
        {
            commentText.text = witnessComments[Random.Range(0, witnessComments.Count)];
            commentTimer = commentTimerReset;
        }
        //inState = true;
        currentState = "Leaving";

        stateColour = Color.red;

        

        wayPoint = entrance.transform.position;
        agent.SetDestination(wayPoint);


        //agent.speed *= scaredSpeed;

    }

    public Vector3 GenerateRandomWayPoint()
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
            point = GenerateRandomWayPoint(); // re-roll a position - I'm not happy with this recursion it could be better
        }
        else
        {
            // select a random point on it
            point = Vector3.Lerp(firstVertexPosition, secondVertexPosition, UnityEngine.Random.Range(0.05f, 0.95f));
        }

        return point;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Entrance") && isWitness)
        {
            Debug.Log("Customer has Left");
            GameObject.FindGameObjectWithTag("Player").GetComponent<ServeDrinks>().witnessEscapeCount++;
            GameObject.FindGameObjectWithTag("Player").GetComponent<ServeDrinks>().customerList.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = stateColour;
        Gizmos.DrawWireSphere(transform.position, lookSphere);
    }
}
