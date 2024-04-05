using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScene : MonoBehaviour, SceneEntrance
{
    

    private void Start() {
        TransitionManager.inst.ReadyToEnterScene(this);
    }
    public void EnterScene() {

    }
}
