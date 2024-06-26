using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterStatUI : MonoBehaviour
{
    public static CharacterStatUI inst;
    [SerializeField] CharacterList Characters;
    [SerializeField] Player Player;
    [SerializeField] Image[] PartnerImages;
    [SerializeField] Color NotYetInteractedWith;

    [Header("TEMP-------------------------------")]
    [SerializeField] TMP_Text PlayerText;
    [Header("TEMP")]
    [SerializeField] TMP_Text[] Texts;

    public void Start() {
        if (inst != null) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        inst = this;

    }

    public void CheckCharacter(int i) {
        PartnerImages[i].color = Color.white;
        if (Characters.list[i].Love < -100) {
            PartnerImages[i].sprite = Characters.list[i].Expressions[(int)Expressions.Negative];
            return;
        }
        if (Characters.list[i].RelatedCutScenes[(int)PartnerCutscenes.FinalScene].completed) {
            PartnerImages[i].sprite = Characters.list[i].Expressions[(int)Expressions.Positive];
            return;
        }
        if (Characters.list[i].Intoxication >= 3) {
            PartnerImages[i].sprite = Characters.list[i].Expressions[(int)Expressions.Drunk];
            return;
        }
        if (Characters.list[i].RelatedCutScenes[0].completed) {
            PartnerImages[i].sprite = Characters.list[i].Expressions[(int)Expressions.Nuetral];
            return;
        }
        PartnerImages[i].color = NotYetInteractedWith;
    }

    public void UpdateUI() {
        for (int i = 0; i < 4; i++) {
            CheckCharacter(i);
        }
        TEMPUI();
    }

    public void TEMPUI() {
        Debug.Log("REMOVE FOR RELEASE");
        string temp = "";
        temp += "Player:\n" + "INTOX: " + Player.Intoxication + "\n" + "SKILL: " + Player.Skill
            + "\n" + "LUCK: " + Player.Luck + "\n";
        PlayerText.text = temp;
        for (int i = 0; i < 4; i++)
            Texts[i].text = Characters.list[i].Name + ":\n" + "INTOX: " + Characters.list[i].Intoxication + "\n" + "SKILL: " + Characters.list[i].Composure
          + "\n" + "LOVE: " + Characters.list[i].Love + "\n";
    }
}
