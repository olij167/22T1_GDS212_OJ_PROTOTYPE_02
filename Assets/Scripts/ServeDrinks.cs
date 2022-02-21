using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServeDrinks : MonoBehaviour
{
    public GameObject customer, customerHand, eyes, selectionPos, suckTarget;
    public Camera cam;
    public Image serviceBar, serviceBarBack, thirstBar; //seenImage
    public TextMeshProUGUI serviceText, witnessText, witnessEscapeText, tipsUIValue;
    public float reachDistance;
    
    public List<GameObject> drinksMenu, customerList;

    public AudioSource voiceBox;
    public AudioClip slurpClip, pourClip, lastClip;
    //public SpawnNPC spawnNPC;

    public float profit, tips, maxTip;

    public float suckTime, suckTimeReset, serveTimeReset, thirstTime, thirstReducedValue, thirstFullTime, thirstMinMultiplier, thirstMaxMultiplier = 2, totalCustomerRating, maxCustomerRating, customersServed;
    public int witnessCount, witnessEscapeCount;

    public bool ableToServe, isSucking, isServing, isSeen;


    public Color originalColour, suckedColour, servingBarColor, suckingBarColor;

    private void Start()
    {
        serviceBarBack.enabled = false;
        serviceBar.enabled = false;
        serviceText.enabled = false;
        voiceBox = transform.GetComponent<AudioSource>();
        customerList = new List<GameObject>();
        
        foreach (GameObject customer in GameObject.FindGameObjectsWithTag("NPC"))
        {
            if (customer.transform.parent == null)
            {
                customerList.Add(customer);
            }

            //for (int i = 0; i < GameObject.FindGameObjectsWithTag("NPC").Length; i++)
            //{
            //    if (customer == null && !customerList.Contains(GameObject.FindGameObjectsWithTag("NPC")[i]))
            //    {
            //        customerList.Add(GameObject.FindGameObjectsWithTag("NPC")[i]);
            //    }
            //}
        }

        //totalCustomerRating = maxCustomerRating / 2;
        //seenImage.enabled = false;
    }

    void Update()
    {

        thirstBar.fillAmount = Mathf.Clamp(thirstTime / thirstFullTime, 0, 2f);

        tipsUIValue.text = "Tips = $" + tips.ToString("0.00");

        if (!isSucking)
        {
            thirstTime += Time.deltaTime;
        }

        //if (isSeen)
        //{
        //    seenImage.enabled = true;
        //}
        //else seenImage.enabled = false;
        if (!isServing && !isSucking)
            FindCustomer();

        if (ableToServe && customer != null )
        {
            if (!customer.GetComponent<StateMachine>().isDrinking && customer.GetComponent<StateMachine>().currentState == "Wait at Bar")
            {
                ServeCustomer();
            }
            else Debug.Log("the customer isn't interested");
        }

        if (customer != null)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                AttackCustomer();
            }
        }

        if (isServing)
        {
            if(!voiceBox.isPlaying)
                voiceBox.PlayOneShot(pourClip);

            serviceBarBack.enabled = true;
            serviceBar.enabled = true;
            serviceText.enabled = true;
            
            serviceText.text = "Serving";
            
            suckTime -= Time.deltaTime;

            serviceBar.fillAmount = Mathf.Clamp(suckTime / serveTimeReset, 0, 1f);
            serviceBar.color = servingBarColor;

            if (suckTime <= 0)
            {
                
                int randomDrink = Random.Range(0, drinksMenu.Count + 1);
                Instantiate(drinksMenu[randomDrink], customerHand.transform);
                customer.GetComponent<StateMachine>().isDrinking = true;
                customersServed += 1;
                profit += 8;
                tips += GetTip();
                //customer.GetComponent<StateMachine>().waitTime

                if (customer != null)
                {
                    customer.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = originalColour;
                }

                //voiceBox.enabled = false;
                serviceText.enabled = false;
                serviceBar.enabled = false;
                serviceBarBack.enabled = false;

                Debug.Log("Customer served");
                gameObject.GetComponent<CharacterController>().enabled = true;
                //suckTime = suckTimeReset;
                isServing = false;
            }
        }

        if (isSucking)
        {
            if (!voiceBox.isPlaying)
                voiceBox.PlayOneShot(slurpClip);

            serviceBarBack.enabled = true;
            serviceBar.enabled = true;
            serviceText.enabled = true;

            serviceText.text = "Sucking";

            if (suckTarget == null)
            {
                if (witnessEscapeCount !<= 0)
                    witnessEscapeCount--;
                if (witnessCount !<= 0)
                    witnessCount--;

                gameObject.GetComponent<CharacterController>().enabled = true;

                serviceText.enabled = false;
                serviceBar.enabled = false;
                serviceBarBack.enabled = false;
                isSucking = false;
            }

            suckTarget.GetComponent<ObjectDetection>().enabled = false;

            suckTime -= Time.deltaTime;

            serviceBar.fillAmount = Mathf.Clamp(suckTime / suckTimeReset, 0, 1f);
            serviceBar.color = suckingBarColor;

            //suckTarget.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.Lerp(originalColour, suckedColour, suckTime/ suckTimeReset);



            if (suckTime <= 0)
            {
                //if (thirstTime - thirstReducedValue >= 0)
                //{
                //    thirstTime -= thirstReducedValue;
                //}
                //else
                thirstTime = 0f;
                //voiceBox.enabled = false;

                suckTarget.GetComponent<StateMachine>().isBitten = true;
                //suckTarget.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = suckedColour;

                suckTarget.GetComponent<StateMachine>().agent.enabled = true;
                gameObject.GetComponent<CharacterController>().enabled = true;

                suckTarget.transform.name.Equals("Bitten NPC");
                suckTarget.transform.gameObject.layer = 0;

                serviceText.enabled = false;
                serviceBar.enabled = false;
                serviceBarBack.enabled = false;
                //suckTime = suckTimeReset;
                isSucking = false;
            }
        }

        if (witnessCount > 0)
        {
            witnessText.enabled = true;
            if (witnessCount >= 1)
            {
                witnessText.text = witnessEscapeCount.ToString() + "/" + witnessCount.ToString() + " Witnesses Have Escaped";
            }

            //if (witnessCount > 1)
            //{
            //    witnessText.text = witnessCount.ToString() + " Witnesses";
            //}
        }
        else witnessText.enabled = false;

        //GetCustomerRating();

        //else thirstTime += Time.deltaTime;

        ////selectionPos.transform.position = Input.mousePosition;
        //if (ableToServe)
        //{

        //    FindCustomer();

        //    if (customer != null)
        //    {

        //    }

        //}
        //else
        //{
        //    FindCustomer();

        //    if (customer != null)
        //}
    }

    //void GetCustomerRating(GameObject customer)
    //{
    //    Debug.Log("cubedroot = " + Mathf.Pow(customer.GetComponent<StateMachine>().waitTime, 1 / 3));
    //       customer.GetComponent<StateMachine>().personalRating = maxCustomerRating - (((thirstFullTime - thirstTime)/thirstFullTime)*thirstMaxMultiplier) - (maxCustomerRating / Mathf.Pow(customer.GetComponent<StateMachine>().waitTime, 1/3));
    //        //sumCustomerRating += customer.GetComponent<StateMachine>().personalRating;

    //        totalCustomerRating = ((totalCustomerRating * customersServed) + customer.GetComponent<StateMachine>().personalRating) / (customersServed + 1);

    //    //    customersServed += 1;

             

    //}

    float GetTip()
    {
        float tip = maxTip * ((thirstFullTime - thirstTime) / thirstFullTime) * ((customer.GetComponent<StateMachine>().maxWaitTime - customer.GetComponent<StateMachine>().waitTime) / customer.GetComponent<StateMachine>().maxWaitTime);
        return tip;
    }

    void FindCustomer()
    {
        int layerMask = 1 << 3;
        if (Physics.Raycast(selectionPos.transform.position, selectionPos.transform.TransformDirection(Vector3.forward), out RaycastHit hit, reachDistance, layerMask))
        {

                customer = hit.transform.gameObject;

                Debug.DrawRay(selectionPos.transform.position, transform.TransformDirection(Vector3.forward), Color.magenta);

                customer.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.yellow;
                customerHand = customer.transform.GetChild(0).gameObject;

                Debug.Log("Customer selected");   
            
        }
        else
        {
            Debug.DrawRay(selectionPos.transform.position, selectionPos.transform.TransformDirection(Vector3.forward) * reachDistance, Color.white);

            //if (customer != null)
            //{
            //    customer.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = originalColour;
            //}

            customer = null;
        }
    }

    void ServeCustomer()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            gameObject.GetComponent<CharacterController>().enabled = false;
            suckTime = serveTimeReset;
            isServing = true;
            customer.GetComponent<StateMachine>().commentText.text = "";
            //customer = null;
        }
    }

    void AttackCustomer()
    {
        customer.GetComponent<StateMachine>().wayPoint = customer.transform.position;
        customer.GetComponent<StateMachine>().agent.SetDestination(customer.GetComponent<StateMachine>().wayPoint);
        customer.GetComponent<Rotator>().enabled = false;
        gameObject.GetComponent<CharacterController>().enabled = false;
        suckTime = suckTimeReset;
        suckTarget = customer;
        isSucking = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ServiceArea"))
        {
            ableToServe = true;
        }

        if (other.gameObject.CompareTag("NPC"))
        {
            if (!ableToServe)
            {
                customer = other.gameObject;
                if (Input.GetMouseButtonDown(0))
                {
                    AttackCustomer();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ServiceArea"))
        {
            ableToServe = false;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("NPC"))
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            AttackCustomer();
    //        }
    //    }
    //}
}
