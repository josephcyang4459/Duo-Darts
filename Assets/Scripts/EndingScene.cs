using UnityEngine;
using UnityEngine.UI;

public class EndingScene : MonoBehaviour, SceneEntrance
{
    [SerializeField] FileHandler FileHandler;
    [SerializeField] CutScene EndingCutscene;
    [SerializeField] Image Background;
    [SerializeField] Achievement[] EndingAchievements;
    private void Start() {
        TransitionManager.inst.ReadyToEnterScene(this);
    }

    public int GetCurrentEnding() {
        SceneNumbers temp = TransitionManager.inst.GetCurrentScene();
        switch (temp) {
            case SceneNumbers.ChadEnding: return 0;
            case SceneNumbers.JessEnding: return 1;
            case SceneNumbers.FayeEnding: return 2;
            case SceneNumbers.ElaineEnding: return 3;
        }
        return -1;
    }


    public void EnterScene() {
        int currentEnding = GetCurrentEnding();
        if (currentEnding == -1)
            return;
        EndingAchievements[currentEnding].TrySetAchievement(true);
        Achievements.Instance.SaveLocalToDisk();
        CutsceneHandler.Instance.SetUpForEnding(this);
        CutsceneHandler.Instance.PlayCutScene(EndingCutscene, (int)Locations.darts);
    }

    public void CutsceneComplete() {
        CutsceneHandler.Instance.HideUI();
        Background.sprite = CutsceneHandler.Instance.cutSceneBackGround.sprite;
        TransitionManager.inst.GoToScene(SceneNumbers.MainMenu);
    }
}
