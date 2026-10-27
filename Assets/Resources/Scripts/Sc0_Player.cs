using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Sc0_Player : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip titleScreenMusic;

    [Header("Objects")]
    public GameObject[] firstSetUI;
    public GameObject[] secondSetUI;

    [Space(22)]
    public TMP_InputField playInput;

    [Header("Other Scripts")]
    public Manager manager;

    private AudioSource audioSource;

    private string playType = "none";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = titleScreenMusic;
        audioSource.loop = true;
        audioSource.Play();

        foreach (GameObject gameObject in firstSetUI)
        {
            gameObject.SetActive(true);
        }

        foreach (GameObject gameObject in secondSetUI)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoBack()
    {
        SessionData.Plays = 0;
        SessionData.PlayType = "none";

        foreach (GameObject gameObject in firstSetUI)
        {
            gameObject.SetActive(true);
        }

        foreach (GameObject gameObject in secondSetUI)
        {
            gameObject.SetActive(false);
        }
    }

    public void GetPlays()
    {
        foreach (GameObject gameObject in firstSetUI)
        {
            gameObject.SetActive(false);
        }

        foreach (GameObject gameObject in secondSetUI)
        {
            gameObject.SetActive(true);
        }
    }

    public void PlayBot()
    {
        SessionData.PlayType = "bot";
        GetPlays();
    }

    public void PlayPlayer()
    {
        SessionData.PlayType = "player";
        GetPlays();
    }

    public void InitGame()
    {
        Debug.Log(playType.ToString());
        string playsString = playInput.text;
        int playsInt = 0;

        if (int.TryParse(playsString, out int result))
        {
            if (result % 2 == 0)
            {
                result++;
            }

            playsInt = result;
        }

        SessionData.Plays = playsInt;
        manager.StartGame(playType.ToString());
    }
}
