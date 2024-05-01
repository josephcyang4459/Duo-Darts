using UnityEngine;

public class SaveHandler : MonoBehaviour, Caller
{
    [SerializeField] public static SaveHandler inst;
    [SerializeField] FileUI FileUI;
    [SerializeField] Schedule Schedule;
    [SerializeField] FileHandler FileHandler;
    [SerializeField] CharacterList Characters;
    [SerializeField] Player Player;
    [SerializeField] EventList Events;
    [SerializeField] int SaveFileVersionNumber;
    

    public void Start() {
        inst = this;
    }

    public void BeginShowSaveMenu() {
        FileUI.BeginShowLoadMenu(this);
    }

    public void Ping() {
        System.GC.Collect();
        PauseMenu.inst.SetEnabled(true);
    }

    public void SaveToFile(int fileIndex) {
        SaveFile saveFile = new SaveFile();
        int charactersToSendToFile = (int)CharacterNames.Owner+1;

        saveFile.Hour = Schedule.hour;
        saveFile.Minute = Schedule.minutes;
        if (CutsceneHandler.Instance != null)
            saveFile.MascFemPortraitIndex = CutsceneHandler.Instance.PlayerPortriatIndex();
        else
            saveFile.MascFemPortraitIndex = 0;

        PlayerSaveData tempPlayerData = new PlayerSaveData();
        tempPlayerData.Skill = Player.Skill;
        tempPlayerData.Intoxication = Player.Intoxication;
        tempPlayerData.Luck = Player.Luck;
        tempPlayerData.TotalPoints = Player.TotalPointsScoredAcrossAllDartMatches;
        saveFile.Player = tempPlayerData;

        saveFile.Characters = new PartnerSaveData[charactersToSendToFile];
        for (int i = 0; i < charactersToSendToFile; i++) {
            PartnerSaveData temp = new PartnerSaveData();
            temp.Composure = Characters.list[i].Composure;
            temp.Intoxication = Characters.list[i].Intoxication;
            temp.Love = Characters.list[i].Love;
            int cutscenes = Characters.list[i].RelatedCutScenes.Length;
            temp.CutsceneCompletion = new bool[cutscenes];
            for(int j = 0; j < cutscenes; j++) {
                temp.CutsceneCompletion[j] = Characters.list[i].RelatedCutScenes[j].completed;
            }
            saveFile.Characters[i] = temp;
        }

        saveFile.EventCompletion = new bool[Events.List.Length];
        for(int i = 0; i < Events.List.Length; i++) {
            saveFile.EventCompletion[i] = Events.List[i].done;
        }

        FileHandler.SaveSaveFile(fileIndex, saveFile);
        FileUI.BeginHideLoadMenu();
    }

    public void LoadFromFile(int fileIndex) {
        SaveFile saveFile = FileHandler.LoadSaveFile(fileIndex);
        if (saveFile == null) {
            Debug.Log("SHIT IS FUCKED YO");
            return;
        }

        Schedule.hour = saveFile.Hour;
        Schedule.minutes = saveFile.Minute;
        CutsceneHandler.Instance.SetCharacterSprite(saveFile.MascFemPortraitIndex);
        Player.Skill = saveFile.Player.Skill;
        Player.Intoxication = saveFile.Player.Intoxication;
        Player.Luck = saveFile.Player.Luck;
        Player.TotalPointsScoredAcrossAllDartMatches = saveFile.Player.TotalPoints;
        int charactersFromFile = saveFile.Characters.Length;
        for (int i = 0; i < charactersFromFile; i++) {
            Characters.list[i].Composure =saveFile.Characters[i].Composure;
            Characters.list[i].Intoxication = saveFile.Characters[i].Intoxication;
            Characters.list[i].Love = saveFile.Characters[i].Love;
            int cutscenes = Characters.list[i].RelatedCutScenes.Length;
            for (int j = 0; j < cutscenes; j++) {
                Characters.list[i].RelatedCutScenes[j].completed = saveFile.Characters[i].CutsceneCompletion[j];
            }
        }

        for (int i = 0; i < Events.List.Length; i++) {
            Events.List[i].done = saveFile.EventCompletion[i];
        }
    }

#if UNITY_EDITOR
    [SerializeField] bool __test;
    [SerializeField] int __fileNumber;
    private void OnValidate() {
        if (__test) {
            __test = false;
            SaveToFile(__fileNumber);
        }   
    }
#endif

}
