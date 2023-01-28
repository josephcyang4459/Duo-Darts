using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject button;

    public void Start()
    {
        UI_Helper.SetSelectedUIElement(button);
    }

    public void PlayGame() {
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
