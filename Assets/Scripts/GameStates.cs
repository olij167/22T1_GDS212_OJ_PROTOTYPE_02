using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStates : MonoBehaviour
{
    public GameObject pauseUI, startUI, mainCam, endGameCam, endGameUI, gameUI;

    public GameTime gameClock;

    public ServeDrinks player;

    public TextMeshProUGUI endGameProfitText;

    private void Start()
    {
        pauseUI.SetActive(false);
        gameUI.SetActive(false);
        endGameUI.SetActive(false);
        endGameCam.SetActive(false);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (gameClock.gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        //endGameWitnessText.text = player.witnessText.text;

        endGameProfitText.text = "Earnings = $" + player.profit.ToString("0.00") + "\n" + "Tips = $" + player.tips.ToString("0.00") + "\n" + "Total Profits = $" + (player.tips + player.profit).ToString("0.00");
    }
    public void StartGame()
    {
        
        gameUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        gameClock.gameStarted = true;

        Time.timeScale = 1f;
        startUI.SetActive(false);
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        endGameCam.SetActive(true);
        endGameUI.SetActive(true);
        player.witnessText.transform.parent = endGameUI.transform;
        gameUI.SetActive(false);
        mainCam.SetActive(false);


    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            pauseUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            pauseUI.SetActive(false);

        }
    }
}
