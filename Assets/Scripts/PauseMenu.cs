using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("What key triggers the menu to pop-up?")]
    public KeyCode key;

    [Header("Does time stop when paused?")]
    public bool timeStop = true;

    [Header("GameObject that turns on when paused (UI)")]
    public GameObject pauseScreen;

    bool paused = false;

    void Start()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            paused = !paused;
            pause(paused);
        }
    }

    void pause(bool p)
    {
        if (p)  // pauses
        {
            if (pauseScreen) pauseScreen.SetActive(true);
            if (timeStop) Time.timeScale = 0;
        }
        else    // upauses
        {
            if (pauseScreen) pauseScreen.SetActive(false);
            if (timeStop) Time.timeScale = 1;
        }
    }

    // for a UI button
    public void pauseButton()
    {
        paused = !paused;
        pause(paused);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
