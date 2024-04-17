using UnityEngine;

public class TutorialHandler : MonoBehaviour {
    [SerializeField] public static TutorialHandler inst;
    [SerializeField] Canvas DartsTutorialCanvas;
    [SerializeField] Canvas StoryTutorialCanvas;
    [SerializeField] Canvas TutorialChoicesCanvas;
    [SerializeField] DartGame dartgame;

    public void Start() { inst = this; }

    // Enable/Disable Tutorial Canvas
    public void EnableDartsTutorial(bool enable) {
        DartsTutorialCanvas.enabled = enable;
        dartgame.setFirstTimePlaying(false);
        bool anyOptionsEnabled = (!PauseMenu.inst.PauseOptionsCanvas.enabled && !PauseMenu.inst.StoryOptionsCanvas.enabled);

        if (!enable && anyOptionsEnabled)
            dartgame.BeginGame();
    }

    public void EnableStoryTutorial(bool enable) { StoryTutorialCanvas.enabled = enable; }

    public void EnableTutorialChoices(bool enable) { TutorialChoicesCanvas.enabled = enable;  }
}
