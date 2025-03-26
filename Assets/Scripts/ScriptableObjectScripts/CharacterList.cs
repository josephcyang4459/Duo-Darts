using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Character List",menuName ="Single Reference/Character List")]
public class CharacterList : ScriptableObject {
    public Partner[] list;

    public int NumPartners() {
        return (int)CharacterNames.Player;
    }
}
