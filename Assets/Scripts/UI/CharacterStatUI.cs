using UnityEngine;
using TMPro;

public class CharacterStatUI : MonoBehaviour
{
    public static CharacterStatUI inst;
    [SerializeField] CharacterList Characters;
    [SerializeField] Player Player;
    [SerializeField] TMP_Text Text;

    public void Start() {
        if (inst != null) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        inst = this;

    }

    public void UpdateUI() {
        string temp = "";
        temp += "Player: =====\n" + "INTOX: " + Player.Intoxication + "\n" + "SKILL: " + Player.Skill
            + "\n" + "LUCK: " + Player.Luck+"\n";
        for(int i=0;i<4;i++)
        temp += Characters.list[i].Name +": =====\n" + "INTOX: " + Characters.list[i].Intoxication + "\n" + "SKILL: " + Characters.list[i].Composure
          + "\n" + "LOVE: " + Characters.list[i].Love+ "\n";

        Text.text = temp;
    }
}
