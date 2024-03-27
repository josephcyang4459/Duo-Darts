using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterList : ScriptableObject {
    [SerializeField]
    public List<Partner> list;
}
