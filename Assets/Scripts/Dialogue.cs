using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour {
    [SerializeField] private TMP_Text textLabel;
    public TypeWriterEffect Script;

    public void WriteDialogue(string String) {
        Script.Run(String, textLabel);
    }
}
