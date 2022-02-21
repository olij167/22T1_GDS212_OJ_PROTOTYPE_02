using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 1f;
    public float x, y, z;

    float changeDirectionTimeRemaining = 0.5f, changeDirectionTimeReset = 0.5f;

    bool changeDirection;

    public bool randomiseX, randomiseY, randomiseZ;

    public float minRandomRange, maxRandomRange;

    public 

    void Start()
    {
        
    }

    void Update()
    {
        if (changeDirection)
        {
            if (randomiseX)
                x = Random.Range(minRandomRange, maxRandomRange);
            if (randomiseY)
                y = Random.Range(minRandomRange, maxRandomRange);
            if (randomiseZ)
                z = Random.Range(minRandomRange, maxRandomRange);

            changeDirection = false;
        }

        if (changeDirectionTimeRemaining > 0)
        {
            changeDirectionTimeRemaining -= Time.deltaTime;
        }

        else if (changeDirectionTimeRemaining <= 0)
        {
            changeDirection = true;
            changeDirectionTimeRemaining = changeDirectionTimeReset;
        }


        gameObject.transform.Rotate(x * speed * Time.deltaTime, y * speed * Time.deltaTime, z * speed * Time.deltaTime);
    }
}
