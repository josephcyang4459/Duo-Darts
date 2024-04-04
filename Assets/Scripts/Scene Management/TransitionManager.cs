using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour, Caller {
    public static TransitionManager inst;
    [SerializeField] AnimationState State;
    [SerializeField] SceneNumbers NextScene;
    [SerializeField] SceneEntrance CurrentSceneEntrance;
    [SerializeField] SceneTransitionAnimation[] TransitionAnimationHeads;
    [SerializeField] int FileIndex;
    void Awake() {
        if (inst != null) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        inst = this;
    }

    public void SetFileIndex(int index) {
        FileIndex = index;
    }

    public int GetFileIndex() {
        return FileIndex;
    }

    public void GoToScene(SceneNumbers nextScene) {
        Audio.inst.StopSong();
        PauseMenu.inst.SetEnabled(false);
        UIState.inst.SetInteractable(false);
        DartSticker.inst.SetVisible(false);
        NextScene = nextScene;
        State = AnimationState.Exiting;
        TransitionAnimationHeads[(int)NextScene].Enter(this);
    }

    public void ReadyToEnterScene(SceneEntrance entrance) {
        
        CurrentSceneEntrance = entrance;
        State = AnimationState.Entering;
        TransitionAnimationHeads[(int)NextScene].Exit(this);
    }

    void EnterScene() {
        TransitionAnimationHeads[(int)NextScene].TransitionCanvas.enabled = false;
        PauseMenu.inst.IsInStoryScene(NextScene == SceneNumbers.Story);
        PauseMenu.inst.SetEnabled(true);
        UIState.inst.SetInteractable(true);
        CurrentSceneEntrance.EnterScene();
    }

    void ExitScene() {
        System.GC.Collect();
        SceneManager.LoadScene((int)NextScene);
    }

    public void Ping() {
        Debug.Log("DOne");
        switch (State) {
            case AnimationState.Entering: EnterScene();break;
            case AnimationState.Exiting: ExitScene(); break;
        }
        State = AnimationState.Passive;
    }

    public SceneNumbers GetCurrentScene() {
        return NextScene;
    }

    [System.Serializable]
    class SceneTransitionAnimation {
#if UNITY_EDITOR
        public string __name;
#endif
        public Canvas TransitionCanvas;
        public UIAnimationElement EnterAnimationHead;
        public UIAnimationElement ExitAnimationHead;

        public void Enter(Caller c)
        {
            TransitionCanvas.enabled = true;
            EnterAnimationHead.Begin(c);
        }

        public void Exit(Caller c) {
            TransitionCanvas.enabled = true;
            ExitAnimationHead.Begin(c);
        }
    }

    enum AnimationState {
        Passive,
        Entering,
        Exiting
    }
    /*
#if UNITY_EDITOR
    //[SerializeField] bool __reset;

    int numberOfScenes;

    void OnValidate() {
        if (__reset) {
            __reset = false;
            TransitionAnimationHeads = new SceneTransitionAnimation[SceneManager.sceneCountInBuildSettings];
            for(int i=0;i<TransitionAnimationHeads.Length;i++) {
                TransitionAnimationHeads[i] = new();
                TransitionAnimationHeads[i].__name = ((SceneNumbers)i).ToString();
            }
        }
    }
#endif
    */
}
