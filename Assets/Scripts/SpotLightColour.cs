using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightColour : MonoBehaviour
{
    public float changeColourTimer, timerReset;
    public Color lightColour1, lightColour2;
    void Start()
    {
        ChangeColours();
        timerReset = changeColourTimer;
    }

    // Update is called once per frame
    void Update()
    {
        changeColourTimer -= Time.deltaTime;

        if (changeColourTimer <= 0)
        {
            ChangeColours();
            changeColourTimer = timerReset;
        }
    
        
        transform.GetComponent<Light>().color = Color.Lerp(lightColour1, lightColour2, Mathf.PingPong(Time.deltaTime, 1));

    }

    void ChangeColours()
    {
        lightColour1 = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.75f, 1f);
        lightColour2 = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.75f, 1f);
    }
}
