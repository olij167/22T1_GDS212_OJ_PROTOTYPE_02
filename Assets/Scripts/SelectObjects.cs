using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjects : MonoBehaviour
{
    public Camera cam;

    public float reachDistance;
    //public GameObject hand;

    public float minCartSizeRange, maxCartSizeRange;

    public GameObject selectedObject, cartPos;

    public List<GameObject> foodInCart;

    public float foodInCartCost;

    //public Timer timer;

    //public ShoppingList shoppingList;
    

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        int layerMask = 1 << 3;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, reachDistance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.magenta);
                        
            selectedObject = hit.transform.gameObject;

            //Debug.Log("hit = " + selectedObject);
            selectedObject.transform.parent = cartPos.transform;

            selectedObject.GetComponent<BoxCollider>().enabled = false;

            selectedObject.transform.position = new Vector3(cartPos.transform.position.x + Random.Range(minCartSizeRange, maxCartSizeRange), cartPos.transform.position.y, cartPos.transform.position.z + Random.Range(minCartSizeRange, maxCartSizeRange));

            if (!gameObject.GetComponent<AudioSource>().isPlaying)
            {
                gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip);
            }

            foodInCart.Add(selectedObject);

            //timer.receiptText.text += "\n" + selectedObject.name.Replace("(Clone)", "   ...    " + "$" + selectedObject.GetComponent<FoodStats>().price.ToString());
            //foodInCartCost += selectedObject.GetComponent<FoodStats>().price;

            //if (!shoppingList.itemsRetrieved)
            //{
            //    shoppingList.CheckItemsInCart();
            //}

            //shoppingList.CheckItemsInCart();

            selectedObject.gameObject.layer = 0;

            selectedObject = null;
            
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }
        //else selectedObject = null;

        //ray = cam.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out hit, Mathf.Infinity, default))
        //{
        //    hand.transform.position = hit.transform.position;
        //    selectedObject = hit.transform.gameObject;
        //}

        //Debug.Log(selectedObject);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.magenta;
    //    Gizmos.DrawRay(ray.origin, ray.direction);
    //}
}
