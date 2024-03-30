using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sprite Collection", menuName = "Reference/List/Sprite Collection")]
public class SpriteCollection : ScriptableObject
{
    public Sprite[] Sprites;
    public Sprite GetSprite(int index)
    {
        return Sprites[index];
    }
}
