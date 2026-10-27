using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Manager : MonoBehaviour
{
    [HideInInspector] public bool isPaused = false;
    
    private float _memoryTimeScale = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SessionData.PlayType = "none";
        SessionData.Plays = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Tell the settings where the main game pause/resume functions are
    public void PauseGame()
    {
        _memoryTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = _memoryTimeScale;
        isPaused = false;
    }

    public void ToggleGameState()
    {
        if (isPaused == true)
        {
            ResumeGame();
        }
        else if (isPaused == false)
        {
            PauseGame();
        }
    }

    public void ResetScene()
    {
        int buildNum = SceneManager.GetActiveScene().buildIndex;
        Time.timeScale = 1f;
        SceneManager.LoadScene(buildNum);
    }

    public void GoToScene(int sceneNumber)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneNumber);
    }

    public void StartGame(string arguments)
    {
        Time.timeScale = 1f;
        SessionData.PlayType = arguments.ToLower();
        SceneManager.LoadScene(1);
    }

    public void CloseGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
