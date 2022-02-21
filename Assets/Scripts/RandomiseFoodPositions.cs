using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomiseFoodPositions : MonoBehaviour
{
    public List<Transform> foodPosList;
    public List<GameObject> foodPrefabs;

    public ServeDrinks shoppingList;

    GameObject foodToSpawn;

    //public GameObject player, hand;

    void Start()
    {
        //Cursor.visible = true;

        foreach (GameObject food in shoppingList.drinksMenu)
        {
            foodPrefabs.Add(food);
        }

        //foreach (GameObject food in foodPrefabs)
        //{
        //    //food.layer = 3;
        //    //food.AddComponent<MeshCollider>();
        //}

        foreach (GameObject foodPos in GameObject.FindGameObjectsWithTag("FoodPos"))
        {
            foodPosList.Add(foodPos.transform);
        }

        for (int i = 0; i < foodPosList.Count; i++)
        {
            if (foodPosList[i].childCount == 0)
            {
                AssignFood();

                if (foodToSpawn != null && !GameObject.Find(foodToSpawn.name))
                {
                    Instantiate(foodToSpawn, foodPosList[i]);
                }

            }
        }

        

        //player.GetComponent<CharacterController>().enabled = false;
        //player.GetComponent<Rigidbody>().freezeRotation = true;
        //hand.GetComponent<SelectObjects>().enabled = false;


        //Time.timeScale = 0;
        //Cursor.lockState = CursorLockMode.Confined;
        

    }

    public GameObject AssignFood()
    {
        int rand = Random.Range(0, foodPrefabs.Count);

        if (!GameObject.Find(foodPrefabs[rand].name))
        {
            foodToSpawn = foodPrefabs[rand];
        }
        else AssignFood();

        return foodToSpawn;
        
    }
}
