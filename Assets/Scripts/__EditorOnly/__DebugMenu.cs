#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class __DebugMenu : MonoBehaviour
{
    public static __DebugMenu inst;
    [SerializeField] InputActionReference debugKey;
    [SerializeField] Canvas DebugCanvas;
    [SerializeField] TMP_InputField Input;

    [SerializeField] TMP_Text Output;
    [SerializeField] string[] ValidCommands;
    [SerializeField] Button[] QuickCommands;
    [SerializeField] TMP_Text[] QuickCommandText;
    [SerializeField] AudioClipList Sounds;
    [SerializeField] List<string> OutputQueue;
    [SerializeField] int MaxLines;
    
    public void Start() {
        if (inst != null) {
            Destroy(gameObject);
            return;
        }
        inst = this;
        debugKey.action.Enable();
        debugKey.action.performed += debugFuntion;
        DontDestroyOnLoad(this);
        HideQuickCommand();
    }

    void debugFuntion(InputAction.CallbackContext c) {
        DebugCanvas.enabled = !DebugCanvas.enabled;
        if (DebugCanvas.enabled) {
            Input.Select();
        }
    }

    private void OnDestroy() {
        debugKey.action.Disable();
        debugKey.action.performed -= debugFuntion;
    }

    List<string> SearchValid(string current) {
        List<string> temp = new();
        for (int i = 0; i < ValidCommands.Length; i++)
            if (ValidCommands[i].ToLower().Contains(current.ToLower()))
                temp.Add(ValidCommands[i]);
        return temp;
    }

    public void OnInputChange(string s) {
        if (s.Length == 0) {
            HideQuickCommand();
            return;
        }
        if (s.Contains(" ")) {
            HideQuickCommand();
            return;
        }
        
        List<string> temp = SearchValid(s);
        for(int i = 0; i < QuickCommands.Length; i++) {
            if(i<temp.Count) {
                QuickCommands[i].gameObject.SetActive(true);
                QuickCommandText[i].text = temp[i];
            }
            else {
                QuickCommands[i].gameObject.SetActive(false);
            }
        }
    }

    public void HideQuickCommand() {
        for (int i = 0; i < QuickCommands.Length; i++) {
            QuickCommands[i].gameObject.SetActive(false);
        }
    }

    public void UseQuickComand(int i) {
        Input.text = QuickCommandText[i].text+" ";
        Input.Select();
        //SetCommand();
    }

    public void SetCommand() {
        if (Input.text.Length == 0)
            return;
        string[] s = Input.text.Split(' ');
        switch (s[0].ToLower()) {
            case "sound":
                SendToOutput(AllSoundNames());
                break;
            case "playsound":
                if (s.Length > 0)
                    PlaySound(s[1]);
                break;
            default:
                SendToOutput("Unknown Command");
                break;
        }
    }

    public void SendToOutput(string s) {
        string[] lines = s.Split('\n');
        while (OutputQueue.Count + lines.Length > MaxLines) {
            OutputQueue.RemoveAt(0);
        }
        for(int i = 0; i < lines.Length; i++) {
            OutputQueue.Add(lines[i]);
        }
        string forOutPut = "";
        foreach(string outputLine in OutputQueue) {
            forOutPut += outputLine + "\n";
        }
        Output.text = forOutPut;
    }

    public string AllSoundNames() {
        string result = "";
        foreach (AudioClip a in Sounds.List) {
            result += a.name + "\n";
        }
        return result;
    }

    public void PlaySound(string s) {
        foreach(AudioClip a in Sounds.List) {
            if (s.ToLower().CompareTo(a.name.ToLower()) == 0) {
                SendToOutput("Playing Sound "+s);
                Audio.inst.PlayClip(a);
                return;
            }
               
        }
        SendToOutput("Could not find Sound");
    }
}
#endif