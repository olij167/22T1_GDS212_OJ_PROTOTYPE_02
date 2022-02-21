using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTime : MonoBehaviour
{
    // timer until game over
    //displayed as 5pm to 3am

    public float gameTimerHours, gameTimerMinutes, closeTime, timeIncreaseSpeed;

    public bool gameStarted;

    public GameObject player;

    public TextMeshProUGUI gameTimeUI;

    string hoursString, minutesString;

    void Awake()
    {
        gameStarted = false;
    }


    void Update()
    {
        if (gameStarted)
        {
            gameTimerMinutes += Time.deltaTime * timeIncreaseSpeed;

            if (gameTimerMinutes >= 60f)
            {
                gameTimerHours += 1;
                gameTimerMinutes = 0f;



            }

            if (gameTimerHours > 23)
            {
                gameTimerHours = 0f;
            }

            if (gameTimerHours == 3)
            {
                Time.timeScale = 0;
                //take camera to game over plane


            }

            if (gameTimerHours > 10)
            {
                hoursString = ((int)gameTimerHours).ToString();
            }
            else hoursString = "0" + ((int)gameTimerHours).ToString();


            if (gameTimerMinutes > 10)
            {
                minutesString = ((int)gameTimerMinutes).ToString();
            }
            else minutesString = "0" + ((int)gameTimerMinutes).ToString();

            gameTimeUI.text = hoursString + ":" + minutesString;

            if (gameTimerHours == 3)
            {
                GetComponent<GameStates>().GameOver();
            }
        }

    }
}
