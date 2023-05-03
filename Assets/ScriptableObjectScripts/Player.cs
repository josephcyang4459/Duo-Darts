using UnityEngine;

[CreateAssetMenu]
public class Player : ScriptableObject {
    public string Name;
    public float Charisma = 1f;
    public float Intoxication = 1f;
    public float Skill = 1f;
    public float Luck = 1f;

    public int TotalPointsScoredAcrossAllDartMatches;

    public void UpdateAttribute(string attribute, float value) {
        attribute = attribute.ToLower();
        switch (attribute) {
            case "charisma":
                Charisma += value;
                break;
            case "intoxication":
                Intoxication += value;
                break;
            case "skill":
                Skill += value;
                break;
            case "luck":
                Luck += value;
                break;
            default:

#if UNITY_EDITOR
                Debug.Log("NOT A SKILL "+attribute);
#endif
                break;
        }
    }

    public void UpdateAttribute(PlayerSkills attribute, float value)
    {
        switch (attribute)
        {
            case PlayerSkills.Charisma:
                Charisma += value;
                break;
            case PlayerSkills.Intoxication:
                Intoxication += value;
                break;
            case PlayerSkills.Skill:
                Skill += value;
                break;
            case PlayerSkills.Luck:
                Luck += value;
                break;
            default:

#if UNITY_EDITOR
                Debug.Log("NOT A SKILL " + attribute);
#endif
                break;
        }
    }
}
