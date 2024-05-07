using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialHandler : MonoBehaviour {
    [SerializeField] public static TutorialHandler inst;
    [SerializeField] InputActionReference PauseInput;
    [SerializeField] Canvas DartsTutorialCanvas;
    [SerializeField] Canvas StoryTutorialCanvas;
    [SerializeField] Canvas TutorialChoicesCanvas;
    [SerializeField] DartGame dartgame;

    public void Start() {
        inst = this;
        PauseInput.action.performed += DisableTutorials;

        string currentSceneName = SceneManager.GetActiveScene().name;
        if (dartgame.firstTimePlaying) {
            switch(currentSceneName) {
                case "Story":
                    EnableStoryTutorial(true);
                    break;
                case "Darts":
                    EnableDartsTutorial(true);
                    break;
                default:
                    break;
            }
        }
    }

    public void OnDestroy() { PauseInput.action.performed -= DisableTutorials; }

    // Enable/Disable Tutorial Canvas'
    public void EnableStoryTutorial(bool enable) { StoryTutorialCanvas.enabled = enable; }

    public void EnableDartsTutorial(bool enable) { DartsTutorialCanvas.enabled = enable; }

    public void EnableTutorialChoices(bool enable) { TutorialChoicesCanvas.enabled = enable; }

    public void DisableTutorials(InputAction.CallbackContext c) {
        StoryTutorialCanvas.enabled = false;
        DartsTutorialCanvas.enabled = false;
        TutorialChoicesCanvas.enabled = false;
    }
}
