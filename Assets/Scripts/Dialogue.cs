using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour {
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private TypeWriterEffect Script;

    void Start() {
        Script.Run("This is a test. Hope it works.", textLabel);
    }

    public void WriteDialogue(string String) {
        Script.Run(String, textLabel);
    }
}
