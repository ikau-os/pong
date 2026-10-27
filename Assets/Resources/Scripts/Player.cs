using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip scoreSound;

    [Space(22)]
    public AudioClip pauseMusic;

    [Header("Properties")]
    public float platformSpeed = 5f;

    [Header("Objects")]
    public GameObject leftPlatform;
    public GameObject rightPlatform;
    public GameObject ball;
    public GameObject pauseMenu;
    public GameObject winnerUI;

    [Header("Components")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winTitleText;

    [Header("Other Scripts")]
    public Manager manager;
    public Ball ballScript;

    private bool playBot = false;

    private int leftScore = 0;
    private int rightScore = 0;
    private int tries = 1;
    private int numOfTries = 5;

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = leftScore.ToString() + " - " + rightScore.ToString();
        numOfTries = SessionData.Plays + 1;

        Debug.Log(numOfTries.ToString());
        Debug.Log(SessionData.Plays.ToString());
        Debug.Log(SessionData.PlayType.ToString());

        audioSource = GetComponent<AudioSource>();

        if (SessionData.PlayType == "bot")
        {
            playBot = true;
        }
        else if (SessionData.PlayType == "player")
        {
            playBot = false;
        }
        else
        {
            manager.GoToScene(0); // if somehow the game starts w/o value then go back to title screen
        }
    }

    // Update is called once per frame
    void Update()
    {
        float moveAmount = platformSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W) || (playBot && Input.GetKey(KeyCode.UpArrow)))
        {
            leftPlatform.transform.position += new Vector3(0f, moveAmount, 0f);
        }
        else if (Input.GetKey(KeyCode.S) || (playBot && Input.GetKey(KeyCode.DownArrow)))
        {
            leftPlatform.transform.position -= new Vector3(0f, moveAmount, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseMenu();
        }

        Vector3 leftPos = leftPlatform.transform.position;
        leftPos.y = Mathf.Clamp(leftPos.y, -4f, 4f);
        leftPlatform.transform.position = leftPos;
        
        if (playBot)
        {
            Vector2 rightPos = rightPlatform.transform.position;
            Vector2 ballPos = ball.transform.position;

            float newY = Mathf.MoveTowards(rightPos.y, ballPos.y, moveAmount);

            rightPlatform.transform.position = new Vector2(rightPos.x, newY);
        }
        else if (!playBot)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rightPlatform.transform.position += new Vector3(0f, moveAmount, 0f);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                rightPlatform.transform.position -= new Vector3(0f, moveAmount, 0f);
            }

            Vector3 rightPos = rightPlatform.transform.position;
            rightPos.y = Mathf.Clamp(rightPos.y, -4f, 4f);
            rightPlatform.transform.position = rightPos;
        }
    }

    public void AddScore(string leftOrRight)
    {
        if (tries < numOfTries)
        {
            if (leftOrRight == "left")
            {
                rightScore++;
            }
            else if (leftOrRight == "right")
            {
                leftScore++;
            }

            scoreText.text = leftScore.ToString() + " - " + rightScore.ToString();

            audioSource.clip = scoreSound;
            audioSource.Play();

            ballScript.SetBall();
            tries++;
        }
        else
        {
            ballScript.SetBall();
            Time.timeScale = 0f;

            if (leftScore > rightScore)
            {
                winTitleText.text = "Right Won!";
            }
            else if (rightScore > leftScore)
            {
                winTitleText.text = "Left Won!";
            }

            winnerUI.SetActive(true);
        }
    }

    public void TogglePauseMenu()
    {
        if (!manager.isPaused)
        {
            manager.PauseGame();
            audioSource.clip = pauseMusic;
            pauseMenu.SetActive(true);
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (manager.isPaused)
        {
            manager.ResumeGame();
            audioSource.Stop();
            pauseMenu.SetActive(false);
            audioSource.clip = null;
            audioSource.loop = false;
        }
    }
}
