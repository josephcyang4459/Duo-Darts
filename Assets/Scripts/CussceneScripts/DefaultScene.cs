using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DefaultScene : CutScene
{
    [System.Serializable]
    public class PresentOptions : block
    {
#if UNITY_EDITOR
        public string name = "present options";
#endif
        public override void action(CutsceneHandler ch)
        {
            ch.PresentChoices();
        }

    }

#if UNITY_EDITOR
    public bool reset;
    private void OnValidate()
    {
        if (reset)
        {
            blocks = new block[1];
            reset = false;
            blocks[0] = new PresentOptions();
        }
    }
#endif
}
